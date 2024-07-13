using System.ComponentModel.DataAnnotations;

namespace model.api;

public class Role
{
    public Role() { }

    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
    public ICollection<Account> Accounts { get; set; }
}