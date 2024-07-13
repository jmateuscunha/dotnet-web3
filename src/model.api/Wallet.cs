using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace model.api;

public class Wallet
{
    public Wallet() { }
    public Wallet(string name, Guid blockchainId, int accountId)
    {
        Id = Guid.NewGuid();
        Name = name;
        BlockchainId = blockchainId;
        AccountId = accountId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public int AccountId { get; set; }
    public Guid BlockchainId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
    public Account Account { get; set; }
    public Blockchain Blockchain { get; set; }
    public ICollection<Asset> Assets { get; set; }

    public void AddAsset(Asset asset)
    {
        Assets ??= new List<Asset>();
        Assets.Add(asset);
        UpdatedAt = DateTime.UtcNow;
    }
}