namespace Framework.Common.Security
{
    public interface IEncryptor
    {
        string Encode(string data);
        string Decode(string dataToEncode);
    }
}