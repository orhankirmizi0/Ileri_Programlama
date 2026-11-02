namespace Ileri_Programlama.Models;

public class Question
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid QuizId { get; set; }
    public bool IsAi { get; set; }
    public string Desc { get; set; } = string.Empty;
    public string SoruTuru { get; set; } = string.Empty;
    public int QuestionTime { get; set; }
    public int QuestionPoint { get; set; }
    public string? KlasikCevap { get; set; }

    // İlişkiler
    public Quiz Quiz { get; set; } = null!;
    public ICollection<Secenek> Secenekler { get; set; } = new List<Secenek>();
    public ICollection<FileRecord> Files { get; set; } = new List<FileRecord>();
}