using Ileri_Programlama.Models;

namespace Ileri_Programlama.Models;

public class Session : BaseEntity
{
    public Guid QuizerId { get; set; }
    public Guid QuizId { get; set; }
    public SessionStatus Status { get; set; } = SessionStatus.Pending;
    public SessionType SessionType { get; set; } = SessionType.Puanli;
    public DateTime? FinishedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // İlişkiler
    public User Quizer { get; set; } = null!;
    public Quiz Quiz { get; set; } = null!;
    public ICollection<SessionUser> SessionUsers { get; set; } = new List<SessionUser>();
}