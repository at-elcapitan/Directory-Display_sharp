using System;
using System.IO;
using System.Linq;

namespace DirectoryDisplay_sharp
{
    class MainClass
    {
        private static string relative_path = "";
        private static string absolute_path = "";
        private static bool sizeFlag;
        private static bool allFlag;

        private static void SetRelativePath(string arg, bool absolute = false)
        {
            string[] path_arr;

            arg = arg.TrimEnd('\\');
            path_arr = arg.Split('\\');

            if (absolute & arg.Length > 1)
            {
                relative_path = "\\" + path_arr[path_arr.Length - 1] + "\\";
            }

            relative_path = path_arr[path_arr.Length - 1] + "\\";
        }

        public static void Main(string[] args)
        {

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--size":
                        sizeFlag = true;
                        break;

                    case "--all":
                        allFlag = true;
                        break;

                    default:
                        if (!arg.StartsWith("-", StringComparison.Ordinal))
                        {
                            SetRelativePath(arg, arg.StartsWith("\\", 
                                            StringComparison.Ordinal));
                            absolute_path = Path.GetFullPath(arg);
                            break;
                        }

                        for (int i = 0; i < arg.Length; ++i)
                        {
                           switch (arg[i])
                            {
                                case 's':
                                    sizeFlag = true;
                                    break;

                                case 'a':
                                    allFlag = true;
                                    break;
                            }
                        }
                        break;
                }
            }

            if (absolute_path == "")
            {
                SetRelativePath(Environment.CurrentDirectory);
                absolute_path = Environment.CurrentDirectory;
            }
            else
            {
                if (!Directory.Exists(absolute_path))
                {
                    Console.WriteLine("wds: {0} - no such directory", 
                                      relative_path);
                    return;
                }
            }

            Console.WriteLine(relative_path);
            var files = Directory.GetFiles(absolute_path)
                        .Select(Path.GetFileName)
                        .ToArray<string>();
            string[] dirs = Directory.GetDirectories(absolute_path);

            Array.Sort(files);
            Array.Sort(dirs);

            foreach (string dir in dirs)
            {
                string fdir = dir.Replace(absolute_path, "");
                var dinfo = new DirectoryInfo(fdir);

                /*if (dinfo.Attributes.HasFlag(FileAttributes.Hidden) && !allFlag)
                {
                    continue;
                }*/

                if (!fdir.StartsWith("\\", StringComparison.Ordinal))
                {
                    fdir = fdir.Insert(0, "\\");
                }

                Console.ForegroundColor = ConsoleColor.Cyan;

                if (sizeFlag) Console.Write("DIR\t-\t- ");
                else Console.Write("DIR\t- ");

                /*if (dinfo.Attributes.HasFlag(FileAttributes.Hidden) && allFlag)
                { 
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine(fdir);
                    continue;
                }*/

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(fdir);
            }

            foreach (string file in files)
            {
                var finfo = new FileInfo(file);
                long size = new FileInfo(file).Length;

                if (finfo.Attributes.HasFlag(FileAttributes.Hidden) && !allFlag)
                {
                    continue;
                }

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("FILE");

                if (sizeFlag)
                {
                    string type = "B";

                    if (size > 1000)
                    {
                        size /= 1024;
                        type = "KB";
                    }
                    else if (size > 1000000)
                    {
                        size /= 1024;
                        type = "MB";
                    }
                    else if (size > 1000000000)
                    {
                        size /= 1024;
                        type = "GB";
                    }

                    if (sizeFlag)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("{0,5} {1}", size, type);
                    }
                }
                
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t- ");

                if (finfo.Attributes.HasFlag(FileAttributes.Hidden) && allFlag)
                    Console.ForegroundColor = ConsoleColor.Gray;

                if (file.EndsWith(".exe") || file.EndsWith(".com") || file.EndsWith(".bat"))
                {
                    Console.Write("*");

                    if (Console.ForegroundColor == ConsoleColor.Gray)
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.WriteLine(file);
            }
        }
    }
}
