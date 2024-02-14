using System;
using System.IO;
using System.Collections;

namespace DirectoryDisplay_sharp
{
    class MainClass
    {
        private static string SetPath(string arg, bool absolute = false)
        {
            string path;
            string[] path_arr;

            arg = arg.TrimEnd(S'/');
            path_arr = arg.Split('/');

            if (absolute & arg.Length > 1)
            {
                path = "/" + path_arr[path_arr.Length - 1] + "/";
                return path;
            }

            path = path_arr[path_arr.Length - 1] + "/";
            return path;
        }

        public static void Main(string[] args)
        {
            //  0   0   0   0
            // SIZ ACC NOC ALL 
            int flags = 0;
            string path = "";

            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--size":
                        flags = flags | 0b1000;
                        break;

                    case "--access":
                        flags = flags | 0b0100;
                        break;

                    case "--no-color":
                        flags = flags | 0b0010;
                        break;

                    case "--all":
                        flags = flags | 0b0001;
                        break;

                    default:
                        if (!arg.StartsWith("-", StringComparison.Ordinal))
                        {
                            path = SetPath(arg, arg.StartsWith("/", 
                                            StringComparison.Ordinal));
                            break;
                        }

                        for (int i = 0; i < arg.Length; ++i)
                        {
                           switch (arg[i])
                            {
                                case 's':
                                    flags = flags | 0b1000;
                                    break;

                                case 'A':
                                    flags = flags | 0b0100;
                                    break;

                                case 'n':
                                    flags = flags | 0b0010;
                                    break;

                                case 'a':
                                    flags = flags | 0b0001;
                                    break;
                            }
                        }
                        break;
                }
            }

            if (path == "")
            {
                path = SetPath(Environment.CurrentDirectory);
            }
            else
            {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("wds: {0} - no such directory", path);
                    return;
                }
            }
        }
    }
}
