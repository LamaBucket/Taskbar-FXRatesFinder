using GTN_WPF.Model;
using GTN_WPF.Model.Implementations;
using GTN_WPF.Model.Interfaces;
using GTN_WPF.ViewModel.Pages;
using System;
using System.Windows.Controls;

namespace GTN_WPF.View.Pages
{
    /// <summary>
    /// Interaction logic for List.xaml
    /// </summary>
    public partial class List : Page
    {
        public List()
        {
            InitializeComponent();

            IConfigManager manager = new ConfigManagerFile();

            AppConfig? config = manager.GetConfig();

            if (config is null || config.ApiKey == String.Empty)
                throw new Exception("Set Api Key First!");

            IDataProvider provider = new DataProviderWeb(config.ApiKey);

            DataContext = new ListViewModel(manager, provider);
        }
    }
}
