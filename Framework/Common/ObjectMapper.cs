namespace Framework.Common
{
    using System;
    using System.Collections.Generic;
    using Model;
    using Security;

    public class ObjectMapper<T> where T : Entity, new()
    {
        private readonly IEncryptor encryptor;

        public ObjectMapper(IEncryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        public T Map(IDictionary<string, string> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var entity = new T();
            string id = GetValue("id", data);
            string name = GetValue("name", data);
            entity.Name = name;
            entity.Id = int.Parse(id);
            return entity;

        }


        public IEnumerable<T> MapCollection(IList<Dictionary<string, string>> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            var entities = new List<T>();
            foreach (var item in data)
                entities.Add(Map(item));
            return entities;
        }

        private string GetValue(string keyName, IDictionary<string, string> dictionary)
        {

            string value;
            if (dictionary.TryGetValue(keyName, out value))
            {
                value = encryptor.Decode(value);
            }
            return value;
        }
    }
}