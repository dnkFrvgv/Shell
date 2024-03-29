using System;
using System.IO;
using System.Linq;
using System.Security.Principal;

namespace Shell
{
    internal class Shell
    {
        private string _currentDirectory;
        private string _currentUser;

        public Shell() 
        { 
            _currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            _currentUser = Environment.UserName;
        }
        public void RunLoop()
        {
            while (true)
            {
                Console.Write("User:" + _currentUser + " >> ");
                var line = Console.ReadLine();

                if (line != null)
                {
                    try
                    {
                        string[] commands = line.Split(' ');
                        RunBuidInCommand(commands);

                    }
                    catch (Exception ex){ 
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        private void RunBuidInCommand(string[] commands)
        {
            switch (commands[0].ToLower())
            {
                case "dir":
                    ExecuteDirCommand(commands);
                    break;

                case "cd":
                    break;

                case "echo":
                    break;

                case "run":
                    break;

                case "help":
                    break;

                default:
                    break;

            }

        }

        private void ExecuteDirCommand(string[] commands)
        {
            if(commands.Length == 1)
            {
                Console.WriteLine($"\nDirectory: {_currentDirectory} \n");

                ShowDirectories(_currentDirectory);
                ShowFiles(_currentDirectory);

                Console.WriteLine("\n");
            }

            /*&& commands[1].StartsWith(".")*/

            if (commands.Length > 1 && commands[1].ToLower().StartsWith("c:") || commands[1].ToLower().StartsWith("/"))
            {
                ShowDirCommand(commands[1]);
            }
            if(commands.Length > 1 && commands[1].ToLower().StartsWith("./"))
            {
                // Todo


            }
            else
            {
                LogError("error");
            }

        }

        private void ShowDirCommand(string v)
        {
            if (!Directory.Exists(v))
            {
                LogError($"Directory {v} does not exist.");
            }

            if (!IsPathNormilised(v))
            {

                string NormalisedPath;
                string[] pathDirectories = v.Split('/');

                if (v.ToLower().StartsWith("c:"))
                {

                    pathDirectories = pathDirectories.Skip(1).ToArray();

                    NormalisedPath = GetNormalisedPath(pathDirectories);

                }

                else
                {
                    NormalisedPath = GetNormalisedPath(pathDirectories);
                }


                Console.WriteLine($"\nDirectory: {NormalisedPath} \n");

                ShowDirectories(NormalisedPath);
                ShowFiles(NormalisedPath);

                Console.WriteLine("\n");

                return;
            }

            Console.WriteLine($"\nDirectory: {v} \n");

            ShowDirectories(v);
            ShowFiles(v);

            Console.WriteLine("\n");
        }

        private bool IsPathNormilised(string v)
        {
            return v.Contains('\\') ? true : false;
        }

        private string GetNormalisedPath(string[] path)
        {
            return "C:\\" + Path.Combine(path);
        }

        private void LogError(string error)
        {
            Console.WriteLine(error);
        }

        private void ShowFiles(string pathToDirectory)
        {
            var files = Directory.GetFiles(pathToDirectory, "*.wav");

            foreach (var file in files)
            {
                Console.WriteLine($"{GetRelativePath(file)}");
            }
        }

        private void ShowDirectories(string pathToDirectory)
        {
            var subdirectories = Directory.GetDirectories(pathToDirectory);

            foreach (var directory in subdirectories)
            {
                Console.WriteLine($"{GetRelativePath(directory)}");
            }
        }

        public string GetRelativePath(string fullPath)
        {
            return fullPath.Substring(fullPath.LastIndexOf('\\') + 1);
        }
    }
}
