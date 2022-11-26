using OpenCvSharp;
using Point = OpenCvSharp.Point;

public class Photo2Text
{
    private static String ascii_lists = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'.  ";
    private Int32 W = 0;
    private Int32 H = 0;
    
    public static void Main(System.String[] args)
    {
        Photo2Text P2T = new Photo2Text();
        String str = P2T.asciiPicPerPil();
        FileStream F = new FileStream("res.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
        for(int i = 0; i < str.Length; i++)
        {   
            F.WriteByte((byte)str[i]);
        }
    }
    
    public int[] getNearestCharPerPixel(int x, int y, int target_weight, int target_height)
    {
        // f(x, y) = f(W / target_weight * x, H / target_height * y)
        int X = W / target_weight * x;
        int Y = H / target_height * y;
        return new int[] { X, Y };  // Pos write back
    }
    
    /*
    public int[] getLinearCharPerPixel(int x, int y, int target_weight, int target_height)
    {

    }
    */
    
    public Char getCharsFromPixel(int r, int g, int b, int alpha = 256)
    {
        if(r == null || g == null || b == null || alpha == null)
        {
            Console.WriteLine("Unacceptable value: " + r + ", " + g + ", " + b + " !");
        }
        if(r > 256 || r < -256)
        {
            r = 256;
        }
        if(g > 256 || g < -256)
        {
            g = 256;
        }
        if(b > 256 || b < -256)
        {
            b = 256;
        }
        if(alpha > 256 || alpha < -256)
        {
            alpha = 256;
        }
        
        if(r < 0)
        {
            r = (r * -1);
        }
        if(g < 0)
        {
            g = (g * -1);
        }
        if(b < 0)
        {
            b = (b * -1);
        }
        if(alpha < 0)
        {
            alpha = (alpha * -1);
        }
        // if(alpha == 0)
        // {
        //     return ' ';
        // }
        
        int length = ascii_lists.Length;
        int gray_scales = (int)(0.2126 * r + 0.7152 * g + 0.0722 * b);
        double unit = (256.0 + 1) / length;
        return ascii_lists[(int)(gray_scales / unit)];
    }
    
    public String asciiPicPerPil()
    {
        String str = "";
        Mat im = new Mat(@"./1.WEBP", ImreadModes.Color | ImreadModes.AnyDepth);
        this.W = im.Width;
        this.H = im.Height;
        im.Resize(new Size(this.W, this.H), this.W, this.H, InterpolationFlags.Linear);
        for(int i = 0; i < im.Rows; i++)
        {
            for(int j = 0; j < im.Cols; j++)
            {
                double alpha = 0.5f;
                Vec3b color = im.At<Vec3b>(i, j);
                str += getCharsFromPixel(color.Item2, color.Item1, color.Item0);
            }
            str += '\n';
        }
        return str;
    }
}
