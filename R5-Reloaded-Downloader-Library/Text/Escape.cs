using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.Text
{
    public static class Escape
    {
        public static string Combine(params int[] seq) => "\x1b[" + string.Join(';', seq) + 'm';

        public static string Combine(string type, int red, int green, int blue)
        {
            var rgbType = type switch
            {
                "foreground" => Sequence.Foreground.Rgb,
                "background" => Sequence.Background.Rgb,
                _ => throw new NotImplementedException()
            };
            return Combine(Sequence.Initialize, rgbType, Sequence.RgbArg, red, green, blue);
        }

        public static class Sequence
        {
            public const int Initialize = 0;
            public const int Highlight = 1;
            public const int Lowlight = 2;
            public const int RgbArg = 2;
            public const int Italics = 3;
            public const int UnderLine = 4;
            public const int Blink = 5;
            public const int Reverse = 7;

            public static class Foreground
            {
                public const int Black = 30;
                public const int Red = 31;
                public const int Green = 32;
                public const int Yellow = 33;
                public const int Blue = 34;
                public const int Magenta = 35;
                public const int Cyan = 36;
                public const int White = 37;
                public const int Rgb = 38;
                public const int Default = 39;

                public static class Light
                {
                    public const int Black = 90;
                    public const int Red = 91;
                    public const int Green = 92;
                    public const int Yellow = 93;
                    public const int Blue = 94;
                    public const int Magenta = 95;
                    public const int Cyan = 96;
                    public const int White = 97;
                }
            }

            public static class Background
            {
                public const int Black = 40;
                public const int Red = 41;
                public const int Green = 42;
                public const int Yellow = 43;
                public const int Blue = 44;
                public const int Magenta = 45;
                public const int Cyan = 46;
                public const int White = 47;
                public const int Rgb = 48;
                public const int Default = 49;

                public static class Light
                {
                    public const int Black = 100;
                    public const int Red = 101;
                    public const int Green = 102;
                    public const int Yellow = 103;
                    public const int Blue = 104;
                    public const int Magenta = 105;
                    public const int Cyan = 106;
                    public const int White = 107;
                }
            }
        }
    }
}
