using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ileri_Programlama.Data;
using Ileri_Programlama.Models;
using Ileri_Programlama.DTOs;

namespace Ileri_Programlama.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase
{
    private readonly AppDbContext _context;

    public QuizController(AppDbContext context)
    {
        _context = context;
    }

    // 1. Yeni Quiz Oluşturma
    [HttpPost]
    public async Task<IActionResult> CreateQuiz([FromBody] QuizCreateDto dto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Id == dto.CreatorUserId);
        if (!userExists)
        {
            return BadRequest(new { message = "Belirtilen CreatorUserId sistemde bulunamadı." });
        }

        var quiz = new Quiz
        {
            UserId = dto.CreatorUserId,
            Puanlama = dto.Puanlama
        };

        await _context.Quizzes.AddAsync(quiz);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Quiz başarıyla oluşturuldu.",
            quizId = quiz.Id
        });
    }

    // 2. Quize Soru ve Seçenekleri Ekleme
    [HttpPost("add-question")]
    public async Task<IActionResult> AddQuestion([FromBody] QuestionCreateDto dto)
    {
        var quiz = await _context.Quizzes.FindAsync(dto.QuizId);
        if (quiz == null)
        {
            return NotFound(new { message = "Soru eklenecek quiz bulunamadı." });
        }

        if (dto.Secenekler == null || dto.Secenekler.Count < 2)
        {
            return BadRequest(new { message = "Bir soru için en az 2 seçenek girilmelidir." });
        }

        var question = new Question
        {
            QuizId = dto.QuizId,
            Desc = dto.Desc,
            QuestionPoint = dto.QuestionPoint,
            QuestionTime = dto.QuestionTime,
            SoruTuru = dto.SoruTuru,
            IsAi = dto.IsAktif,
            Secenekler = dto.Secenekler.Select(s => new Secenek
            {
                Desc = s.Desc,
                Queue = s.Queue,
                IsTrue = s.IsTrue
            }).ToList()
        };

        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Soru ve seçenekleri başarıyla eklendi.",
            questionId = question.Id
        });
    }

    // 3. Quiz ve Soruları Getirme
    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizById(Guid id)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions)
                .ThenInclude(qs => qs.Secenekler)
            .Include(q => q.User)
            .Where(q => q.Id == id && q.DeletedAt == null)
            .Select(q => new
            {
                q.Id,
                q.Puanlama,
                CreatedBy = q.User.Username,
                q.CreatedAt,
                QuestionCount = q.Questions.Count,
                Questions = q.Questions.Select(qs => new
                {
                    qs.Id,
                    qs.Desc,
                    qs.QuestionPoint,
                    qs.QuestionTime,
                    qs.SoruTuru,
                    Secenekler = qs.Secenekler.OrderBy(s => s.Queue).Select(s => new
                    {
                        s.Id,
                        s.Desc,
                        s.Queue,
                        s.IsTrue
                    })
                })
            })
            .FirstOrDefaultAsync();

        if (quiz == null)
        {
            return NotFound(new { message = "Quiz bulunamadı." });
        }

        return Ok(quiz);
    }

    // 4. Tüm Quizleri Listeleme
    [HttpGet]
    public async Task<IActionResult> GetAllQuizzes()
    {
        var quizzes = await _context.Quizzes
            .Where(q => q.DeletedAt == null)
            .Include(q => q.User)
            .Select(q => new
            {
                q.Id,
                q.Puanlama,
                CreatedBy = q.User.Username,
                q.CreatedAt
            })
            .ToListAsync();

        return Ok(quizzes);
    }
}