using Identity.Dtos;

namespace Application.Dtos;

public class UserResponceDto : ResponceTokenDto
{
    public string Id { get; set; } = "";
    public string Username { get; set; } = "";
    public IList<string> Role { get; set; } = new List<string>{ };
    public bool LoginStatus { get; set; } = false;
    
}