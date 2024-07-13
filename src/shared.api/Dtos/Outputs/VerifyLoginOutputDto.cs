namespace shared.api.Dtos.Ouputs;

public class VerifyLoginOutputDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public int Expires { get; set; }
}
