using Framework.Common;
using Framework.Model;
using Framework.Repository.Abstractions;

namespace Framework.Repository.Implementations
{
    public class AppSettingsRepository : IAppSettingsRepository
    {
        private readonly SerializatorIso<AppSettings> serializator;

        private readonly string fileName;

        public AppSettingsRepository()
        {
            serializator = new  SerializatorIso<AppSettings>();
            fileName = "appset.xml";
        }

        public AppSettings Get()
        {
            return serializator.Deserialize(fileName)?? new AppSettings();
        }

        public void Save(AppSettings appSettings)
        {
            serializator.Serialize(fileName,appSettings);
        }
    }
}