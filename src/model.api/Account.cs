using System.ComponentModel.DataAnnotations;

namespace model.api;

public class Account
{
    public Account() { }
    public Account(string email, string password)
    {
        Email = email;
        Password = password;
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    [Key]
    public int Id { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    public ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<Wallet> Wallets { get; set; }
}
