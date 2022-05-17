using System.Collections.Generic;
using System;
using ReceptionBot.Models;
using ReceptionBot.Services;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;

namespace ReceptionBot.ViewModels
{
    public class JournalViewModel : ViewModelBase
    {
        private ObservableCollection<JournalRecord> journalRecords = new ObservableCollection<JournalRecord>(DbHelper.GetJournalRecords()); 
        public ObservableCollection<JournalRecord> JournalRecords
        {
            get => journalRecords; 
            private set => this.RaiseAndSetIfChanged(ref journalRecords, value);
        }

        public JournalViewModel()
        {
            ClearJournal = ReactiveCommand.Create(
                () => 
                {
                    DbHelper.ClearJournal();
                    JournalRecords = new ObservableCollection<JournalRecord>(DbHelper.GetJournalRecords()); 
                }
            );
        }

        public void UpdateJournal()
        {
            JournalRecords = new ObservableCollection<JournalRecord>(DbHelper.GetJournalRecords()); 
        }

        public ReactiveCommand<Unit, Unit> ClearJournal { get; }
    }
}