using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace postgresposhelnah.Service
{
    public class EmailConfirmService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Создание структуры передаваемого сообщения
            var emailMessage = new MimeMessage()
            {
                // Заголовок
                Subject = subject,
                // Содержимое письма
                Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                }
            };

            // От кого
            emailMessage.From.Add(new MailboxAddress("Администрация", "valera.skripa4@gmail.com"));
            // Для кого
            emailMessage.To.Add(new MailboxAddress("", email));           

            // Протокол для передачи сообщения
            using (var client = new SmtpClient())
            {
                // Подключение к поставщику email. У каждого поставщика свои сервера и порты
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                // Аутентификация отправителя. Здесь я подключил пароль приложения, через настройки Gmail аккаунта

                await client.AuthenticateAsync("valera.skripa4@gmail.com", "dmvtkgwsrcszubva");

                // Отправка ссылки 
                await client.SendAsync(emailMessage);
                // Выход отправителя
                await client.DisconnectAsync(true);
            }
        }
    }
}
