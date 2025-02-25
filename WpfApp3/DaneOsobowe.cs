using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp3
{
    public class DaneOsobowe : INotifyPropertyChanged
    {
        private string _imie;
        private string _nazwisko;
        private DateTime? _dataUrodzenia;
        private string _plec;
        private string _pesel;

        public string Imie
        {
            get => _imie;
            set { _imie = value; OnPropertyChanged(nameof(Imie)); }
        }

        public string Nazwisko
        {
            get => _nazwisko;
            set { _nazwisko = value; OnPropertyChanged(nameof(Nazwisko)); }
        }

        public DateTime? DataUrodzenia
        {
            get => _dataUrodzenia;
            set { _dataUrodzenia = value; OnPropertyChanged(nameof(DataUrodzenia)); }
        }

        public string Plec
        {
            get => _plec;
            set { _plec = value; OnPropertyChanged(nameof(Plec)); }
        }

        public string Pesel
        {
            get => _pesel;
            set { _pesel = value; OnPropertyChanged(nameof(Pesel)); }
        }

        public DaneOsobowe() { }

        public string ErrorMessage { get; private set; }

        public bool SprawdzPesel()
        {
            ErrorMessage = string.Empty;

            if (Pesel.Length != 11 || !long.TryParse(Pesel, out _))
            {
                ErrorMessage = "PESEL musi mieć 11 cyfr.";
                return false;
            }

            int rok = int.Parse(Pesel.Substring(0, 2));
            int miesiac = int.Parse(Pesel.Substring(2, 2));
            int dzien = int.Parse(Pesel.Substring(4, 2));
            int plecCyfra = int.Parse(Pesel.Substring(9, 1));

            // Obliczanie roku i miesiąca
            if (miesiac > 80)
            {
                rok += 1800;
                miesiac -= 80;
            }
            else if (miesiac > 60)
            {
                rok += 2200;
                miesiac -= 60;
            }
            else if (miesiac > 40)
            {
                rok += 2100;
                miesiac -= 40;
            }
            else if (miesiac > 20)
            {
                rok += 2000;
                miesiac -= 20;
            }
            else
            {
                rok += 1900;
            }

            // Walidacja daty
            if (!DataUrodzenia.HasValue ||
                DataUrodzenia.Value.Year != rok ||
                DataUrodzenia.Value.Month != miesiac ||
                DataUrodzenia.Value.Day != dzien)
            {
                ErrorMessage = "Nieprawidłowa data urodzenia.";
                return false;
            }

            // Walidacja płci
            if ((plecCyfra % 2 == 0 && Plec.ToLower() != "kobieta") ||
                (plecCyfra % 2 == 1 && Plec.ToLower() != "mężczyzna"))
            {
                ErrorMessage = "Nieprawidłowa płeć.";
                return false;
            }

            // Sprawdzenie cyfry kontrolnej
            if (!SprawdzCyfreKontrolna(Pesel))
            {
                ErrorMessage = "Błędna cyfra kontrolna.";
                return false;
            }

            return true; // PESEL jest poprawny
        }


        private bool SprawdzCyfreKontrolna(string pesel)
        {
            int[] wagi = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
            int suma = 0;

            for (int i = 0; i < 10; i++)
                suma += wagi[i] * (pesel[i] - '0');

            int cyfraKontrolna = (10 - (suma % 10)) % 10;
            return cyfraKontrolna == (pesel[10] - '0');
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
