using GTN_WPF.Model.Implementations;
using GTN_WPF.Model.Interfaces;
using GTN_WPF.ViewModel.Pages;
using System.Windows.Controls;

namespace GTN_WPF.View.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();

            IConfigManager manager = new ConfigManagerFile();

            DataContext = new SettingsViewModel(manager);
        }
    }
}
