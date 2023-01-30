using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GTN_WPF.Model
{
    public class AppConfig
    {
        public event OnConfigChanged? ConfigChanged;

        public delegate void OnConfigChanged();



        private TimeSpan _ratesRefreshTime = TimeSpan.FromMinutes(5);
        private List<string> _popupRatesNames = new List<string>() { FXRate.Default.Name };

        private string _iconRateName = FXRate.Default.Name;
        private string _localCurrencyName = FXRate.Default.Name;
        private string _apiKey = String.Empty;


        public string ApiKey => _apiKey;
        public TimeSpan RatesRefreshTime => _ratesRefreshTime;
        public List<String> PopupRatesNames => _popupRatesNames;
        public String IconRateName => _iconRateName;

        public string LocalCurrencyName => _localCurrencyName;

        public List<String> BoundRatesNames => new List<String>(PopupRatesNames).Append(IconRateName).Distinct().ToList();

        internal bool BindItem(string name)
        {
            try
            {
                if (BoundRatesNames.Contains(name))
                    return true;

                _popupRatesNames.Add(name);


                ConfigChanged?.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal bool UnbindItem(string name)
        {
            try
            {
                if (!BoundRatesNames.Contains(name))
                    return true;

                _popupRatesNames.Remove(name);


                ConfigChanged?.Invoke();
                return true;
            }
            catch
            {
                return false;
            }
        }


        [JsonConstructor]
        internal AppConfig(string apiKey, List<string> popupRatesNames, string iconRateName, TimeSpan ratesRefreshTime, string localCurrencyName)
        {
            _apiKey = apiKey;
            _popupRatesNames = popupRatesNames;
            _iconRateName = iconRateName;
            _ratesRefreshTime = ratesRefreshTime;
            _localCurrencyName = localCurrencyName;
        }

        internal AppConfig()
        {
        }
    }
}
