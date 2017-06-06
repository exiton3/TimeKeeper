using Framework.Model;

namespace Framework.Repository.Abstractions
{
    public interface IAppSettingsRepository
    {
        AppSettings Get();
        void Save(AppSettings appSettings);
    }
}