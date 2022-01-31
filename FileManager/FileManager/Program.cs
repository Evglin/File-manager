using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FileManager
{
    class Program
    {
   
        static void Main(string[] args)
        {
            
            Console.SetWindowSize(105, 37);
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth+1;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.Clear();

            Manager fm = new Manager();
            fm.printbox();
            fm.DriveGet();

            fm.PrintCommandLine("");
            ConsoleKeyInfo keypressed;
            fm.CommandLine = "";

            do
            {
               keypressed = Console.ReadKey();
               fm.Keypress(keypressed); 
            }
            while (true);
            
        }   
    }

   
}
