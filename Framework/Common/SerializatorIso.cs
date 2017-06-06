using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Framework.Common
{
    public class SerializatorIso<T> : ISerializator<T> where T : class
    {
        private readonly IsolatedStorageFile isolatedStorage;

        public SerializatorIso()
        {
            isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
        }

        public void Serialize(string fileName, T obj)
        {
            IsolatedStorageFileStream fileStream;
            try
            {

                var xmlser = new XmlSerializer(typeof(T));

                fileStream = new IsolatedStorageFileStream(fileName, FileMode.Create, isolatedStorage);

                xmlser.Serialize(fileStream, obj);

                fileStream.Close();
            }
            catch (Exception ex)
            {
                throw new SerializationException(ex.Message);
            }
        }

        public T Deserialize(string fileName)
        {
            var files = isolatedStorage.GetFileNames(fileName);
            if (files.Length != 0)
            {
                var fileStream = new IsolatedStorageFileStream(fileName, FileMode.Open, isolatedStorage);

                try
                {
                    var xmlser = new XmlSerializer(typeof(T));

                    var obj = (T)xmlser.Deserialize(fileStream);

                    fileStream.Close();

                    return obj;

                }
                catch
                {
                    return null;
                }

            }
            return null;
        }
    }
}