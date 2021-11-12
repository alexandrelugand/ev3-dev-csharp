using System;
using System.IO;

namespace LcdTest
{
    public class Program
    {
        // ReSharper disable once UnusedMember.Local
        private const short ScreenWidth = 178; //pixels
        private const short ScreenHeight = 128; //pixels
        private const short LineLength = 24;// bytes
        private const short Size = 3072; //bytes

        public static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine("Trying to write in /dev/fb0");
                var buf = new byte[Size];

                // draw a vertical line in column 100 (0 based index)
                for (var row = 0; row < ScreenHeight; row++)
                {
                    buf[row * LineLength + 100 / 8] = 1 << (100 % 8);
                }

                // draw a horizontal line in row 64 (0 based index)
                for (var col = 0; col < LineLength; col++)
                {
                    buf[64 * LineLength + col] = 0xff;
                }

                // draw a circle, center at (40,40), radius is 20
                for (var x = 0; x < 20; x++)
                {
                    var y = Math.Sqrt(20 * 20 - x * x);
                    buf[(40 + (int)(y)) * LineLength + (40 + x) / 8] = (byte)(1 << ((40 + x) % 8));
                    buf[(40 - (int)(y)) * LineLength + (40 + x) / 8] = (byte)(1 << ((40 + x) % 8));
                    buf[(40 + (int)(y)) * LineLength + (40 - x) / 8] = (byte)(1 << ((40 - x) % 8));
                    buf[(40 - (int)(y)) * LineLength + (40 - x) / 8] = (byte)(1 << ((40 - x) % 8));
                }

                using (var fileStream = new FileStream("/dev/fb0", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    foreach (var b in buf)
                    {
                        fileStream.WriteByte(b);
                    }
                    fileStream.Flush();
                }
                //Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
            }
            Console.ReadKey();
        }
    }
}
