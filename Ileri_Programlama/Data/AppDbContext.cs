using Microsoft.EntityFrameworkCore;
using Ileri_Programlama.Models;

namespace Ileri_Programlama.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<UserInfo> UserInfos => Set<UserInfo>();
    public DbSet<TempUser> TempUsers => Set<TempUser>();
    public DbSet<EmailChallenge> EmailChallenges => Set<EmailChallenge>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<FileRecord> Files => Set<FileRecord>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Secenek> Secenekler => Set<Secenek>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionUser> SessionUsers => Set<SessionUser>();
    public DbSet<SessionAnswer> SessionAnswers => Set<SessionAnswer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User - UserInfo
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserInfo)
            .WithOne(ui => ui.User)
            .HasForeignKey<UserInfo>(ui => ui.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - RefreshTokens
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // User - Files
        modelBuilder.Entity<FileRecord>()
            .HasOne(f => f.User)
            .WithMany(u => u.Files)
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question - Files
        modelBuilder.Entity<FileRecord>()
            .HasOne(f => f.Question)
            .WithMany(q => q.Files)
            .HasForeignKey(f => f.QuestionId)
            .OnDelete(DeleteBehavior.SetNull);

        // User - Quiz
        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.User)
            .WithMany(u => u.Quizzes)
            .HasForeignKey(q => q.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Quiz - Question
        modelBuilder.Entity<Question>()
            .HasOne(q => q.Quiz)
            .WithMany(qz => qz.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // Question - Secenek
        modelBuilder.Entity<Secenek>()
            .HasOne(s => s.Question)
            .WithMany(q => q.Secenekler)
            .HasForeignKey(s => s.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Session - User (Quizer) & Quiz
        modelBuilder.Entity<Session>()
            .HasOne(s => s.Quizer)
            .WithMany(u => u.HostedSessions)
            .HasForeignKey(s => s.QuizerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.Quiz)
            .WithMany(q => q.Sessions)
            .HasForeignKey(s => s.QuizId)
            .OnDelete(DeleteBehavior.Cascade);

        // Session - SessionUser
        modelBuilder.Entity<SessionUser>()
            .HasOne(su => su.Session)
            .WithMany(s => s.SessionUsers)
            .HasForeignKey(su => su.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // SessionUser - User
        modelBuilder.Entity<SessionUser>()
            .HasOne(su => su.User)
            .WithMany(u => u.SessionParticipations)
            .HasForeignKey(su => su.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // SessionUser - SessionAnswer
        modelBuilder.Entity<SessionAnswer>()
            .HasOne(sa => sa.SessionUser)
            .WithMany(su => su.SessionAnswers)
            .HasForeignKey(sa => sa.SessionUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}