using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infra.Providers.SmsProvider.SmsIrService;

namespace Servino.Presentation.RazorPagesUI.Pages
{
    public class IndexModel(AppDbContext context,ISmsService sms) : PageModel
    {
        //for checking ui
        public List<Category> Categories { get; set; }
        public void OnGet()
        {
            Categories = context.Categories.ToList();
        }
    }
}
