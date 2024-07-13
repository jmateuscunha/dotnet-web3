using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace model.api;

public class Asset
{
    public Asset() { }
    public Asset(Guid walletId, string address, byte[] keystore)
    {
        Id = Guid.NewGuid();
        WalletId = walletId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        Address = address;
        Balance = 0;
        Keystore = keystore;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; }
    public Guid WalletId { get; set; }
    public string Address { get; set; }
    public BigInteger Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}
    public byte[] Keystore { get; set; }
    public Wallet Wallet { get; set; }
    public ICollection<Transaction> Transactions { get; set;}
}

