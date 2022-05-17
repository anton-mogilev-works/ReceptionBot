using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReceptionBot.Views
{
    public partial class RightPanelView : UserControl
    {
        private Button startButton;
        public RightPanelView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            startButton = this.FindControl<Button>("Start");
        }
    }
}