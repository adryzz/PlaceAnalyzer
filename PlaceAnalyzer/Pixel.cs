using System.Drawing;
using NodaTime;
using NodaTime.Text;

namespace PlaceAnalyzer;

public struct Pixel
{
    public static Dictionary<Uid, uint> UserMap = new Dictionary<Uid, uint>();

    public static uint Users = 0;

    public static readonly InstantPattern TimestampPattern =
        InstantPattern.CreateWithInvariantCulture("yyyy-MM-dd HH:mm:ss.fff 'UTC'");

    //public Instant Timestamp;

    public uint Uid;

    private uint internalPosColor; // 11 bits X, 11 bits Y, 5 bits color

    public uint Color => internalPosColor << 27;

    public uint X => internalPosColor << 11;

    public uint Y => internalPosColor << 22;
    /*
    /// <summary>
    /// Mod only
    /// </summary>
    public ushort Width;
    
    /// <summary>
    /// Mod only
    /// </summary>
    public ushort Height;*/
    public Pixel(RedditPixel p)
    {
        Uid u = new Uid(p.Uid);
        lock (UserMap)
        {
            if (UserMap.ContainsKey(u))
            {
                Uid = UserMap[u];
            }
            else
            {
                uint i = Interlocked.Increment(ref Users);

                UserMap.Add(u, i);
                Uid = i;
            }
        }

        var c = ColorTranslator.FromHtml(p.Color);
        Colors color = (Colors)(((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B) & 0xffffffffL);
        
        //Timestamp = TimestampPattern.Parse(p.Timestamp).Value;
        //Timestamp = default;
        string[] pos = p.Coordinates.Split(',');
        
        internalPosColor = uint.Parse(pos[0]);
        internalPosColor |= uint.Parse(pos[1]) << 11;
        internalPosColor |= (uint) color << 27;

        /*Width = 0;
        Height = 0;
        
        if (pos.Length > 2)
        {
            Width = ushort.Parse(pos[2]);
            Width = ushort.Parse(pos[3]);
        }*/
    }
    
    public enum Colors : uint
    {
        Burgundy = 0x6D001A,
        DarkRed = 0xBE0039,
        Red = 0xFF4500,
        Orange = 0xFFA800,
        Yellow = 0xFFD635,
        PaleYellow = 0xFFF8B8,
        DarkGreen = 0x00A368,
        Green = 0x00CC78,
        LightGreen = 0x7EED56,
        DarkTeal = 0x00756F,
        Teal = 0x009EAA,
        LightTeal = 0x00CCC0,
        DarkBlue = 0x2450A4,
        Blue = 0x3690EA,
        LightBlue = 0x51E9F4,
        Indigo = 0x493AC1,
        Periwinkle = 0x6A5CFF,
        Lavender = 0x94B3FF,
        DarkPurple = 0x811E9F,
        Purple = 0xB44AC0,
        PalePurple = 0xE4ABFF,
        Magenta = 0xDE107F,
        Pink = 0xFF3881,
        LightPink = 0xFF99AA,
        DarkBrown = 0x6D482F,
        Brown = 0x9C6926,
        Beige = 0xFFB470,
        Black = 0x000000,
        DarkGray = 0x515252,
        Gray = 0x898D90,
        LightGray = 0xD4D7D9,
        White = 0xFFFFFF
    }
    
    public static byte GetColor(Colors color) => color switch
    {
        Colors.Burgundy => 0,
        Colors.DarkRed => 1,
        Colors.Red => 2,
        Colors.Orange => 3,
        Colors.Yellow => 4,
        Colors.PaleYellow => 5,
        Colors.DarkGreen => 6,
        Colors.Green => 7,
        Colors.LightGreen => 8,
        Colors.DarkTeal => 9,
        Colors.Teal => 10,
        Colors.LightTeal => 11,
        Colors.DarkBlue => 12,
        Colors.Blue => 13,
        Colors.LightBlue => 14,
        Colors.Indigo => 15,
        Colors.Periwinkle => 15,
        Colors.Lavender => 16,
        Colors.DarkPurple => 17,
        Colors.Purple => 18,
        Colors.PalePurple => 19,
        Colors.Magenta => 20,
        Colors.Pink => 21,
        Colors.LightPink => 23,
        Colors.DarkBrown => 24,
        Colors.Brown => 25,
        Colors.Beige => 26,
        Colors.Black => 27,
        Colors.DarkGray => 28,
        Colors.Gray => 29,
        Colors.LightGray => 30,
        Colors.White => 31
    };
    
    public static Colors GetColor(byte color) => color switch
    {
        0 => Colors.Burgundy,
        1 => Colors.DarkRed,
        2 => Colors.Red,
        3 => Colors.Orange,
        4 => Colors.Yellow,
        5 => Colors.PaleYellow,
        6 => Colors.DarkGreen,
        7 => Colors.Green,
        8 => Colors.LightGreen,
        9 => Colors.DarkTeal,
        10 => Colors.Teal,
        11 => Colors.LightTeal,
        12 => Colors.DarkBlue,
        13 => Colors.Blue,
        14 => Colors.LightBlue,
        15 => Colors.Indigo,
        16 => Colors.Periwinkle,
        17 => Colors.Lavender,
        18 => Colors.DarkPurple,
        19 => Colors.Purple,
        20 => Colors.PalePurple,
        21 => Colors.Magenta,
        22 => Colors.Pink,
        23 => Colors.LightPink,
        24 => Colors.DarkBrown,
        25 => Colors.Brown,
        26 => Colors.Beige,
        27 => Colors.Black,
        28 => Colors.DarkGray,
        29 => Colors.Gray,
        30 => Colors.LightGray,
        31 => Colors.White
    };
}