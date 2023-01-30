using GTN_WPF.Model;
using GTN_WPF.Model.Interfaces;
using System;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GTN_WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IConfigManager configManager, IDataProvider dataProvider)
        {
            _configManager = configManager;
            _dataProvider = dataProvider;

            _taskbarIcon = Properties.Resources.TaskbarIcon;

            _popupData = new DataTable();

            _popupData.Columns.Add("Currency Name", typeof(string));
            _popupData.Columns.Add("Exchange Rate", typeof(decimal));


            Info = new FXRatesProviderThreadInfo(_configManager.GetConfig() ?? throw new InvalidExpressionException(), dataProvider);
            Info.ActualRatesChanged += OnActualRatesChanged;

            FXRatesProvider = new Thread(Info.RefreshActualRates);
            FXRatesProvider.IsBackground = true;

            FXRatesProvider.Start();
        }



        private Thread FXRatesProvider;
        private FXRatesProviderThreadInfo Info;


        private Visibility _appVisibility;
        private string _selectedPageName = string.Empty;


        private DataTable _popupData;
        private Icon _taskbarIcon;


        private IConfigManager _configManager;
        private IDataProvider _dataProvider;


        public Visibility AppVisibility
        {
            get => _appVisibility;
            set
            {
                _appVisibility = value;
                OnPropertyChanged(nameof(AppVisibility));
            }
        }

        public ListBoxItem? SelectedItem
        {
            set
            {
                if (value != null)
                {
                    SelectedPageName = $"Pages/{value.Name}.xaml";
                }
            }
        }

        public string SelectedPageName
        {
            get => _selectedPageName;
            set
            {
                _selectedPageName = value;
                OnPropertyChanged(nameof(SelectedPageName));
            }
        }

        public DataTable PopupData
        {
            get => _popupData;
            set
            {
                _popupData = value;
                OnPropertyChanged(nameof(PopupData));
            }
        }

        public Icon TaskbarIcon
        {
            get => _taskbarIcon;
            set
            {
                _taskbarIcon = value;
                OnPropertyChanged(nameof(TaskbarIcon));
            }
        }


        private void OnActualRatesChanged()
        {
            foreach (FXRate rate in Info.ActualRates)
            {
                if (Info.Config.PopupRatesNames.Contains(rate.Name))
                {
                    DataRow rw = PopupData.NewRow();

                    rw["Currency Name"] = rate.Name;
                    rw["Exchange Rate"] = rate.ExchangeRate;

                    PopupData.Rows.Add(rw);
                }

                if (Info.Config.IconRateName == rate.Name)
                {
                    decimal value = rate.ExchangeRate;

                    value = Math.Round(value, 0);

                    string content = value.ToString();

                    TaskbarIcon = content.GenerateIcon();
                }
            }
            PopupData = PopupData;
        }


        public RelayCommand HideBaseApplicationCommand => new RelayCommand((param) =>
        {
            AppVisibility = Visibility.Collapsed;
        });

        public RelayCommand ShowBaseApplicationCommand => new RelayCommand((param) =>
        {
            AppVisibility = Visibility.Visible;
        });

        public RelayCommand CloseApplication => new RelayCommand((param) =>
        {
            Application.Current.Shutdown();
        });

        public RelayCommand ReloadAppCommand => new RelayCommand((param) =>
        {
            DataTable dt = PopupData;
            dt.Clear();

            PopupData = dt;

            Info = new FXRatesProviderThreadInfo(_configManager.GetConfig() ?? throw new InvalidExpressionException(), _dataProvider);
            Info.ActualRatesChanged += OnActualRatesChanged;

            FXRatesProvider = new Thread(Info.RefreshActualRates);
            FXRatesProvider.IsBackground = true;

            FXRatesProvider.Start();
        });
    }
}
