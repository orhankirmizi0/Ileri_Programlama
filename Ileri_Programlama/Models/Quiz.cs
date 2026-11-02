using Ileri_Programlama.Models;

namespace Ileri_Programlama.Models;

public class Quiz : BaseEntity
{
    public Guid UserId { get; set; }
    public PuanlamaTuru Puanlama { get; set; }
    public DateTime? DeletedAt { get; set; }

    // İlişkiler
    public User User { get; set; } = null!;
    public ICollection<Question> Questions { get; set; } = new List<Question>();
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}