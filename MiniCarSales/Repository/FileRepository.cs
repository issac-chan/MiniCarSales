using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MiniCarSales.Repository
{
    public class FileRepository<T> where T : new()
    {
        public static T ReadDataFromFile(TableType tableType, string filePath)
        {
            using (var file = File.OpenText(string.Format(filePath, tableType.ToString())))
            {
                using (var reader = new JsonTextReader(file))
                {
                    var serializer = new JsonSerializer();

                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        public static bool WriteDataToFile(TableType tableType, string filePath, T data)
        {
            using (var fs = File.Open( string.Format(filePath, tableType.ToString()), FileMode.OpenOrCreate, FileAccess.ReadWrite,FileShare.Read))
            {
                using (var sw = new StreamWriter(fs))
                {
                    using (var jw = new JsonTextWriter(sw))
                    {
                        var serializer = new JsonSerializer();
                        serializer.Serialize(jw, data);
                    }
                }
            }

            return true;          
        }
    }
}