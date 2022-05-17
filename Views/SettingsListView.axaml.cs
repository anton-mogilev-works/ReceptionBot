using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReceptionBot.Views
{
    public partial class SettingsListView : UserControl
    {
        public SettingsListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}