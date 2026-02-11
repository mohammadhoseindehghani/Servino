using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Servino.Domain.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Service;
using Servino.Framework.Caching;
using Servino.Infa.DataAccess.Repo.Dapper.Repositories;
using Servino.Infa.DataAccess.Repo.EfCore.Repositories;
using Servino.Infa.Db.SqlServer.EfCore.DataSeed;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infa.Db.SqlServer.EfCore.Identity.Service;
using Servino.Infra.Providers.SmsProvider.SmsIrService;
using Servino.Infrastructure.BackgroundJobs;
using Servino.Presentation.RazorPagesUI.Configurations;
using Servino.Presentation.RazorPagesUI.CustomMiddleware;
using Servino.Presentation.RazorPagesUI.Services.File;
using System.Data;

var builder = WebApplication.CreateBuilder(args);



builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<SiteSettings>>().Value);


builder.Services.AddDbContext<AppDbContext>((sp, options) =>
{
    var siteSettings = sp.GetRequiredService<SiteSettings>();
    options.UseSqlServer(siteSettings.ConnectionStrings.SqlConnection);
});


builder.Services.AddScoped<IDbConnection>(sp =>
{
    var siteSettings = sp.GetRequiredService<SiteSettings>();  
    return new SqlConnection(siteSettings.ConnectionStrings.SqlConnection); 
});


builder.Services.AddHangfire(config =>
{
    var siteSettings = builder.Services.BuildServiceProvider().GetRequiredService<SiteSettings>();

    config.UseSqlServerStorage(siteSettings.ConnectionStrings.SqlConnection); 
});


builder.Services.AddHangfireServer();

builder.Services.AddHangfireServer();



builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddClaimsPrincipalFactory<AppUserClaimsPrincipalFactory>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login"; 
    options.AccessDeniedPath = "/Auth/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<DbInitializer>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddRazorPages();




builder.Services.AddScoped<RequestReminderJob>();



builder.Services.AddDapperRepositoryServices(builder.Configuration);

builder.Services.AddEfRepositoryService(builder.Configuration);

builder.Services.AddScoped<IIdentityService, IdentityService>();

builder.Services.AddServiceServices(builder.Configuration);

builder.Services.AddApplicationServices(builder.Configuration);




builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ISmsService>(provider =>
    new SmsIrService("Hsglr29hTKTzz9k4F9mDFVdFQMkllYkxv3VV5wDBilPyljbp"));

builder.Services.AddStackExchangeRedisCache(options =>
{
    var siteSettings = builder.Services.BuildServiceProvider().GetRequiredService<SiteSettings>();
    options.Configuration = siteSettings.Redis.Configuration;
    options.InstanceName = siteSettings.Redis.InstanceName;
});


var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();



app.UseHangfireDashboard("/hangfire");

using (var scope = app.Services.CreateScope())
{
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

    recurringJobManager.AddOrUpdate<RequestReminderJob>(
        "check-requests-without-suggestion",
        job => job.CheckRequestsWithoutSuggestion(JobCancellationToken.Null),
        Cron.Weekly); // Adjust frequency as needed
}




using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<DbInitializer>();
        await initializer.SeedAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication(); 
app.UseAuthorization();  

app.MapRazorPages();

app.Run();