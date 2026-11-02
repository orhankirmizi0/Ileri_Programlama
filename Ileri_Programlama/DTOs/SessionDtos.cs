using Ileri_Programlama.Models;

namespace Ileri_Programlama.DTOs;

// 1. Canlı Oturum Başlatma DTO'su
public class StartSessionDto
{
    public Guid QuizId { get; set; }
    public Guid QuizerId { get; set; } // Oturumu başlatan hoca / kullanıcı
    public SessionType SessionType { get; set; } = SessionType.Puanli;
}

// 2. Öğrencinin Oturuma Katılma DTO'su
public class JoinSessionDto
{
    public Guid SessionId { get; set; } // Katılınacak oturumun ID'si
    public Guid UserId { get; set; }    // Katılan öğrenci
}

// 3. Öğrencinin Soruya Cevap Verme DTO'su
public class SubmitAnswerDto
{
    public Guid SessionUserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; } // Seçilen seçeneğin GUID'si
}