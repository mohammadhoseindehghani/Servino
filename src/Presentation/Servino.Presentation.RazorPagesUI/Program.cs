using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Servino.Domain.AppService;
using Servino.Domain.AppService.UserAgg;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Contracts.Data;
using Servino.Domain.Core.CategoryAgg.Contracts.Service;
using Servino.Domain.Core.CommentAgg.Contracts.AppService;
using Servino.Domain.Core.CommentAgg.Contracts.Data;
using Servino.Domain.Core.CommentAgg.Contracts.Service;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.ExpertHomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.HomeServiceAgg.Contracts.AppService;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Data;
using Servino.Domain.Core.HomeServiceAgg.Contracts.Service;
using Servino.Domain.Core.LocationAgg.Contracts.AppService;
using Servino.Domain.Core.LocationAgg.Contracts.Data;
using Servino.Domain.Core.LocationAgg.Contracts.Service;
using Servino.Domain.Core.RequestAgg.Contracts.AppService;
using Servino.Domain.Core.RequestAgg.Contracts.Data;
using Servino.Domain.Core.RequestAgg.Contracts.Service;
using Servino.Domain.Core.SuggestionAgg.Contracts.AppService;
using Servino.Domain.Core.SuggestionAgg.Contracts.Data;
using Servino.Domain.Core.SuggestionAgg.Contracts.Service;
using Servino.Domain.Core.UserAgg.Contracts.AppService;
using Servino.Domain.Core.UserAgg.Contracts.Data;
using Servino.Domain.Core.UserAgg.Contracts.Service;
using Servino.Domain.Service;
using Servino.Infa.DataAccess.Repo.EfCore.Repositories;
using Servino.Infa.Db.SqlServer.EfCore.DataSeed;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infa.Db.SqlServer.EfCore.Identity.Service;
using Servino.Infra.Providers.SmsProvider.SmsIrService;
using Servino.Presentation.RazorPagesUI.CustomMiddleware;
using Servino.Presentation.RazorPagesUI.Services.File;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

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

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IHomeServiceRepository, HomeServiceRepository>();
builder.Services.AddScoped<IExpertRepository, ExpertRepository>();
builder.Services.AddScoped<IExpertHomeServiceRepository, ExpertHomeServiceRepository>();
builder.Services.AddScoped<IProvinceRepository, ProvinceRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ISuggestionRepository, SuggestionRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();


builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IHomeServiceService, HomeServiceService>();
builder.Services.AddScoped<IExpertHomeServiceService, ExpertHomeServiceService>();
builder.Services.AddScoped<IExpertService, ExpertService>();
builder.Services.AddScoped<IProvinceService, ProvinceService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISuggestionService, SuggestionService>();
builder.Services.AddScoped<IAdminService, AdminService>();


builder.Services.AddScoped<IUserAppService, UserAppService>();
builder.Services.AddScoped<ICommentAppService, CommentAppService>();
builder.Services.AddScoped<ICategoryAppService, CategoryAppService>();
builder.Services.AddScoped<IHomeServiceAppService, HomeServiceAppService>();
builder.Services.AddScoped<IExpertAppService, ExpertAppService>();
builder.Services.AddScoped<IProvinceAppService, ProvinceAppService>();
builder.Services.AddScoped<ICityAppService, CityAppService>();
builder.Services.AddScoped<IRequestAppService, RequestAppService>();
builder.Services.AddScoped<ICustomerAppService, CustomerAppService>();
builder.Services.AddScoped<ISuggestionAppService, SuggestionAppService>();
builder.Services.AddScoped<IAdminAppService, AdminAppService>();


builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<ISmsService>(provider =>
    new SmsIrService("Hsglr29hTKTzz9k4F9mDFVdFQMkllYkxv3VV5wDBilPyljbp")); 

var app = builder.Build();
app.UseMiddleware<RequestLoggingMiddleware>();

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