using System;
using System.Windows;
using System.Windows.Threading;

namespace GTN_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(GetInnerException(e.Exception).Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }

        private static Exception GetInnerException(Exception ex)
        {
            if (ex.InnerException is null)
                return ex;
            else return GetInnerException(ex.InnerException);
        }
    }
}
