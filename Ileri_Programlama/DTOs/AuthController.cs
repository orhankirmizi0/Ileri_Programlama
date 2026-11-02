using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ileri_Programlama.Data;
using Ileri_Programlama.Models;
using Ileri_Programlama.DTOs;

namespace Ileri_Programlama.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // 1. Yeni Kullanıcı Kaydı (Register)
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        // E-posta veya Kullanıcı adı benzersizlik kontrolü
        var exists = await _context.Users.AnyAsync(u => u.Email == dto.Email || u.Username == dto.Username);
        if (exists)
        {
            return BadRequest(new { message = "Bu e-posta adresi veya kullanıcı adı zaten kullanımda." });
        }

        // 1-1 ilişkiyi kurarak User ve UserInfo nesnesini aynı anda oluşturuyoruz
        var newUser = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            // NOT: Gerçek projelerde BCrypt ile hash'lenir, temel akış için düz atama
            Password = dto.Password,
            UserInfo = new UserInfo
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone
            }
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Kullanıcı başarıyla kaydedildi.",
            userId = newUser.Id
        });
    }

    // 2. Kullanıcıları Listeleme (Test amaçlı)
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(u => u.UserInfo)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.CreatedAt,
                FullName = u.UserInfo != null ? $"{u.UserInfo.FirstName} {u.UserInfo.LastName}" : ""
            })
            .ToListAsync();

        return Ok(users);
    }
}