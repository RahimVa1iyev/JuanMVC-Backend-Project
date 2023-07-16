namespace JuanMVC.Email
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
