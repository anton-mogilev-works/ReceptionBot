using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReceptionBot.Views
{
    public partial class SettingPropertyView : UserControl
    {
        public SettingPropertyView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}