using app.Domain.Common;
using app.Domain.HomeServiceAgg.Entities;
using app.Domain.UserAgg.Entities;

namespace app.Domain.ExpertHomeServiceAgg.Entities;

public class ExpertHomeService : BaseEntity
{
    public int ExpertId { get; set; }
    public int HomeServiceId { get; set; }

    public Expert Expert { get; set; }
    public HomeService HomeService { get; set; }
}