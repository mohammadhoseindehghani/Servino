using Servino.Domain.Core._common;
using Servino.Domain.Core.CommentAgg.Entity;
using Servino.Domain.Core.ExpertHomeServiceAgg.Entity;
using Servino.Domain.Core.SuggestionAgg.Entity;

namespace Servino.Domain.Core.UserAgg.Entity;

public class Expert : BaseEntity
{
    public int UserId { get; set; }

    public string? Bio { get; set; } 
    public string? Address { get; set; }
    public string? BankCardNumber { get; set; }
    public string? ShebaNumber { get; set; }
    public decimal? AverageScore { get; set; } 

    public User User { get; set; }
    public ICollection<ExpertHomeService> ExpertHomeServices { get; set; }
    public ICollection<Suggestion> Suggestions { get; set; }
    public ICollection<Comment> CommentsReceived { get; set; }

    public Expert()
    {
        ExpertHomeServices = new List<ExpertHomeService>();
        Suggestions = new List<Suggestion>();
        CommentsReceived = new List<Comment>();
    }
}