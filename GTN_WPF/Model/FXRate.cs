using Newtonsoft.Json;

namespace GTN_WPF.Model
{
    public class FXRate
    {
        public static FXRate Default => new FXRate(0, "RUB");
        public static FXRate Unknown => new FXRate(-1, "Unknown");

        private decimal _exchangeRate = 0;
        private string _name = string.Empty;

        public string Name => _name;
        public decimal ExchangeRate => _exchangeRate;


        public override string ToString()
        {
            return Name;
        }


        [JsonConstructor]
        internal FXRate(decimal exchangeRate, string name)
        {
            _exchangeRate = exchangeRate;
            _name = name;
        }
    }
}
