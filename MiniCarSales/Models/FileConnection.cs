namespace MiniCarSales.Models
{
    public class FileConnection
    {
        public FileConnection (string path)
        {
            FilePath = path;
        }

        public string FilePath { get; set; }
    }
}