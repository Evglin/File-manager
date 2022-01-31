using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace FileManager
{
    public class CommandExeptions : Exception
    {
        public ErrorCodes _ErrorCode = ErrorCodes.WrongCommand;
        public ErrorCodes ErrorCode
        {
            get
            {
                return _ErrorCode;
            }
            set
            {
                _ErrorCode = value;
            }
        }
    }

    public enum ErrorCodes
    {
        WrongCommand = 100,
        WrongPath = 200,
    }
    class Manager
    {
        public enum WindowModes
        {
            directory = 1,
            files = 2,
            command = 3,
        }

        public WindowModes Mode = WindowModes.directory;

        public struct directory
        {
            public DirectoryInfo info;
            public bool sub;
            public bool noaccess;
            public bool deployed;
            public int level;
            public int DirectoryNumber;
            public int FilesNumber;
        }

        public static List<DriveInfo> DriveList = new List<DriveInfo>();    
        public int currentDrive = 0;
        public int DriveX = 5; // начальная позиция печати списка дисков по горизонтали
        public int DriveY = 25; // позиция печати списка дисков по вертикали

        public int DirectoryesOnPage = Properties.Settings.Default.DirectoryNumber;
        public int filesOnPAge = Properties.Settings.Default.Filesnumbers;


        public List<directory> directorylist = new List<directory>();
        public int CurrentDirectory = 0;
        public int StartDirectoryNumber = 0;
        public int MaxDirectoryName = 20;
        public int DirectoryX = 3;
        public int DirectoryY = 2;
        public List<string> DirectoryPath = new List<string>();

        public List<FileInfo> fileList = new List<FileInfo>();
        public int CurrentFile = 0;
        public int MaxFileName = 25;
        public int FileX = 40;
        public int FileY = 2;

        public List<int> filePageList = new List<int>();
        public int currentPage = 0;
        public int PageX = 41;
        public int PageY = 25;
        public int MaximumPages = 15;


        public List<string> CommandList = new List<string>();
        public string CommandLine = "";
        public int CommandLineX = 1;
        public int CommandLineY = 32;

        public int InfoBoxX = 1;
        public int InfoBoxY = 27;

        public int listindex = 0;
        public int oldlistindex = 0;


        private void drawBox()
        {
            char line = ' ';
            for (int x = 0; x <= Console.WindowWidth - 1; x++)
            {
                for (int y = 0; y <= Console.WindowHeight - 1; y++)
                {
                    bool PrintLine = false;
                    line = ' ';
                    if (x == 0 && y == 0)
                    {
                        line = '┌';
                        PrintLine = true;
                    }
                    else if (x == 0 && y == Console.WindowHeight - 1)
                    {
                        line = '└';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == 0)
                    {
                        line = '┐';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == Console.WindowHeight - 1)
                    {
                        line = '┘';
                        PrintLine = true;
                    }
                    else if (x == 0 || x == Console.WindowWidth - 1)
                    {
                        line = '│';
                        PrintLine = true;
                    }
                    else if (y == 0 || y == Console.WindowHeight - 1)
                    {
                        line = '─';
                        PrintLine = true;
                    }
                    if (PrintLine = true)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(line);
                    }
                }
            }
        }

        private void DrawCommandLine()
        {
            char line = ' ';
            for (int x = 0; x <= Console.WindowWidth - 1; x++)
            {
                for (int y = CommandLineY - 1; y <= CommandLineY + 1; y++)
                {
                    bool PrintLine = false;
                    line = ' ';
                    if (x == 0 && y == CommandLineY - 1)
                    {
                        line = '├';
                        PrintLine = true;
                    }
                    else if (x == 0 && y == CommandLineY + 1)
                    {
                        line = '├';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == CommandLineY - 1)
                    {
                        line = '┤';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == CommandLineY + 1)
                    {
                        line = '┤';
                        PrintLine = true;
                    }
                    else if (x == 0 || x == Console.WindowWidth - 1)
                    {
                        line = '│';
                        PrintLine = true;
                    }
                    else if (y == CommandLineY - 1 || y == CommandLineY + 1)
                    {
                        line = '─';
                        PrintLine = true;
                    }
                    if (PrintLine = true)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(line);
                    }
                }
            }
        }

        private void DrawInfobox()
        {
            char line = ' ';
            for (int x = 0; x <= Console.WindowWidth - 1; x++)
            {
                for (int y = InfoBoxY - 1; y <= InfoBoxY + 4; y++)
                {
                    bool PrintLine = false;
                    line = ' ';
                    if (x == 0 && y == InfoBoxY - 1)
                    {
                        line = '├';
                        PrintLine = true;
                    }
                    else if (x == 0 && y == InfoBoxY + 4)
                    {
                        line = '├';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == InfoBoxY - 1)
                    {
                        line = '┤';
                        PrintLine = true;
                    }
                    else if (x == Console.WindowWidth - 1 && y == InfoBoxY + 4)
                    {
                        line = '┤';
                        PrintLine = true;
                    }
                    else if (x == 0 || x == Console.WindowWidth - 1)
                    {
                        line = '│';
                        PrintLine = true;
                    }
                    else if (y == InfoBoxY - 1 || y == InfoBoxY + 4)
                    {
                        line = '─';
                        PrintLine = true;
                    }
                    if (PrintLine = true)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(line);
                    }
                }
            }
        }

        private void DrawFileBox()
        {
            char line = ' ';
            int x = FileX - 1;
                for (int y = 0; y <= InfoBoxY-1; y++)
                {
                    bool PrintLine = false;
                    line = ' ';
                    if (y == InfoBoxY-1)
                    {
                        line = '┴';
                        PrintLine = true;
                    }
                    else if (y == 0)
                    {
                        line = '┬';
                        PrintLine = true;
                    }
                    else 
                    {
                        line = '│';
                        PrintLine = true;
                    }

                    if (PrintLine = true)
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(line);
                    }
                }
            
        }

        public void printbox()
        {
            drawBox();
            DrawCommandLine();
            DrawInfobox();
            DrawFileBox();

        }
       /// <summary>
       /// получает список дисков
       /// </summary>
        public void DriveGet()
        {


            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in allDrives)
            {
                Manager.DriveList.Add(drive);
                DrivePrint(DriveList.Count - 1);
            }
            DriveChoose(0);
        }


        /// <summary>
        /// процедура выбора диска
        /// </summary>
        /// <param name="DriveIndex">
        /// индекс диска в списке
        /// </param>
        private void DriveChoose(int DriveIndex)
        {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
            DrivePrint(currentDrive);
    
            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            DrivePrint(DriveIndex);
            currentDrive = DriveIndex;

            directorylist.Clear();
            directory dirInfo = new directory();
            dirInfo.info = new System.IO.DirectoryInfo(DriveList[DriveIndex].Name);
            dirInfo.sub = true;
            dirInfo.deployed = true;
            directorylist.Add(dirInfo);
           // GetDirectoryInfo(0);    
            GetDirectory(DriveList[DriveIndex].Name);
            CurrentDirectory = 0;
            StartDirectoryNumber = 0;
            chooseDirectory(CurrentDirectory);
            StartDirectoryNumber = 0;
            PrintDirectoryList();
 
        }

        /// <summary>
        /// печать "буквы" диска
        /// </summary>
        /// <param name="DriveIndex">
        /// индекс диска в списке
        /// </param>
        public void DrivePrint(int DriveIndex)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(DriveX + (DriveIndex * 4), DriveY);
            Console.Write($"{DriveList[DriveIndex].Name}");
            PrintCommandLine(CommandLine);
        }

        /// <summary>
        /// перемещение по списку дисков (вправо)
        /// </summary>
        private void DriveUp()
        {
            int NewDriveIndex = currentDrive + 1;

            if (NewDriveIndex > DriveList.Count - 1)
            {
                NewDriveIndex = DriveList.Count - 1;
            }
            else
            {
                DriveChoose(NewDriveIndex);

            }
        }

            /// <summary>
            /// перемещение по списку диснов (влево)
            /// </summary>
            private void DriveDown()
        {
            int NewDriveIndex = currentDrive - 1;

            if (NewDriveIndex < 0)
            {
                NewDriveIndex = 0;
            }
            else
            {
                DriveChoose(NewDriveIndex);
            }
        }


        /// <summary>
        /// получаем список каталогов (1 уровень) с заполнением информации о количестве папок и файлов (очень долго)
        /// </summary>
        /// <param name="path">
        /// верхний уровень (диск)
        /// </param>
        private void GetDirectory(string path)
        {
            directory dirInfo = new directory();
            List<string> dirs = new List<string>(Directory.EnumerateDirectories(path));
            foreach (string dir in dirs)
            {
                dirInfo = new directory();
                dirInfo.info = new System.IO.DirectoryInfo(dir);
                dirInfo.level = 1;
                try
                {
                    string[] subDirs = Directory.GetDirectories(dirInfo.info.FullName);
                    if (subDirs.Length > 0)
                    {
                        dirInfo.deployed = false;
                    }
                    else
                    {
                        dirInfo.deployed = true;
                    }
                    dirInfo.noaccess = false;
                }
                catch
                {
                    dirInfo.noaccess = true;
                }
                directorylist.Add(dirInfo);
                //GetDirectoryInfo(directorylist.Count -1);
            }
        }

        /// <summary>
        /// выводим список каталогов
        /// </summary>
        private void PrintDirectoryList()
        {
            //очищаем напечатанный список
            for (int i = 0; i < 20; i++)
            {
                System.Console.BackgroundColor = ConsoleColor.DarkBlue;
                System.Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(DirectoryX-1, DirectoryY + i);
                Console.Write($"{' ', 37}");
            }
            for (int i = StartDirectoryNumber; i < StartDirectoryNumber + Math.Min(directorylist.Count, DirectoryesOnPage); i++)
            {
                printDirectory(i);
            }
        }

        private void printLeveLs(int DirectoryLevel, int Directoryindex, int DirectoryCount)
        {
            directory dirInfo = new directory();
            dirInfo = directorylist[Directoryindex];
            char firstchar = '├';
            if (Directoryindex > 0)
            { 
                if (directorylist[Directoryindex].level > directorylist[Directoryindex - 1].level)
                {
                    firstchar = '├';
                }
             }
            if (Directoryindex < DirectoryCount - 2)
            {
                if (directorylist[Directoryindex].level < directorylist[Directoryindex + 1].level)
                {
                    firstchar = '└';
                }
            }
            else if (Directoryindex == 0)
            {
                firstchar = '┌';
            }
            else if (Directoryindex == (DirectoryCount - 1))
            {
                firstchar = '└';
            }
            else
            {
                firstchar = '├';
            }       

            Console.SetCursorPosition(DirectoryX + DirectoryLevel-2, DirectoryY + Directoryindex - StartDirectoryNumber);
            Console.Write(firstchar);

           // for (int x = 2; x <= DirectoryLevel + 1; x++)
           // {
                Console.SetCursorPosition(DirectoryX + DirectoryLevel - 1, DirectoryY + Directoryindex - StartDirectoryNumber);
                Console.Write('─');
           // }

        }

        /// <summary>
        /// выводим на печать строку каталога
        /// </summary>
        /// <param name="Directoryindex"></param>
        private void printDirectory(int Directoryindex)
        {
            directory dirInfo = new directory();
            dirInfo = directorylist[Directoryindex];
            switch (dirInfo.noaccess)
            {
                case true:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case false:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

            }

            printLeveLs(dirInfo.level, Directoryindex, directorylist.Count);
            string deployed = dirInfo.deployed == true ? "-" : "+";
            Console.SetCursorPosition(DirectoryX + dirInfo.level, DirectoryY + Directoryindex - StartDirectoryNumber);
            Console.Write($"{deployed} {dirInfo.info.Name.Substring(0,Math.Min(dirInfo.info.Name.Length, MaxDirectoryName))}");
        }

        /// <summary>
        /// установка текущего каталога
        /// </summary>
        /// <param name="indexnew">
        /// индекс каталога в списке
        /// </param>
        private void chooseDirectory(int indexnew)
        {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
            printDirectory(CurrentDirectory);

            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            PrintDirectoryList();
            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            CurrentDirectory = indexnew;
            printDirectory(CurrentDirectory);
            try
            {
                getFileList(directorylist[indexnew].info.FullName);
            }
            catch
            {
            }
            PrintInfobox(indexnew);
            PrintCommandLine(CommandLine);
        }

        /// <summary>
        /// "развертывание каталога" - получени вложенных папок
        /// </summary>
        private void deployDirectory()
        {
            if (CommandLine.Length > 0)
            {
                CommandList.Add(CommandLine);
                getCommand(CommandLine.Split(' '));
                CommandLine = "";
                listindex = CommandList.Count - 1;
                oldlistindex = listindex + 1;
                PrintCommandLine(CommandLine);
                return;
            }

            directory dir = directorylist[CurrentDirectory];
            if (dir.deployed == false)
            {
                dir.deployed = true;

                try
                {
                    List<string> dirs = new List<string>(Directory.EnumerateDirectories(directorylist[CurrentDirectory].info.FullName));
                    dir.noaccess = false;
                    int newindex = 1;
                    if (dirs.Count > 0)
                    {
                        dir.deployed = true;
                    }
                    foreach (string Dir in dirs)
                    {
                        directory dirInfo = new directory();
                        dirInfo.info = new System.IO.DirectoryInfo(Dir);
                        dirInfo.level = dir.level + 1;
                        try
                        {
                            List<string> subdirs = new List<string>(Directory.EnumerateDirectories(dirInfo.info.FullName));
                            dirInfo.noaccess = false;
                            if (subdirs.Count > 0)
                            {
                                dirInfo.deployed = false;
                            }
                            else 
                            {
                                dirInfo.deployed = true;
                            }
                        }
                        catch
                        {
                            dirInfo.noaccess = true;
                        }
                        directorylist.Insert(CurrentDirectory + newindex, dirInfo);
                        GetDirectoryInfo(CurrentDirectory + newindex);
                        newindex++;
                    }
                }
                catch
                {
                    dir.noaccess = true;
                }
                directorylist[CurrentDirectory] = dir;
                PrintDirectoryList();
            }
        }

        /// <summary>
        /// перемещенеи по списку каталогов (вверх)
        /// </summary>
        private void DirectoryUp()
        {

            int NewchoosedDirectiryNumber = CurrentDirectory - 1;
            if (NewchoosedDirectiryNumber < 0)
            {
                NewchoosedDirectiryNumber = 0;
            }
            if (NewchoosedDirectiryNumber < StartDirectoryNumber)
            {
                StartDirectoryNumber--;
            }
            chooseDirectory(NewchoosedDirectiryNumber);
            CurrentDirectory = NewchoosedDirectiryNumber;
        }

        /// <summary>
        /// перемещение по списку каталогов (вниз)
        /// </summary>
        private void DirectoryDown()
        {
            int NewchoosedDirectiryNumber = CurrentDirectory + 1;
            if (NewchoosedDirectiryNumber > directorylist.Count - 1)
            {
                NewchoosedDirectiryNumber = directorylist.Count - 1;
            }
            if (NewchoosedDirectiryNumber - StartDirectoryNumber >= DirectoryesOnPage)
            {
                StartDirectoryNumber++;
            }
            chooseDirectory(NewchoosedDirectiryNumber);
            CurrentDirectory = NewchoosedDirectiryNumber;
        }

        /// <summary>
        /// вывод на экран информации и каталоге
        /// </summary>
        /// <param name="Dirindex">
        /// индекс каталога в списке
        /// </param>
        private void PrintInfobox(int Dirindex)
        {
            /*
             * полное имя файла
             * размер        дата создания
             * количество папок  количество файлов
             */

            directory dirInfo = new directory();
            dirInfo = directorylist[Dirindex];

            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;

            //Имя файла
            Console.SetCursorPosition(InfoBoxX, InfoBoxY);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(InfoBoxX, InfoBoxY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("");
            Console.SetCursorPosition(InfoBoxX, InfoBoxY);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{dirInfo.info.FullName,-20}");

            //размер
            Console.SetCursorPosition(InfoBoxX, InfoBoxY+1);
            Console.Write(new string(' ', 10));
            Console.SetCursorPosition(InfoBoxX, InfoBoxY+1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Bytes: ");
            Console.SetCursorPosition(InfoBoxX+14, InfoBoxY+1);
            Console.Write(new string(' ', 20));
            Console.SetCursorPosition(InfoBoxX+14, InfoBoxY+1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            //дата создания
            Console.SetCursorPosition(InfoBoxX+24, InfoBoxY+1);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Дата создания");
            Console.SetCursorPosition(InfoBoxX+44, InfoBoxY+1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{Convert.ToString(dirInfo.info.CreationTime),-20}");
            

            //количество папок
            Console.SetCursorPosition(InfoBoxX, InfoBoxY+2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Папок:");
            Console.SetCursorPosition(InfoBoxX+14, InfoBoxY+2);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{Convert.ToString(dirInfo.DirectoryNumber),-20}");

            //количество файлов
            Console.SetCursorPosition(InfoBoxX+24, InfoBoxY+2);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Файлов:");
            Console.SetCursorPosition(InfoBoxX+44, InfoBoxY+2);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{Convert.ToString(dirInfo.FilesNumber),-20}");
        }

        /// <summary>
        /// получаем инофрмацию о количестве вложенных папок и файлов
        /// </summary>
        /// <param name="Dirindex">
        /// индекс каталога в списке
        /// </param>
        private void GetDirectoryInfo(int Dirindex)
        {
            directory dirInfo = directorylist[Dirindex];
            try
            {
                List<string>dirs = new List<string>(Directory.EnumerateDirectories(dirInfo.info.FullName));
                List<string>files = new List<string>(Directory.EnumerateFiles(dirInfo.info.FullName));
                dirInfo.DirectoryNumber = dirInfo.DirectoryNumber + dirs.Count;
                dirInfo.FilesNumber = dirInfo.FilesNumber + files.Count;
                directorylist[Dirindex] = dirInfo;
                foreach (string dir in dirs)
                {
                    GetSubDirectoryInfo(dir, Dirindex);
                }
            }
            catch 
            {} 
        }

        /// <summary>
        /// получение информации о всех вложениях папок (вся иерархия)
        /// </summary>
        /// <param name="path">
        /// пусть текущего каталога
        /// </param>
        /// <param name="Dirindex">
        /// идекм родительского каталога (для записи информации)
        /// </param>
        private void GetSubDirectoryInfo(string path, int Dirindex)
        {
            directory dirInfo = directorylist[Dirindex];

            try
            {
                List<string> dirs = new List<string>(Directory.EnumerateDirectories(path));
                List<string> files = new List<string>(Directory.EnumerateFiles(path));
                dirInfo.DirectoryNumber = dirInfo.DirectoryNumber + dirs.Count;
                dirInfo.FilesNumber = dirInfo.FilesNumber + files.Count;
                directorylist[Dirindex] = dirInfo;
                foreach (string dir in dirs)
                {
                    GetSubDirectoryInfo(dir, Dirindex);
                }
            }
            catch
            {}

        }

        /// <summary>
        /// вывод на печать командной строки
        /// </summary>
        /// <param name="CommandLine">
        /// текущее значение команды
        /// </param>
        public void PrintCommandLine(string CommandLine)
        {

            //командная строка
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(CommandLineX, CommandLineY);
            Console.Write(new string(' ', Console.WindowWidth-2));
            Console.SetCursorPosition(CommandLineX, CommandLineY);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($">{CommandLine}");
            Console.CursorVisible = true;
        }

        /// <summary>
        /// доьавленеи команды в список истории команд, запуск команды на исполнение
        /// </summary>
        private void runCommand()
        {
            if (CommandLine.Length > 0)
            {
                CommandList.Add(CommandLine);              
                CommandLine.Trim();
                getCommand(CommandLine.Split(' '));
                CommandLine = "";
            }
        }

        /// <summary>
        /// обработка нажатия клавиш
        /// </summary>
        /// <param name="Keypressed"></param>
        public void Keypress(ConsoleKeyInfo Keypressed)
        {
            switch (Keypressed.Key)
            {
                case ConsoleKey.UpArrow when Mode == WindowModes.directory:
                    DirectoryUp();
                    break;
                case ConsoleKey.DownArrow when Mode == WindowModes.directory:
                    DirectoryDown();
                    break;
                case ConsoleKey.LeftArrow when Mode == WindowModes.directory:
                    DriveDown();
                    break;
                case ConsoleKey.RightArrow when Mode == WindowModes.directory:
                    DriveUp();
                    break;
                case ConsoleKey.Enter when Mode == WindowModes.directory:
                    deployDirectory();
                    break;

                case ConsoleKey.UpArrow when Mode == WindowModes.files:
                    FilesUp();
                    break;
                case ConsoleKey.DownArrow when Mode == WindowModes.files:
                    FilesDown();
                    break;

                case ConsoleKey.RightArrow when Mode == WindowModes.files:
                    PageUp();
                    break;
                case ConsoleKey.LeftArrow when Mode == WindowModes.files:
                    PageDown();
                    break;
                case ConsoleKey.Enter when Mode == WindowModes.files:
                    runFile();
                    break;

                case ConsoleKey.UpArrow when Mode == WindowModes.command:
                    RestoreCommandUp();
                    break;
                case ConsoleKey.DownArrow when Mode == WindowModes.command:
                    RestoreCommandDown();
                    break;
                case ConsoleKey.Enter when Mode == WindowModes.command:
                    runCommand();
                    break;
             
                case ConsoleKey.Tab:
                   changeMode();
                    break;
                case ConsoleKey.Backspace:
                    if (CommandLine.Length > 0)
                    {
                        CommandLine = CommandLine.Remove(CommandLine.Length - 1);
                    }
                    PrintCommandLine(CommandLine);
                    break;
                case ConsoleKey.Escape:
                    quitProgram();
                    Environment.Exit(0);
                    break;
                default:
                    CommandLine = CommandLine + Keypressed.KeyChar;
                break;
            }
        }

        /// <summary>
        /// смена режима управления интерфейсом
        /// </summary>
        private void changeMode()
        {
            CommandLine = "";
            PrintCommandLine(CommandLine);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            switch (Mode)
            {
                case WindowModes.directory:
                    Console.SetCursorPosition(DirectoryX, DirectoryY - 1);
                    Console.Write($"     ");

                    if (fileList.Count > 0)
                    {
                        Mode = WindowModes.files;
                        Console.SetCursorPosition(FileX, FileY - 1);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"{directorylist[CurrentDirectory].info.Name}");
                        chooseFile(CurrentFile);
                    }
                    else
                    {
                        Mode = WindowModes.command;
                        Console.SetCursorPosition(CommandLineX, CommandLineY - 1);
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write("     ");
                        PrintCommandLine(CommandLine);
                    }
                    break;
                case WindowModes.files:
                    Console.SetCursorPosition(FileX, FileY - 1);
                    Console.Write($"{directorylist[CurrentDirectory].info.Name}");

                    Mode = WindowModes.command;
                    Console.SetCursorPosition(CommandLineX, CommandLineY - 1);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("     ");
                    PrintCommandLine(CommandLine);
                    break;
                case WindowModes.command:
                    Console.SetCursorPosition(CommandLineX, CommandLineY - 1);
                    Console.Write("     ");

                    Mode = WindowModes.directory;
                    Console.SetCursorPosition(DirectoryX, DirectoryY - 1);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Write("     ");
                    chooseDirectory(CurrentDirectory);
                    break;
            }
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }

        /// <summary>
        /// очистка списка файлов при смене каталога
        /// </summary>
        private void ClearFileList()
        {
            Console.SetCursorPosition(FileX, FileY-1);
            Console.Write($"{new string(' ', Console.WindowWidth - FileX-1)}");

            for (int i = 0; i <= filesOnPAge+1; i++)
            {
                System.Console.SetCursorPosition(FileX, FileY + i);
                Console.Write(new string(' ', Console.WindowWidth - FileX - 2));
            }
        }

        /// <summary>
        /// вывод имени файлв
        /// </summary>
        /// <param name="path">
        /// имя файла
        /// </param>
        /// <param name="index">
        /// индекс файла в списке файлов
        /// </param>
        private void printFile(string path, int index)
        {
            FileInfo FileInfo = new FileInfo(path);
            string FileName = "";
            string extension = FileInfo.Extension;
            long lenght = 0;
            try
            { 
                lenght = FileInfo.Length;
            }
            catch
            {
                lenght = 0;
            }
            switch (extension)
            {
                case ".exe":
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ".txt":
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "":
                    extension = "DIR";
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;

            }

            FileName = FileInfo.Name.Substring(0, Math.Min(FileInfo.Name.Length, MaxFileName));
            string Extension = extension.Substring(0, Math.Min(extension.Length, 5));
            int y = index - (currentPage * filesOnPAge) + 1;
            try
            {
                System.Console.SetCursorPosition(FileX, FileY + y);
            }
            catch
            {
                CurrentFile = 0;
                System.Console.SetCursorPosition(FileX, FileY);
            }
            Console.Write($"{FileName,-25} {Extension,5} {lenght,10} {FileInfo.CreationTime,-20}");

        }

        /// <summary>
        /// вывод списка файлов (по номеру страницы)
        /// </summary>
        /// <param name="pageNumber">
        /// номер страницы
        /// </param>
        private void printFilePage(int pageNumber)
        {
            Console.SetCursorPosition(PageX + pageNumber * 4, PageY);
            Console.Write($"{pageNumber + 1}");
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            ClearFileList();
            
            switch (Mode)
            {
                case WindowModes.files:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            Console.SetCursorPosition(FileX, FileY - 1);
            Console.Write($"{directorylist[CurrentDirectory].info.Name}");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Gray;
            int firstfileNumber = (currentPage * 20);
            for (int i = firstfileNumber; i <= System.Math.Min(firstfileNumber + filesOnPAge, fileList.Count)-1; i++)
            {

                printFile(fileList[i].FullName, i);
            }
            
        }

        /// <summary>
        /// очистка списка файлов
        /// </summary>
        /// <param name="pageNumber">
        /// номер старницы
        /// </param>
        private void ClearFilePage(int pageNumber)
        {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition(PageX + pageNumber * 4, PageY);
            Console.Write($"{pageNumber + 1}");

            int firstfileNumber = (currentPage * 20);
            for (int i = firstfileNumber; i <= System.Math.Min(firstfileNumber + filesOnPAge, fileList.Count - 1); i++)
            {

                printFile(fileList[i].FullName, i);
            }

        }

        /// <summary>
        /// получение списка файлов у выбранного каталога
        /// </summary>
        /// <param name="path"></param>
        private void getFileList(string path)
        {
            fileList.Clear();

            for (int PageNumber = 0; PageNumber <= Math.Min(filePageList.Count, MaximumPages); PageNumber++)
            {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;
               Console.SetCursorPosition(PageX + PageNumber * 4, PageY);
               Console.Write($"  ");
            }
            filePageList.Clear();
            CurrentFile = 0;
            ClearFileList();
            List<string> dirs = new List<string>(Directory.EnumerateDirectories(path));
            foreach (string dir in dirs)
            {
                FileInfo dirinfo = new FileInfo(dir);
                fileList.Add(dirinfo);
            }

            List<string> files = new List<string>(Directory.EnumerateFiles(path));
                   
            foreach (string file in files)
            {
                FileInfo FileInfo = new FileInfo(file);
                fileList.Add(FileInfo);
            }
            int PageNumbers = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(fileList.Count) / Convert.ToDouble(filesOnPAge)));
            for (int PageNumber = 0; PageNumber <= PageNumbers - 1; PageNumber++)
            {
                filePageList.Add(PageNumber);
                System.Console.BackgroundColor = ConsoleColor.DarkBlue;
                System.Console.ForegroundColor = ConsoleColor.Gray;
                if (PageNumbers <= MaximumPages)
                { 
                    Console.SetCursorPosition(PageX + PageNumber * 4, PageY);
                    Console.Write($"{PageNumber + 1}");
                }
            }
            printFilePage(0);
        }

        /// <summary>
        /// выбор страницы (в списке файлов)
        /// </summary>
        /// <param name="indexnew">
        /// индлекс выбранной страницы
        /// </param>
        private void choosePage( int indexnew)

        {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            printFilePage(currentPage);

            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;
            currentPage = indexnew;
            printFilePage(indexnew);
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.Gray;
            if (fileList.Count > 0)
            {
                CurrentFile = (currentPage * filesOnPAge);
            }

            }

        /// <summary>
        /// установка выбранного файла текущим
        /// </summary>
        /// <param name="indexnew">
        /// инлекс файлв в аписке файлов
        /// </param>
        private void chooseFile(int indexnew)
        {
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
  
            printFile(fileList[CurrentFile].FullName, CurrentFile);

            System.Console.BackgroundColor = ConsoleColor.Gray;
            System.Console.ForegroundColor = ConsoleColor.DarkBlue;

            printFile(fileList[indexnew].FullName, indexnew);
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
      
        }

        /// <summary>
        /// перемещение по списку файлов (вверх)
        /// </summary>
        private void FilesUp()
        {
            int NewchoosedFileNumber = CurrentFile - 1;
            if (NewchoosedFileNumber < (currentPage * filesOnPAge))
            {
                NewchoosedFileNumber = (currentPage * filesOnPAge);
            }

            chooseFile(NewchoosedFileNumber);
            CurrentFile = NewchoosedFileNumber;
        }

        /// <summary>
        /// перемещенеи по списку файлов (вниз)
        /// </summary>
        private void FilesDown()
        {
            int NewchoosedFileNumber = CurrentFile + 1;
            if (NewchoosedFileNumber > Math.Min(fileList.Count-1, (currentPage * filesOnPAge) + filesOnPAge-1))
            {
                NewchoosedFileNumber = Math.Min(fileList.Count-1, (currentPage * filesOnPAge) + filesOnPAge-1);
            }

            chooseFile(NewchoosedFileNumber);
            CurrentFile = NewchoosedFileNumber;

        }

        private void runFile()
        {
            if (CommandLine.Length > 0)
            {
                CommandList.Add(CommandLine);
                getCommand(CommandLine.Split(' '));
                CommandLine = "";
                listindex = CommandList.Count - 1;
                oldlistindex = listindex + 1;
                PrintCommandLine(CommandLine);
            }
            Process.Start(fileList[CurrentFile].FullName) ;

        }

        /// <summary>
        /// перемещение по списку страниц (вправо)
        /// </summary>
        private void PageUp()
        {
            int NewCurrentPage = currentPage + 1;
            if (NewCurrentPage > filePageList.Count - 1)
            {
                NewCurrentPage = filePageList.Count - 1;
            }
            choosePage(NewCurrentPage);
  
        }

        /// <summary>
        /// перемещенеи по списку страниц (влево)
        /// </summary>
        private void PageDown()
        {
            int NewCurrentPage = currentPage - 1;
            if (NewCurrentPage < 0)
            {
                NewCurrentPage = 0;
            }

            choosePage(NewCurrentPage);

        }


        /// <summary>
        /// перемещенеи по истории списка комманд (вверх)
        /// </summary>
        private void RestoreCommandUp()
        {
            CommandLine = "";
            PrintCommandLine(CommandLine);
            oldlistindex = oldlistindex - 1;
            if (oldlistindex < 0)
            {
                oldlistindex = 0;
            }
            try
            {
                CommandLine = CommandList[oldlistindex];
            }
            catch
            { }
            PrintCommandLine(CommandLine);
        }

        /// <summary>
        /// перемещенеи по истории списка команд (вниз)
        /// </summary>
        private void RestoreCommandDown()
        {
            CommandLine = "";
            PrintCommandLine(CommandLine);
            oldlistindex = oldlistindex + 1;
            if (oldlistindex > listindex)
            { 
                oldlistindex = listindex;
            }
            try
            { 
                CommandLine = CommandList[oldlistindex];
            }
            catch
            { }    
            PrintCommandLine(CommandLine);
        }

        private void findsubDirectory(string path)
        {
            directory dir = directorylist.Find(search => search.info.FullName == path);
           // if (dir.info == null)
           // {
                DirectoryInfo DirInfo = new DirectoryInfo(path);
                DirectoryPath.Insert(0,DirInfo.Name);
                if (DirInfo.Parent != null)
                {
                    findsubDirectory(DirInfo.Parent.FullName);
                }
           // }
        }

        public void findDirectory(string path)
        {
            DirectoryPath.Clear();
            directory dir = directorylist.Find(search => search.info.FullName == path);
           if (dir.info == null)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(path);
                DirectoryPath.Insert(0, DirInfo.Name);
                try
                {
                    findsubDirectory(DirInfo.Parent.FullName);
                }
                catch
                { }
            }

            DriveInfo Dr = DriveList.Find(search => search.Name.Equals(DirectoryPath[0], StringComparison.OrdinalIgnoreCase));
            System.Console.BackgroundColor = ConsoleColor.DarkBlue;
            System.Console.ForegroundColor = ConsoleColor.White;
            DrivePrint(currentDrive);
            currentDrive = DriveList.IndexOf(Dr);
            DriveChoose(currentDrive);
            int i = 1;
            foreach (string newPath in DirectoryPath)
            {
                i++;
                directory  Dir = directorylist.Find(search => search.info.Name.Equals(newPath, StringComparison.OrdinalIgnoreCase));
                CurrentDirectory = directorylist.IndexOf(Dir);
                chooseDirectory(CurrentDirectory);
                if (i <= DirectoryPath.Count)
                {
                    deployDirectory();
                }

            }
        }

        public void startProgram()
        {
            try
            {
                string TextFile = File.ReadAllText("start.json");
                directory dir = JsonConvert.DeserializeObject<directory>(TextFile);
                findDirectory(dir.info.FullName);
            }
            catch
            {
                /*
                CommandExeptions exeption = new CommandExeptions();
                exeption.ErrorCode = ErrorCodes.WrongPath;
                throw exeption;
                */
            }
        }

            
        private void quitProgram()
        {
            directory dir = directorylist[CurrentDirectory];
            string text = JsonConvert.SerializeObject(dir);
            File.WriteAllText("start.json", text);

        }

        public void getCommand(string[] args)
        {
            CommandLine = "";
            try
            {
                switch (args[0])
                {
                    case "copy":
                        CopyCommand(args);
                        break;
                    case "ls":
                        openCommand(args);
                        break;
                    case "move":
                        MoveCommand(args);
                        break;
                    case "del":
                        DeleteCommand(args);
                        break;
                    default:
                        CommandExeptions exeption = new CommandExeptions();
                        exeption.ErrorCode = ErrorCodes.WrongCommand;
                        throw exeption;
                        break;
                }
            }
            catch (CommandExeptions exeption)
            {
                string filename = $"error.txt";
                File.AppendAllText(filename, $"{Convert.ToString(DateTime.Now.Date)} {exeption.ErrorCode} {exeption.Message} \n" ); 
            }

        }

   

        private void openCommand(string[] args)
        {
            bool DirectoryExist = false;
            try
            {
                DirectoryInfo NewDir = new DirectoryInfo(args[1]);
                DirectoryExist = NewDir.Exists;
                if (DirectoryExist != true)
                {
                    CommandExeptions exeption = new CommandExeptions();
                    exeption.ErrorCode = ErrorCodes.WrongPath;
                    throw exeption;
                }
                else
                {
                    findDirectory(NewDir.FullName);
                }
            }
            catch
            {
                CommandExeptions exeption = new CommandExeptions();
                exeption.ErrorCode = ErrorCodes.WrongCommand;
                throw exeption;
            }
            
            
        }

        private void FileCopy(string source, string destination)
            {
            try
            {
                File.Copy(source, destination);
            }
            catch
            {
                CommandExeptions exeption = new CommandExeptions();
                exeption.ErrorCode = ErrorCodes.WrongPath;
                throw new Exception();
            }
        }

        private void DirectoryCopy(string source, string destination)
        {
            try
            {
               
            }
            catch
            {
                CommandExeptions exeption = new CommandExeptions();
                exeption.ErrorCode = ErrorCodes.WrongPath;
                throw new Exception();
            }
        }
        private void CopyCommand(string[] args)
        {
            if (File.Exists(args[1]))
            {
                FileCopy(args[1], args[2]);
            }
            if (Directory.Exists(args[1]))
            {
                DirectoryCopy(args[1], args[2]);
            }
        }

        private void MoveCommand(string[] args)
        {
     
        }

        private void DeleteCommand(string[] args)
        {

        }

    }
}
