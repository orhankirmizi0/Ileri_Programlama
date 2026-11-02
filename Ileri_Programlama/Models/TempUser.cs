using Ileri_Programlama.Models;

namespace Ileri_Programlama.Models;

public class TempUser : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public DateTime? DeletedAt { get; set; }
}