namespace Framework.Common
{
    public interface ISerializator<T> where T : class
    {
        void Serialize(string fileName, T obj);
        T Deserialize(string fileName);
    }
}