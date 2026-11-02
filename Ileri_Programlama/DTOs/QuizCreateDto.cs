using Ileri_Programlama.Models;

namespace Ileri_Programlama.DTOs;

public class QuizCreateDto
{
    public Guid CreatorUserId { get; set; }
    public PuanlamaTuru Puanlama { get; set; }
}