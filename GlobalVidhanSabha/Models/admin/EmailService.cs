using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public static class EmailService
{
    private static string fromEmail = "macreelinfosoft@gmail.com";
    private static string appPassword = "wglg owzm muts ejli";

    public static async Task SendLoginEmailAsync(string toEmail, string username, string password)
    {
        MailMessage message = new MailMessage();
        message.From = new MailAddress(fromEmail);
        message.To.Add(toEmail);
        message.Subject = "Vidhan Sabha Login Details";

        message.Body = $@"
Dear User,

Your account has been created successfully.

Username: {username}
Password: {password}

Please login and change your password.

Regards,
Team Global Vidhan Sabha
";

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
        smtp.EnableSsl = true;
        smtp.Credentials = new NetworkCredential(fromEmail, appPassword);

        await smtp.SendMailAsync(message);
    }
}

public static class PasswordGenerator
{
    public static string GeneratePassword(string name, string phoneNo)
    {
        string cleanName = name?.Replace(" ", "") ?? "User";

        string last4Digits = "0000";
        if (!string.IsNullOrEmpty(phoneNo) && phoneNo.Length >= 4)
        {
            last4Digits = phoneNo.Substring(phoneNo.Length - 4);
        }

        return $"{cleanName}{last4Digits}";
    }
}