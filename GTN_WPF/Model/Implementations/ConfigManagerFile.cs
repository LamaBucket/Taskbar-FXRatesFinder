using GTN_WPF.Model.Interfaces;
using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace GTN_WPF.Model.Implementations
{
    public class ConfigManagerFile : IConfigManager
    {
        public ConfigManagerFile()
        {
        }

        public AppConfig? GetConfig()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), SystemConstants.AppConfigName);

            if (!File.Exists(path))
                CreateConfig(path);

            string content = File.ReadAllText(path);

            if (string.IsNullOrEmpty(content))
                return null;

            return JsonConvert.DeserializeObject<AppConfig>(content);
        }

        public void RecreateConfig()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), SystemConstants.AppConfigName);

            if (File.Exists(path))
                File.Delete(path);

            CreateConfig(path);
        }

        public void UpdateConfig(AppConfig config)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), SystemConstants.AppConfigName);

            if (!File.Exists(path))
                CreateConfig(path);

            File.WriteAllText(path, JsonConvert.SerializeObject(config));
        }

        private void CreateConfig(string path)
        {
            string json = JsonConvert.SerializeObject(new AppConfig());

            using (FileStream fs = File.Create(path))
            {
                fs.Write(Encoding.Default.GetBytes(json));
            }
        }


    }
}
