using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.ExpertHomeServiceAgg.Entity;

public class ExpertHomeService : BaseEntity
{
    public int ExpertId { get; set; }
    public int HomeServiceId { get; set; }

    public Expert Expert { get; set; }
    public HomeService HomeService { get; set; }
}