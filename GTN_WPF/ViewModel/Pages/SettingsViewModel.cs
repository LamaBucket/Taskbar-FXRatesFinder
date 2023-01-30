using GTN_WPF.Model;
using GTN_WPF.Model.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace GTN_WPF.ViewModel.Pages
{
    internal class SettingsViewModel : ViewModelBaseConfig
    {
        private TimeSpan _ratesRefreshTime = TimeSpan.Zero;

        public TimeSpan RatesRefreshTime
        {
            get => _ratesRefreshTime;
            set
            {
                _ratesRefreshTime = value;
                OnPropertyChanged(nameof(RatesRefreshTime));
            }
        }

        private ObservableCollection<string> _popupRates = new ObservableCollection<string>();

        public ObservableCollection<string> PopupRates
        {
            get => _popupRates;
            set
            {
                _popupRates = value;
                OnPropertyChanged(nameof(PopupRates));
            }
        }

        private string _iconRateName = String.Empty;

        private string _localCurrencyName = String.Empty;

        public string LocalCurrencyName
        {
            get => _localCurrencyName;
            set
            {
                _localCurrencyName = value;
                OnPropertyChanged(nameof(LocalCurrencyName));
            }
        }

        public string IconRateName
        {
            get => _iconRateName;
            set
            {
                _iconRateName = value;
                OnPropertyChanged(nameof(IconRateName));
            }
        }

        public RelayCommand UnbindCommand => new RelayCommand((param) =>
        {
            string name = (param as string) ?? FXRate.Unknown.Name;
            _config.UnbindItem(name);
        });

        public RelayCommand SaveCommand => new RelayCommand((param) =>
        {
            _config = new AppConfig(_config.ApiKey, PopupRates.ToList(), IconRateName, RatesRefreshTime, LocalCurrencyName);
            RefreshConfig();
        });

        public RelayCommand DiscardCommand => new RelayCommand((param) =>
        {
            RefreshConfig();
        });


        public RelayCommand MoveUpCommand => new RelayCommand((param) =>
        {
            if (PopupRates.Count > 1 && param is string)
            {
                string name = param as string ?? String.Empty; // it will never be string empty.

                int index = PopupRates.IndexOf(PopupRates.Where(x => x == name).First());

                if (index > 0)
                {
                    PopupRates.Move(index, index - 1);
                }
            }
        });

        public RelayCommand MoveDownCommand => new RelayCommand((param) =>
        {
            if (PopupRates.Count > 1 && param is string)
            {
                string name = param as string ?? String.Empty; // it will never be string empty.

                int index = PopupRates.IndexOf(PopupRates.Where(x => x == name).First());

                if (index < PopupRates.Count - 1)
                {
                    PopupRates.Move(index, index + 1);
                }
            }
        });

        public RelayCommand SetIconItemCommand => new RelayCommand((param) =>
        {
            IconRateName = param as string ?? FXRate.Default.Name;
        });

        public RelayCommand SetLocalCurrencyNameCommand => new RelayCommand((param) =>
        {
            LocalCurrencyName = param as string ?? FXRate.Default.Name;
        });

        internal protected override void RefreshConfig()
        {
            base.RefreshConfig();

            RatesRefreshTime = _config.RatesRefreshTime;
            PopupRates = new ObservableCollection<string>(_config.PopupRatesNames);
            IconRateName = _config.IconRateName;
            LocalCurrencyName = _config.LocalCurrencyName;
        }

        public SettingsViewModel(IConfigManager configManager) : base(configManager)
        {
            RatesRefreshTime = _config.RatesRefreshTime;
            PopupRates = new ObservableCollection<string>(_config.PopupRatesNames);
            IconRateName = _config.IconRateName;
            LocalCurrencyName = _config.LocalCurrencyName;
        }


    }
}
