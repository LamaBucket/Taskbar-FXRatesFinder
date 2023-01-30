using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GTN_WPF.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string caller = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
    }
}
