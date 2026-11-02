using Ileri_Programlama.Models;

namespace Ileri_Programlama.Models;

public class SessionAnswer : BaseEntity
{
    public Guid SessionUserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid? AnswerId { get; set; } // Seçilen seçeneğin GUID'i

    // İlişkiler
    public SessionUser SessionUser { get; set; } = null!;
    public Question Question { get; set; } = null!;
    public Secenek? SelectedOption { get; set; }
}