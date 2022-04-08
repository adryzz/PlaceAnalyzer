using System.Runtime.Intrinsics;

namespace PlaceAnalyzer;

public struct Uid
{
    public Vector256<byte> HighBytes;
    public Vector256<byte> LowBytes;

    public Uid(byte[] arr)
    {
        if (arr.Length != 64)
            throw new ArgumentException();
        LowBytes = Vector256.Create(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7], arr[8],
            arr[9], arr[10], arr[11], arr[12], arr[13], arr[14], arr[15], arr[16], arr[17], arr[18], arr[19],
            arr[20], arr[21], arr[22], arr[23], arr[24], arr[25], arr[26], arr[27], arr[28], arr[29], arr[30],
            arr[31]);
        
        HighBytes = Vector256.Create(arr[32], arr[33], arr[34], arr[35], arr[36], arr[37], arr[38], arr[39], arr[40],
            arr[41], arr[42], arr[43], arr[44], arr[45], arr[46], arr[47], arr[48], arr[49], arr[50], arr[51],
            arr[52], arr[53], arr[54], arr[55], arr[56], arr[57], arr[58], arr[59], arr[60], arr[61], arr[62],
            arr[63]);
    }

    public Uid(string b64) : this(Convert.FromBase64String(b64))
    {
    }
}