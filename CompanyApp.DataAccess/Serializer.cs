using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CompanyApp.DataAccess
{
    public class Serializer<T> where T: class
    {
        private readonly string _fileName;
        public Serializer(string fileName)
        {
            _fileName = fileName;
        }

        public void Save(T data)
        {
            using Stream st = File.Open(_fileName, FileMode.Create, FileAccess.Write);
            JsonSerializer.Serialize(st, data, new JsonSerializerOptions { WriteIndented = true });
        }

        public T Load()
        {
            using Stream st = File.Open(_fileName, FileMode.Open, FileAccess.Read);
            return JsonSerializer.Deserialize<T>(st);
        }
    }
}
