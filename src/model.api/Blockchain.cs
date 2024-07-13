namespace model.api;

public class Blockchain
{
    public Blockchain() { }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Network { get; set; }
    public int ChainId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set;}

    public ICollection<Wallet> Wallets { get; set; }
}

