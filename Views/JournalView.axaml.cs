using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReceptionBot.Views
{
    public partial class JournalView : UserControl
    {
        public JournalView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}