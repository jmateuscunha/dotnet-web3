using System.Numerics;

namespace model.api;

public class Transaction
{
    public Transaction() { }

    public Transaction(string hash, string fromAddress, string toAddress, BigInteger amountRequested,BigInteger feeApplied, BigInteger nonce, Guid assetId)
    {
        Id=Guid.NewGuid();
        Hash=hash;
        FromAddress=fromAddress;
        ToAddress=toAddress;
        AmountRequested=amountRequested;
        AmountDeducted=amountRequested+feeApplied;
        FeeApplied=feeApplied;
        Nonce=nonce;
        AssetId=assetId;
        CreatedAt=DateTime.UtcNow;
        UpdatedAt=DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public string Hash { get; set; }
    public string FromAddress { get; set; }
    public string ToAddress { get; set; }
    public BigInteger AmountDeducted { get; set; }
    public BigInteger AmountRequested { get; set; }
    public BigInteger FeeApplied { get; set; }
    public BigInteger Nonce { get; set; }
    public string Status { get; set; }   
    public BigInteger? BlockNumber { get; set; }
    public Guid AssetId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Asset Asset { get; set; }
}

