namespace UserService.Web.API.Application.DTOs;

public class UserUpdatePasswordDTO
{
    public int Id { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}