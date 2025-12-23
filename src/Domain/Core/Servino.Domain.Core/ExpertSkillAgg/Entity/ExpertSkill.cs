using Servino.Domain.Core._common;
using Servino.Domain.Core.HomeServiceAgg.Entity;
using Servino.Domain.Core.UserAgg.Entity;

namespace Servino.Domain.Core.ExpertSkillAgg.Entity;

public class ExpertSkill : BaseEntity
{
    public int ExpertId { get; set; }
    public int HomeServiceId { get; set; }

    public Expert Expert { get; set; }
    public HomeService HomeService { get; set; }
}