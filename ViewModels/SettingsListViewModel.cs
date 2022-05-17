using System;
using System.Reactive;
using ReactiveUI;

namespace ReceptionBot.ViewModels
{
    public class SettingsListViewModel : ViewModelBase
    {
        public SettingsListViewModel()
        {
            ShowBotSettings = ReactiveCommand.Create(
                () => { }
            );

        }
        public ReactiveCommand<Unit, Unit> ShowBotSettings { get; }
    }
}