

using app.Domain.Common;

namespace app.Domain.Entities;

public class ExpertHomeService : BaseEntity
{
    public int ExpertId { get; set; }
    public int HomeServiceId { get; set; }

    public Expert Expert { get; set; }
    public HomeService HomeService { get; set; }
}