using System;
using System.ComponentModel;
using System.Windows;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        public DaneOsobowe Dane { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Dane = new DaneOsobowe();
            DataContext = Dane; 
        }

        private void SprawdzDane_Click(object sender, RoutedEventArgs e)
        {
            if (Dane.SprawdzPesel())
            {
                MessageBox.Show("PESEL jest poprawny.", "Weryfikacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(Dane.ErrorMessage, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }


}
