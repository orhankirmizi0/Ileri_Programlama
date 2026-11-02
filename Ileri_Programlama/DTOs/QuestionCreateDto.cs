namespace Ileri_Programlama.DTOs;

public class SecenekDto
{
    public string Desc { get; set; } = string.Empty;
    public int Queue { get; set; }
    public bool IsTrue { get; set; }
}

public class QuestionCreateDto
{
    public Guid QuizId { get; set; }
    public string Desc { get; set; } = string.Empty;
    public int QuestionPoint { get; set; } = 10;
    public int QuestionTime { get; set; } = 30; // saniye cinsinden süre
    public string SoruTuru { get; set; } = "CoktanSecmeli";
    public bool IsAktif { get; set; } = true;
    public List<SecenekDto> Secenekler { get; set; } = new();
}