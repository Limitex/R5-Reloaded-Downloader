using R5_Reloaded_Downloader_Library.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace R5_Reloaded_Downloader_Library.IO
{
    public static class ConsoleExpansion
    {
        private static readonly int InformationMaxWidth = 5;

        [DllImport("kernel32.dll", SetLastError = true)] private static extern IntPtr GetStdHandle(int nStdHandle);
        [DllImport("kernel32.dll")] private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);
        [DllImport("kernel32.dll")] private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        private static readonly Hashtable ColorEscape = new()
        {
            ["Black"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Black),
            ["Gray"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.White, Escape.Sequence.Lowlight),
            ["Red"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Red),
            ["Green"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Green),
            ["Yellow"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Yellow),
            ["Blue"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Blue),
            ["Magenta"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Magenta),
            ["Cyan"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Cyan),
            ["White"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.White),
            ["Default"] = Escape.Combine(Escape.Sequence.Initialize, Escape.Sequence.Foreground.Default),
        };

        private static string LogInfo(string info, string color, string value) =>
            ColorEscape["Magenta"] + "[ " +
            ColorEscape[color] + info.PadRight(InformationMaxWidth) +
            ColorEscape["Magenta"] + " ][ " +
            ColorEscape["Gray"] + DateTime.Now.ToString("yyyy/MM/dd") + " " +
            ColorEscape["White"] + DateTime.Now.ToString("HH:mm:ss") +
            ColorEscape["Magenta"] + " ] " +
            ColorEscape["Default"] + value;

        public static void LogWrite(string value) => Console.Write('\n' + LogInfo("Info", "Green", value));
        public static void LogNotes(string value) => Console.Write('\n' + LogInfo("Notes", "Yellow", value));
        public static void LogError(string value) => Console.Write('\n' + LogInfo("Error", "Red", value));
        public static void LogDebug(string value) => Console.Write('\n' + LogInfo("Debug", "Blue", value));
        public static void LogInput(string value) => Console.Write('\n' + LogInfo("Input", "Cyan", value));

        public static void WriteWidth(char c, string? text = null)
        {
            var outString = "";
            if (text == null)
                for (int i = 0; i < Console.WindowWidth; i++) outString += c;
            else
            {
                var size = (Console.WindowWidth / 2f) - (text.Length / 2f) - 2f;
                for (int i = 0; i <= size; i++) outString += c;
                outString += ' ' + text + ' ';
                for (int i = 0; i <= size; i++) outString += c;
            }
            Console.Write('\n' + outString);
        }

        public static void DisableEasyEditMode()
        {
            const int STD_INPUT_HANDLE = -10;
            const uint ENABLE_QUICK_EDIT = 0x0040;

            var consoleHandle = GetStdHandle(STD_INPUT_HANDLE);
            GetConsoleMode(consoleHandle, out uint consoleMode);
            SetConsoleMode(consoleHandle, consoleMode & ~ENABLE_QUICK_EDIT);
        }

        public static void EnableVirtualTerminalProcessing()
        {
            const int STD_OUTPUT_HANDLE = -11;
            const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;

            var consoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);
            GetConsoleMode(consoleHandle, out uint consoleMode);
            SetConsoleMode(consoleHandle, consoleMode | ENABLE_VIRTUAL_TERMINAL_PROCESSING);
        }

        public static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Press the key to exit.");
            Console.ReadKey();
            Environment.Exit(0x8020);
        }
    }
}
