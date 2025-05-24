using DiscService.Messaging.Models;

namespace DiscService.Messaging.Kafka;

public class BotMessageHandler : IMessageHandler
{
    public Task<BotMessage?> HandleAsync(BotMessage incoming)
    {
        string? responseText;
        object? replyMarkup = null;

        switch (incoming.Data.Text)
        {
            case "/hello":
                responseText = "Привет! Это *DiscService* 👋";
                break;
            case "/world":
                responseText = "Команда /world _принята_ 🌍";
                break;
            case "callback_test":
                responseText = $"Вы нажали: *{incoming.Data.Text}*";
                break;
            case "/buttons":
                responseText = "Нажмите кнопку:";
                replyMarkup = new
                {
                    inline_keyboard = new[]
                    {
                        new[]
                        {
                            new { text = "👉 Нажми меня", callback_data = "callback_test" }
                        }
                    }
                };
                break;
            default:
                responseText = "Неизвестная команда.";
                break;
        }

        return Task.FromResult<BotMessage?>(new BotMessage
        {
            Method = "sendmessage",
            Filename = null,
            KafkaMessageId = incoming.KafkaMessageId,
            Status = "COMPLETED",
            Data = new SendMessageData
            {
                ChatId = incoming.Data.ChatId,
                Text = responseText,
                ParseMode = "Markdown",
                ReplyMarkup = replyMarkup
            }
        });
    }
}