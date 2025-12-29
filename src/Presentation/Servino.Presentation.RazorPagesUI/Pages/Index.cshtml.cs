using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;

namespace Servino.Presentation.RazorPagesUI.Pages
{
    public class IndexModel(AppDbContext context) : PageModel
    {
        //for checking ui
        public List<Category> Categories { get; set; }
        public void OnGet()
        {
            Categories = context.Categories.ToList();
        }
    }
}
