namespace Ileri_Programlama.Models;

public class Secenek
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuestionId { get; set; }
    public string? Desc { get; set; }
    public int Queue { get; set; }
    public bool IsTrue { get; set; }

    // İlişki
    public Question Question { get; set; } = null!;
}