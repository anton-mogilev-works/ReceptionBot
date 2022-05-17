using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReceptionBot.Views
{
    public partial class ButtonsListView : UserControl
    {
        public ButtonsListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}