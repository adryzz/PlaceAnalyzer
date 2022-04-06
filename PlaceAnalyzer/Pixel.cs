using System.Drawing;
using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace PlaceAnalyzer;

public struct Pixel
{
    public static readonly InstantPattern TimestampPattern =
        InstantPattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss.fff 'UTC'");
    public Instant Timestamp { get; set; }
    
    public string Uid { get; set; }
    
    public Color Color { get; set; }
    
    public ushort X { get; set; }
    
    public ushort Y { get; set; }
    
    /// <summary>
    /// Mod only
    /// </summary>
    public ushort Width { get; set; }
    
    /// <summary>
    /// Mod only
    /// </summary>
    public ushort Height { get; set; }

    public Pixel(RedditPixel p)
    {
        Uid = p.Uid;
        Color = ColorTranslator.FromHtml(p.Color);
        
        Timestamp = TimestampPattern.Parse(p.Timestamp).Value;

        string[] pos = p.Coordinates.Split(',');
        
        X = ushort.Parse(pos[0]);
        Y = ushort.Parse(pos[1]);
        
        Width = 0;
        Height = 0;
        
        if (pos.Length > 2)
        {
            Width = ushort.Parse(pos[2]);
            Width = ushort.Parse(pos[3]);
        }
    }
}