using System.Collections.ObjectModel;
using ReceptionBot.Models;
using ReactiveUI;

namespace ReceptionBot.ViewModels
{
    public class ButtonsListViewModel : ViewModelBase
    {
        private ObservableCollection<Button> buttonsLits;

        public string Header {set; get;} = "Кнопки";

        public ObservableCollection<Button> ButtonsList
        {
            get => buttonsLits;
            private set => this.RaiseAndSetIfChanged(ref buttonsLits, value);
        }

        public ButtonsListViewModel(ObservableCollection<Button> buttonsLits)
        {
            ButtonsList = buttonsLits;
            
        }  
        
    }
}