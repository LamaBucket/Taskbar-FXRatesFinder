using GTN_WPF.Model;
using GTN_WPF.Model.Interfaces;
using System;

namespace GTN_WPF.ViewModel
{
    public class ViewModelBaseConfig : ViewModelBase
    {
        internal protected IConfigManager _configManager;

        internal protected AppConfig _config;

        internal protected virtual void RefreshConfig()
        {
            _configManager.UpdateConfig(_config);
            _config = _configManager.GetConfig() ?? throw new ArgumentNullException();

            _config.ConfigChanged += RefreshConfig;
        }


        public ViewModelBaseConfig(IConfigManager configManager)
        {
            _configManager = configManager;
            _config = _configManager.GetConfig() ?? throw new ArgumentNullException();

            _config.ConfigChanged += RefreshConfig;
        }
    }
}
