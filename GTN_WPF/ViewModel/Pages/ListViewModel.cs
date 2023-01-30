using GTN_WPF.Model;
using GTN_WPF.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace GTN_WPF.ViewModel.Pages
{
    public class ListViewModel : ViewModelBaseConfig
    {
        private IDataProvider _provider;


        private string _searchQuery = String.Empty;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value ?? String.Empty;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private DataTable _rates;

        public DataTable Rates
        {
            get => _rates;
            set
            {
                _rates = value;
                OnPropertyChanged(nameof(Rates));
            }
        }



        public RelayCommand SearchCommand => new RelayCommand((param) =>
       {
           IEnumerable<FXRate> searchResult = _provider.SearchRates(SearchQuery);

           UpdateRatesTable(searchResult, _config.BoundRatesNames);
       });

        public RelayCommand ChangeBinding => new RelayCommand((param) =>
       {
           if (param is DataRowView)
           {
               DataRowView rowView = (DataRowView)param;

               DataRow rw = Rates.NewRow();



               bool bound = (bool)rowView.Row["Bound To Currency"];
               string name = (string)rowView.Row["Currency Name"];
               decimal exchangeRate = (decimal)rowView.Row["Exchange Rate"];

               if (bound)
               {
                   _config.UnbindItem(name);
               }
               else
               {
                   _config.BindItem(name);
               }

               rw["Currency Name"] = name;
               rw["Exchange Rate"] = exchangeRate;
               rw["Bound To Currency"] = !bound;

               Rates.Rows.Remove(rowView.Row);
               Rates.Rows.InsertAt(rw, 0);
           }
       });


        public ListViewModel(IConfigManager configManager, IDataProvider dataProvider) : base(configManager)
        {
            _provider = dataProvider;

            _rates = new DataTable();

            _rates.Columns.Add("Currency Name", typeof(string));
            _rates.Columns.Add("Exchange Rate", typeof(decimal));
            _rates.Columns.Add("Bound To Currency", typeof(bool));
        }

        private void UpdateRatesTable(IEnumerable<FXRate> rates, IList<String> boundRates)
        {
            bool bound = false;

            Rates.Clear();

            foreach (FXRate rate in rates)
            {
                if (boundRates.Contains(rate.Name))
                    bound = true;

                DataRow rw = Rates.NewRow();

                rw["Currency Name"] = rate.Name;
                rw["Exchange Rate"] = rate.ExchangeRate;
                rw["Bound To Currency"] = bound;

                Rates.Rows.Add(rw);

                bound = false;
            }

        }
    }
}
