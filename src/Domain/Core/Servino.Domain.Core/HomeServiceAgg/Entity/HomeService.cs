using Servino.Domain.Core._common;
using Servino.Domain.Core.CategoryAgg.Entity;
using Servino.Domain.Core.ExpertSkillAgg.Entity;
using Servino.Domain.Core.RequestAgg.Entity;

namespace Servino.Domain.Core.HomeServiceAgg.Entity;

public class HomeService : BaseEntity
{
    public string Title { get; set; }
    public string? ShortDescription { get; set; }
    public decimal BasePrice { get; set; } 
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
    public long VisitCount { get; set; } 

    public int CategoryId { get; set; }

    public Category Category { get; set; }
    public ICollection<ExpertSkill> ExpertSkills { get; set; } 
    public ICollection<Request> Requests { get; set; } 
}