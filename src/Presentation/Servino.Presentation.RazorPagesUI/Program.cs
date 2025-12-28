using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servino.Domain.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Contracts.Data;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Service;
using Servino.Infa.DataAccess.Repo.EfCore.Repositories;
using Servino.Infa.Db.SqlServer.EfCore.DataSeed;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infa.Db.SqlServer.EfCore.Identity.Service;
using Servino.Presentation.RazorPagesUI.Services.File;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 4;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>() 
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IHomeServiceRepository, HomeServiceRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();



builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IHomeServiceService, HomeServiceService>();



builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<ICommentAppService, CommentAppService>();
builder.Services.AddScoped<ICategoryAppService, CategoryAppService>();
builder.Services.AddScoped<IHomeServiceAppService, HomeServiceAppService>();



builder.Services.AddScoped<IFileService, FileService>();



var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<DbInitializer>();
        await initializer.SeedAsync(); 

        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database initialization completed successfully.");
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

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
