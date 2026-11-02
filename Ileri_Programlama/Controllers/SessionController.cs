using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ileri_Programlama.Data;
using Ileri_Programlama.Models;
using Ileri_Programlama.DTOs;

namespace Ileri_Programlama.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly AppDbContext _context;

    public SessionController(AppDbContext context)
    {
        _context = context;
    }

    // 1. Canlı Oturum Başlatma
    [HttpPost("start")]
    public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
    {
        var quiz = await _context.Quizzes.FindAsync(dto.QuizId);
        if (quiz == null)
        {
            return NotFound(new { message = "Quiz bulunamadı." });
        }

        var session = new Session
        {
            QuizId = dto.QuizId,
            QuizerId = dto.QuizerId,
            SessionType = dto.SessionType,
            Status = SessionStatus.Pending
        };

        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Sınav oturumu başlatıldı.",
            sessionId = session.Id,
            status = session.Status.ToString()
        });
    }

    // 2. Oturuma Katılma
    [HttpPost("join")]
    public async Task<IActionResult> JoinSession([FromBody] JoinSessionDto dto)
    {
        var session = await _context.Sessions.FindAsync(dto.SessionId);
        if (session == null)
        {
            return NotFound(new { message = "Oturum bulunamadı." });
        }

        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null)
        {
            return NotFound(new { message = "Kullanıcı bulunamadı." });
        }

        var sessionUser = new SessionUser
        {
            SessionId = dto.SessionId,
            UserId = dto.UserId
        };

        await _context.SessionUsers.AddAsync(sessionUser);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Oturuma başarıyla katıldınız.",
            sessionUserId = sessionUser.Id,
            sessionId = session.Id
        });
    }

    // 3. Soruya Cevap Gönderme
    [HttpPost("submit-answer")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto dto)
    {
        var sessionUser = await _context.SessionUsers.FindAsync(dto.SessionUserId);
        if (sessionUser == null)
        {
            return NotFound(new { message = "Katılımcı oturum kaydı bulunamadı." });
        }

        var secenek = await _context.Secenekler.FindAsync(dto.AnswerId);
        if (secenek == null)
        {
            return NotFound(new { message = "Seçenek bulunamadı." });
        }

        var answer = new SessionAnswer
        {
            SessionUserId = dto.SessionUserId,
            QuestionId = dto.QuestionId,
            AnswerId = dto.AnswerId
        };

        await _context.SessionAnswers.AddAsync(answer);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Cevap başarıyla kaydedildi.",
            isTrue = secenek.IsTrue
        });
    }

    // 4. Oturum Sonuçları ve Skor Tablosu
    [HttpGet("{sessionId}/results")]
    public async Task<IActionResult> GetSessionResults(Guid sessionId)
    {
        var session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);

        if (session == null)
        {
            return NotFound(new { message = "Oturum bulunamadı." });
        }

        var sessionUsers = await _context.SessionUsers
            .Where(su => su.SessionId == sessionId)
            .Include(su => su.User)
            .Include(su => su.SessionAnswers)
            .ToListAsync();

        var allAnswerIds = sessionUsers
            .SelectMany(su => su.SessionAnswers)
            .Where(sa => sa.AnswerId.HasValue)
            .Select(sa => sa.AnswerId!.Value)
            .Distinct()
            .ToList();

        var secenekDict = await _context.Secenekler
            .Where(s => allAnswerIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.IsTrue);

        var allQuestionIds = sessionUsers
            .SelectMany(su => su.SessionAnswers)
            .Select(sa => sa.QuestionId)
            .Distinct()
            .ToList();

        var questionDict = await _context.Questions
            .Where(q => allQuestionIds.Contains(q.Id))
            .ToDictionaryAsync(q => q.Id, q => q.QuestionPoint);

        var leaderboard = sessionUsers.Select(su =>
        {
            int correctCount = 0;
            int totalScore = 0;

            foreach (var ans in su.SessionAnswers)
            {
                if (ans.AnswerId.HasValue &&
                    secenekDict.TryGetValue(ans.AnswerId.Value, out bool isTrue) &&
                    isTrue)
                {
                    correctCount++;
                    if (questionDict.TryGetValue(ans.QuestionId, out int point))
                    {
                        totalScore += point;
                    }
                }
            }

            return new
            {
                SessionUserId = su.Id,
                Username = su.User != null ? su.User.Username : "Bilinmeyen Kullanıcı",
                TotalAnswers = su.SessionAnswers.Count,
                CorrectAnswers = correctCount,
                Score = totalScore
            };
        })
        .OrderByDescending(r => r.Score)
        .ToList();

        return Ok(new
        {
            sessionId = session.Id,
            quizId = session.QuizId,
            leaderboard = leaderboard
        });
    }
}