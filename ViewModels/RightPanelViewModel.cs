using System;
using ReactiveUI;
using System.Reactive;

namespace ReceptionBot.ViewModels
{
    public class RightPanelViewModel : ViewModelBase
    {
        private bool botStatus = false;

        public bool BotStatus
        {
            get => botStatus;
            set => this.RaiseAndSetIfChanged(ref botStatus, value);
        }

        private string buttonCaption;

        public string ButtonCaption
        {
            get => buttonCaption;
            private set => this.RaiseAndSetIfChanged(ref buttonCaption, value);
        }

        public string CaptionFromStatus(bool status)
        {
            return status ? "Бот включён" : "Бот выключен";
        }

        public void SyncButtonWithStatus() 
        {
            ButtonCaption = CaptionFromStatus(BotStatus);
        }

        public RightPanelViewModel()
        {
            SwitchBot = ReactiveCommand.Create(
                () =>
                {
                    SyncButtonWithStatus();
                    return BotStatus;
                }
            );

            SyncButtonWithStatus();
        }

        public ReactiveCommand<Unit, bool> SwitchBot { get; }
    }
}
