using System;
using System.IO;
using System.Linq;

namespace DirectoryDisplay_sharp
{
    class MainClass
    {
        private static string relative_path = "";
        private static string absolute_path = "";

        private static void SetRelativePath(string arg, bool absolute = false)
        {
            string[] path_arr;

            arg = arg.TrimEnd('/');
            path_arr = arg.Split('/');

            if (absolute & arg.Length > 1)
            {
                relative_path = "/" + path_arr[path_arr.Length - 1] + "/";
            }

            relative_path = path_arr[path_arr.Length - 1] + "/";
        }

        public static void Main(string[] args)
        {
            //  0   0  0
            // SIZ ACC ALL 
            int flags = 0;

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--size":
                        flags = flags | 0b100;
                        break;

                    case "--access":
                        flags = flags | 0b010;
                        break;

                    case "--all":
                        flags = flags | 0b001;
                        break;

                    default:
                        if (!arg.StartsWith("-", StringComparison.Ordinal))
                        {
                            SetRelativePath(arg, arg.StartsWith("/", 
                                            StringComparison.Ordinal));
                            absolute_path = Path.GetFullPath(arg);
                            break;
                        }

                        for (int i = 0; i < arg.Length; ++i)
                        {
                           switch (arg[i])
                            {
                                case 's':
                                    flags = flags | 0b100;
                                    break;

                                case 'A':
                                    flags = flags | 0b010;
                                    break;

                                case 'a':
                                    flags = flags | 0b001;
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

            Console.WriteLine("{0}\n", relative_path);
            var files = Directory.GetFiles(absolute_path)
                        .Select(Path.GetFileName);
            string[] dirs = Directory.GetDirectories(absolute_path);

            foreach (string dir in dirs)
            {
                string fdir = dir.Replace(absolute_path, "");
                if (!fdir.StartsWith("/", StringComparison.Ordinal))
                {
                    fdir = fdir.Insert(0, "/");
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("DIR\t");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(fdir);
            }

            foreach (string file in files)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("FILE\t");

                if (file.Contains(".exe") || file.Contains(".com") || file.Contains(".bat"))
                {
                    Console.Write("*");
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine(file);
            }
        }
    }
}
