

namespace Ileri_Programlama.Models;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? EmailVerifiedAt { get; set; }
    public string Password { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }

    // İlişkiler
    public UserInfo? UserInfo { get; set; }
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<FileRecord> Files { get; set; } = new List<FileRecord>();
    public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
    public ICollection<Session> HostedSessions { get; set; } = new List<Session>();
    public ICollection<SessionUser> SessionParticipations { get; set; } = new List<SessionUser>();
}