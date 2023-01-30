using GTN_WPF.Model.Interfaces;
using System.Collections.Generic;
using System.Threading;

namespace GTN_WPF.Model
{
    public class FXRatesProviderThreadInfo
    {
        public event OnActualRatesChanged? ActualRatesChanged;

        public delegate void OnActualRatesChanged();


        private AppConfig _config;
        private IDataProvider _dataProvider;
        private IList<FXRate> _actualRates;


        public IList<FXRate> ActualRates
        {
            get => _actualRates;
            set
            {
                _actualRates = value;
                ActualRatesChanged?.Invoke();
            }
        }
        public IDataProvider DataProvider => _dataProvider;
        public AppConfig Config => _config;

        public void RefreshActualRates()
        {
            IList<FXRate> rates = new List<FXRate>();

            while (true)
            {
                rates.Clear();

                foreach (string targetCurrency in _config.BoundRatesNames)
                {
                    rates.Add(_dataProvider.GetFXRate(_config.LocalCurrencyName, targetCurrency));
                }

                ActualRates = rates;

                Thread.Sleep(_config.RatesRefreshTime);
            }
        }

        internal FXRatesProviderThreadInfo(AppConfig config, IDataProvider dataProvider)
        {
            _config = config;
            _dataProvider = dataProvider;
            _actualRates = new List<FXRate>();
        }
    }
}
