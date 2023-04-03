using GTN_WPF.Model;
using GTN_WPF.Model.Implementations;
using GTN_WPF.Model.Interfaces;
using GTN_WPF.ViewModel;
using System;
using System.ComponentModel;
using System.Windows;

namespace GTN_WPF.View
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        private string IconPropertyName = String.Empty;

        public Main()
        {
            InitializeComponent();

            Icon = ToImageSource(Properties.Resources.ApplicationIcon);

            IConfigManager manager = new ConfigManagerFile();

            AppConfig? config = manager.GetConfig();

            if (config is null || String.IsNullOrEmpty(config.ApiKey))
            {
                MessageBox.Show("Set ApiKey in GTNAppConfig.json first!");

                Application.Current.Shutdown();
            }

            MainViewModel vm = new MainViewModel(manager, new DataProviderWeb(config.ApiKey));

            vm.PropertyChanged += UpdateTaskbarIcon;
            IconPropertyName = nameof(vm.TaskbarIcon);

            DataContext = vm;
            TaskbarControl.Icon = vm.TaskbarIcon;
        }

        private void UpdateTaskbarIcon(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == IconPropertyName && sender is MainViewModel)
            {
                MainViewModel vm = (MainViewModel)sender;

                TaskbarControl.Icon = vm.TaskbarIcon;
            }
        }

        private static System.Windows.Media.ImageSource ToImageSource(System.Drawing.Icon icon)
        {
            System.Windows.Media.ImageSource imageSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                System.Windows.Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
