using System.Reactive;
using ReactiveUI;
using ReceptionBot.Models;
using System.Collections.Generic;

namespace ReceptionBot.ViewModels
{
    public class SettingPropertyViewModel : ViewModelBase
    {
        private Setting botToken;

        public Setting BotToken
        {
            get => botToken;
            set => this.RaiseAndSetIfChanged(ref botToken, value);
        }

        private Setting buttonsInRow;

        public Setting ButtonsInRow
        {
            get => buttonsInRow;
            set => this.RaiseAndSetIfChanged(ref buttonsInRow, value);
        }

        private Setting defaultAnswer;

        public Setting DefaultAnswer
        {
            get => defaultAnswer;
            set => this.RaiseAndSetIfChanged(ref defaultAnswer, value);
        }

        public SettingPropertyViewModel()
        {
            SaveSettingsButton = ReactiveCommand.Create(
                () => new List<Setting>() { BotToken, ButtonsInRow, DefaultAnswer }
            );
        }

        public ReactiveCommand<Unit, List<Setting>> SaveSettingsButton { get; }
    }
}
