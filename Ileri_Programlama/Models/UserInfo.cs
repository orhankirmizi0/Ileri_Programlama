
namespace Ileri_Programlama.Models;

public class UserInfo : BaseEntity
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? SchoolId { get; set; }
    public string Phone { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}