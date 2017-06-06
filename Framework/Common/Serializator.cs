namespace Framework.Common
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    public class Serializator<T> : ISerializator<T> where T : class
    {
        public void Serialize(string fileName, T obj)
        {
            try
            {

                XmlSerializer xmlser = new XmlSerializer(typeof(T));

                FileStream filestream = new FileStream(fileName, FileMode.Create);

                xmlser.Serialize(filestream, obj);

                filestream.Close();
            }
            catch(Exception ex)
            {
                throw new SerializationException(ex.Message);
            }
        }

        public T Deserialize(string fileName)
        {
            FileInfo fl = new FileInfo(fileName);

            if (fl.Exists)
            {
                var filestream = new FileStream(fileName, FileMode.Open);

                try
                {

                    var xmlser = new XmlSerializer(typeof(T));


                    var obj = (T)xmlser.Deserialize(filestream);

                    filestream.Close();

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