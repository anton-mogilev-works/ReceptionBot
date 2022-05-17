using ReactiveUI;

namespace ReceptionBot.ViewModels
{
    public class MessageViewModel : ViewModelBase
    {
        private string message = string.Empty;

        public string Message
        {
            get => message;
            set => this.RaiseAndSetIfChanged(ref message, value);
        }
        public MessageViewModel()
        {

        }

        public MessageViewModel(string message)
        {
            Message = message;
        }
    }

}