using ReactiveUI;
using ReceptionBot.Models;
using System.Reactive;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using ReceptionBot.Services;

namespace ReceptionBot.ViewModels
{
    public class ButtonPropertyViewModel : ViewModelBase
    {
        private string _data;
        public string Data
        {
            get => _data;
            set => this.RaiseAndSetIfChanged(ref _data, value);
        }

        public Button button = new Button();

        public Button Button
        {
            get => button;
            set => this.RaiseAndSetIfChanged(ref button, value);
        }

        public ButtonPropertyViewModel(Button button)
        {
            if (button is not null)
            {
                Button = button;
            }

            SaveButton = ReactiveCommand.Create(() => Button);
            CancelButton = ReactiveCommand.Create(() => Button);
            DeleteButton = ReactiveCommand.Create(() => Button.Id);            
        }

        public ReactiveCommand<Unit, Button> SaveButton { get; }
        public ReactiveCommand<Unit, Button> CancelButton { get; }
        public ReactiveCommand<Unit, int> DeleteButton { get; }

        // public ReactiveCommand<Unit, Unit> AddImageButton { get; }

        public async Task AddImageButton()
        {
            try
            {
                var dialog = new Avalonia.Controls.OpenFileDialog();
                string[] result = new string[]{};
                dialog.Filters.Add(
                    new Avalonia.Controls.FileDialogFilter()
                    {
                        // Name = "Text",
                        Extensions = { "*" }
                    }
                );
                result = await dialog.ShowAsync(new Avalonia.Controls.Window());
                if (result != null)
                {                    
                    // Data = File.ReadAllText(result.FirstOrDefault());
                    foreach(string filename in result)
                    {
                        Console.WriteLine(filename);    
                    }
                }
            }
            catch (Exception ex)
            {
                // _serviceProvider.GetService<ILog>().LogError($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
