using System;
using ReactiveUI;
using ReceptionBot.Services;
using ReceptionBot.Models;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace ReceptionBot.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string messageText = string.Empty;
        public string MessageText
        {
            get => messageText;
            set => this.RaiseAndSetIfChanged(ref messageText, value);
        }

        public TelegramBot telegramBot;
        private ObservableCollection<Button> buttonsLits = new ObservableCollection<Button>(
            DbHelper.getButtons()
        );

        public ObservableCollection<Button> ButtonsList
        {
            get => new ObservableCollection<Button>(DbHelper.getButtons());
            set { this.RaiseAndSetIfChanged(ref buttonsLits, value); }
        }

        public ViewModelBase leftPanelViewModel;
        public ViewModelBase centralPanelViewModel;

        public ViewModelBase rightPanelViewModel;

        public ViewModelBase LeftPanelViewModel
        {
            get => leftPanelViewModel;
            private set => this.RaiseAndSetIfChanged(ref leftPanelViewModel, value);
        }

        public ViewModelBase CentralPanelViewModel
        {
            get => centralPanelViewModel;
            private set => this.RaiseAndSetIfChanged(ref centralPanelViewModel, value);
        }

        public ViewModelBase RightPanelViewModel
        {
            get => rightPanelViewModel;
            private set => this.RaiseAndSetIfChanged(ref rightPanelViewModel, value);
        }

        public void ShowButtons()
        {
            LeftPanelViewModel = new ButtonsListViewModel(ButtonsList);
            CentralPanelViewModel = new MessageViewModel(
                "Выберите или создайте в левой панели кнопку для редактирования"
            );
        }

        public void ShowSettings()
        {
            LeftPanelViewModel = new SettingsListViewModel();
            var cpvm = new SettingPropertyViewModel();

            Setting botToken = DbHelper.GetSettingByName("BotToken");
            Setting buttonsInRow = DbHelper.GetSettingByName("ButtonsInRow");
            Setting defaultAnswer = DbHelper.GetSettingByName("DefaultAnswer");

            cpvm.BotToken = botToken;
            cpvm.ButtonsInRow = buttonsInRow;
            cpvm.DefaultAnswer = defaultAnswer;

            cpvm.SaveSettingsButton
                .AsObservable<List<Setting>>()
                .Subscribe(
                    model =>
                    {
                        if (model is not null && model.GetType() == typeof(List<Setting>))
                        {
                            DbHelper.SaveSettings(model.ToArray());
                            telegramBot.Init();
                        }
                    }
                );

            CentralPanelViewModel = cpvm;
        }

        public void EditButton(Button button)
        {
            var vm = new ButtonPropertyViewModel(button);

            vm.SaveButton
                .AsObservable<Button>()
                .Subscribe(
                    model =>
                    {
                        if (model != null && model.Id == 0)
                        {
                            DbHelper.saveButton(model);
                        }
                        else
                        {
                            var button = ButtonsList.FirstOrDefault(i => i.Id == model.Id);
                            if (button is not null)
                            {
                                DbHelper.saveButton(model);
                            }
                        }

                        LeftPanelViewModel = new ButtonsListViewModel(ButtonsList);
                        telegramBot.Init();
                    }
                );

            vm.CancelButton
                .AsObservable()
                .Subscribe(
                    model =>
                    {
                        CentralPanelViewModel = new ButtonPropertyViewModel(
                            DbHelper.GetButtonById(model.Id)
                        );
                    }
                );

            vm.DeleteButton
                .AsObservable<int>()
                .Subscribe(
                    id =>
                    {
                        if (id.GetType() == typeof(int) && id > 0)
                        {
                            DbHelper.DeleteButtonById(id);
                            LeftPanelViewModel = new ButtonsListViewModel(ButtonsList);
                            CentralPanelViewModel = new MessageViewModel(
                                "Выберите или создайте в левой панели кнопку для редактирования"
                            );
                            telegramBot.Init();
                        }
                    }
                );

            CentralPanelViewModel = vm;
        }

        public void ShowAbout()
        {
            CentralPanelViewModel = new AboutViewModel();
        }

        public void HelpGuide()
        {
            CentralPanelViewModel = new HelpGuideViewModel();
        }

        public void ShowCommands() { }

        public void ShowJournal()
        {
            CentralPanelViewModel = new JournalViewModel();
        }

        public MainWindowViewModel()
        {
            DbHelper.CheckAndRepairSettingsList();
            telegramBot = new TelegramBot();

            telegramBot
                .WhenAnyValue(x => x.Message)
                .Subscribe(
                    x =>
                    {
                        DbHelper.RecordToJournal(telegramBot.Message);
                        MessageText = x.FormatedDateTime + "   " + x.Text + "\r\n" + MessageText;
                    }
                );

            var rp = new RightPanelViewModel();
            rp.SwitchBot
                .AsObservable()
                .Subscribe(
                    status =>
                    {
                        if (status)
                        {
                            telegramBot.Start.Execute().Subscribe();
                        }
                        else
                        {
                            telegramBot.Stop.Execute().Subscribe();
                        }
                    }
                );

            telegramBot.Stop
                .AsObservable()
                .Subscribe(
                    _ =>
                    {
                        rp.BotStatus = false;
                        rp.SyncButtonWithStatus();
                    }
                );

            RightPanelViewModel = rp;
        }
    }
}
