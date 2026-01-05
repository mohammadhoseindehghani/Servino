using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Servino.Domain.Core.CategoryAgg.Contracts.AppService;
using Servino.Domain.Core.CategoryAgg.Dtos;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Infa.Db.SqlServer.EfCore.DbContexts;
using Servino.Infra.Providers.SmsProvider.SmsIrService;

namespace Servino.Presentation.RazorPagesUI.Pages
{
    public class IndexModel(ICategoryAppService categoryAppService) : PageModel
    {
        public List<CategoryClientDto> Categories { get; set; } = [];

        public async Task OnGet(CancellationToken ct)
        {
            Categories = await categoryAppService.GetCategoriesByParentIdAsync(null, ct);
        }
    }
}
