using ReactiveUI;
using ReceptionBot.Models;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reactive;
using System;

namespace ReceptionBot.Services
{
    public class TelegramBot : ReactiveObject
    {
        private static TelegramBotClient botClient;
        private CancellationTokenSource cancellationTokenSource;
        // private static bool status;

        private JournalRecord message = new JournalRecord() { Text = "Запуск программы"};

        public JournalRecord Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }

        public int buttonsInRow = 2;
        public string botToken;

        public string defaultAnswer;

        ReplyKeyboardMarkup replyKeyboardMarkup;
        Dictionary<string, string> buttonsAnswers;

        public bool Status { set; get; } = false;

        public TelegramBot()
        {
            Init();

            Stop = ReactiveCommand.Create(
                () =>
                {
                    cancellationTokenSource.Cancel();
                    JournalRecord message = new JournalRecord() { Text = "Бот остановлен"};                    
                    Message = message;                    
                }
            );

            Start = ReactiveCommand.Create(() => StartBot());
        }

        public void Init()
        {
            
            buttonsAnswers = DbHelper.getButtonsAnswers();
            defaultAnswer = DbHelper.GetSettingByName("DefaultAnswer").Value;
            botToken = DbHelper.GetSettingByName("BotToken").Value;
            try
            {
                buttonsInRow = Int32.Parse(DbHelper.GetSettingByName("ButtonsInRow").Value);
            }
            catch {}

            replyKeyboardMarkup = GetReplyKeyboardMarkup();
            botClient = new TelegramBotClient(botToken);
        }

        private async void StartBot()
        {
            cancellationTokenSource = new CancellationTokenSource();

            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions
                {
                    AllowedUpdates = { } // receive all update types
                },
                cancellationToken: cancellationTokenSource.Token
            );

            try
            {
                var me = await botClient.GetMeAsync();
                JournalRecord message = new JournalRecord() { Text = $"Бот @{me.Id} начал слушать сообщения"};
                Message = message;
            }
            catch
            {
                JournalRecord message = new JournalRecord() { Text = "Ошибка авторизации бота"};                
                Message = message;                
            }
        }

        // public void Stop()
        // {
        //     cancellationTokenSource.Cancel();
        // }

        public ReactiveCommand<Unit, Unit> Stop { get; set; }

        public ReactiveCommand<Unit, Unit> Start { get; set;}

        public async void Test()
        {
            var me = await botClient.GetMeAsync();
            Console.WriteLine(me.Id);
        }

        private ReplyKeyboardMarkup GetReplyKeyboardMarkup()
        {
            List<Button> buttons = DbHelper.getButtons();
            List<List<KeyboardButton>> keyboardButtons = new List<List<KeyboardButton>>();

            int i = 0;
            while(i < buttons.Count)
            {
                List<KeyboardButton> keyboardButtonsRow = new List<KeyboardButton>();
                int k = 0;
                while(k < buttonsInRow && i < buttons.Count)
                {
                    keyboardButtonsRow.Add(new KeyboardButton(buttons[i].Name));                    
                    k++;
                    i++;
                }
                keyboardButtons.Add(keyboardButtonsRow);
                
            }

            // KeyboardButton[] keyboardButtons = new KeyboardButton[buttons.Count];
            // for (int i = 0; i < buttons.Count; i++)
            // {
            //     keyboardButtons[i] = new KeyboardButton(buttons[i].Name);
            // }            
            return new ReplyKeyboardMarkup(keyboardButtons) { ResizeKeyboard = true };
        }

        public async Task HandleUpdateAsync(
            ITelegramBotClient botClient,
            Update update,
            CancellationToken cancellationToken
        )
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            string answer = string.Empty;

            try
            {
                answer = buttonsAnswers[messageText];
            }
            catch
            {
                answer = defaultAnswer;
            }

            Message secondMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: answer,
                replyMarkup: replyKeyboardMarkup,
                cancellationToken: cancellationToken
            );
        }

        public Task HandleErrorAsync(
            ITelegramBotClient botClient,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                  => $"Ошибка Telegram API: {apiRequestException.ErrorCode} {apiRequestException.Message}",
                _ => exception.ToString()
            };

            JournalRecord message = new JournalRecord() { Text = ErrorMessage};            
            Message = message;

            Stop.Execute().Subscribe();

            return Task.CompletedTask;
        }
    }
}
