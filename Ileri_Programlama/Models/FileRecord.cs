namespace Ileri_Programlama.Models;

public class FileRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string PathFile { get; set; } = string.Empty;
    public Guid? QuestionId { get; set; }

    // İlişkiler
    public User User { get; set; } = null!;
    public Question? Question { get; set; }
}