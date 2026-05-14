using app.Domain._common;
using app.Domain.CommentAgg.Entities;
using app.Domain.ExpertHomeServiceAgg.Entities;
using app.Domain.SuggestionAgg.Entities;

namespace app.Domain.UserAgg.Entities;

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