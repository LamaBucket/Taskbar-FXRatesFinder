using System.Collections.Generic;

namespace GTN_WPF.Model.Interfaces
{
    public interface IDataProvider
    {
        IList<FXRate> SearchRates(string searchFilter);

        FXRate GetFXRate(string localCurrencyName, string targetCurrencyName);
    }
}
