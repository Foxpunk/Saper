using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Saper
{
    public partial class TableWindow : Window
    {
        // ObservableCollection для динамического отображения данных
        public ObservableCollection<PlayerRecord> PlayerRecords { get; set; }

        public TableWindow(List<PlayerRecord> records)
        {
            InitializeComponent();
            PlayerRecords = new ObservableCollection<PlayerRecord>(records);
            RecordTable.ItemsSource = PlayerRecords;
        }
    }
}
