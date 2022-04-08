using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using CsvHelper;
using StreamUtils;

namespace PlaceAnalyzer
{
    public static class Program
    {
        public static unsafe void Main(string[] args)
        {
            Console.WriteLine(sizeof(Pixel));
            int count = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<Pixel> pixels = new List<Pixel>(12000000);

            IEnumerable<string> files = Directory.EnumerateFiles("/run/media/adryzz/a", "*.gzip");
            foreach(string file in files)
            {
                Console.WriteLine($"Loading {file}...");
                using FileStream fs = File.OpenRead(file);
                using BufferedStream buf = new BufferedStream(fs);
                using GZipStream gzip = new GZipStream(buf, CompressionMode.Decompress);

                using (var reader = new StreamReader(gzip))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<RedditPixel>();

                    foreach (var rpix in records)
                    {
                        Pixel pix = new Pixel(rpix);
                        pixels.Add(pix);
                    }
                }
                gzip.Dispose();
                buf.Dispose();
                fs.Dispose();
                Console.WriteLine($"File loaded! pixels: {pixels.Count}");
                Console.WriteLine($"Current memory usage: {format(GC.GetTotalMemory(true))}");
                Interlocked.Increment(ref count);
                Console.WriteLine($"loaded {count}/{files.Count()} files");
            }
        }
        

        static bool inRange(Pixel p, int x, int y, int w, int h)
        {
            return (p.X >= x && p.Y >= y && p.X <= w && p.Y <= h);
        }

        static string format(long i)
        {

                // Get absolute value
                long absolute_i = (i < 0 ? -i : i);
                // Determine the suffix and readable value
                string suffix;
                double readable;
                if (absolute_i >= 0x1000000000000000) // Exabyte
                {
                    suffix = "EB";
                    readable = (i >> 50);
                }
                else if (absolute_i >= 0x4000000000000) // Petabyte
                {
                    suffix = "PB";
                    readable = (i >> 40);
                }
                else if (absolute_i >= 0x10000000000) // Terabyte
                {
                    suffix = "TB";
                    readable = (i >> 30);
                }
                else if (absolute_i >= 0x40000000) // Gigabyte
                {
                    suffix = "GB";
                    readable = (i >> 20);
                }
                else if (absolute_i >= 0x100000) // Megabyte
                {
                    suffix = "MB";
                    readable = (i >> 10);
                }
                else if (absolute_i >= 0x400) // Kilobyte
                {
                    suffix = "KB";
                    readable = i;
                }
                else
                {
                    return i.ToString("0 B"); // Byte
                }
                // Divide by 1024 to get fractional value
                readable = (readable / 1024);
                // Return formatted number with suffix
                return readable.ToString("0.### ") + suffix;
        }
    }
}