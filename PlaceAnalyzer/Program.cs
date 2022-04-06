using System.Diagnostics;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using CsvHelper;

namespace PlaceAnalyzer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IEnumerable<string> files = Directory.EnumerateFiles("/mnt/adryzz/stats", "*.gzip");
            foreach (var file in files)
            {
                int count = 0;
                Stopwatch stopwatch = Stopwatch.StartNew();
                
                FileStream fs = File.OpenRead(file);
                BufferedStream buf = new BufferedStream(fs);
                GZipStream gzip = new GZipStream(buf, CompressionMode.Decompress);
                
                using (var reader = new StreamReader(gzip))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<RedditPixel>();

                    foreach (var rpix in records)
                    {
                        Pixel pix = new Pixel(rpix);
                        count++;
                    }
                }
                Console.WriteLine(stopwatch.Elapsed);
            }
        }
    }
}