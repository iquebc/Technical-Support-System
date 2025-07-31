using System.Text.RegularExpressions;

namespace UserService.Web.API.Domain.Validation
{
    public class EmailValidation
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex padrão para validação de e-mails comuns
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
    }
}
