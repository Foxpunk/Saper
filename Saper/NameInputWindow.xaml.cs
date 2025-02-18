using System.Windows;

namespace Saper
{
    public partial class NameInputWindow : Window
    {
        public string PlayerName { get; private set; }

        public NameInputWindow()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            PlayerName = NameInput.Text.Trim();
            DialogResult = true;
            Close();
        }
    }
}
