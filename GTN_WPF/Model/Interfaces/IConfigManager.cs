namespace GTN_WPF.Model.Interfaces
{
    public interface IConfigManager
    {
        public AppConfig? GetConfig();

        public void RecreateConfig();

        public void UpdateConfig(AppConfig config);
    }
}
