namespace shared.api.Dtos.Ouputs;

public class GasPriceSuggestionOutputDto
{
    public string Status { get; set; }
    public string Message { get; set; }
    public GasPriceSuggestionResultOutputDto Result { get; set; }
}

public class GasPriceSuggestionResultOutputDto
{
    public string LastBlock { get; set; }
    public string SafeGasPrice { get; set; }
    public string ProposeGasPrice { get; set; }
    public string FastGasPrice { get; set; }
    public string SuggestBaseFee { get; set; }
    public string GasUsedRatio { get; set; }
}