using System.ComponentModel.DataAnnotations;

namespace shared.api.Dtos.Inputs;

public record AddWalletInputDto([Required] string Name, [Required] Guid? BlockchainId, [Required] int? AccountId);