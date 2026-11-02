using Ileri_Programlama.Models;

namespace Ileri_Programlama.Models;

public class SessionUser : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid SessionId { get; set; }
    public bool IsTemp { get; set; }

    // İlişkiler
    public User? User { get; set; }
    public Session Session { get; set; } = null!;
    public ICollection<SessionAnswer> SessionAnswers { get; set; } = new List<SessionAnswer>();
}