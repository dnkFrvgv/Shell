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

                return;
            }

            /*&& commands[1].StartsWith(".")*/

            if (IsPathAbsolute(commands))
            {
                if (!IsPathNormilised(commands[1]))
                {
                    commands[1] = NormalisePath(commands[1]);
                }

                ShowDirCommand(commands[1]);
                return;
            }
            if(IsPathRelative(commands))
            {
                // Todo
                // join with current dir
                return;
            }
            LogError("error");
            return;

        }

        private string NormalisePath(string v)
        {
            string[] pathDirectories = v.Split('/');

            if (v.ToLower().StartsWith("c:"))
            {

                pathDirectories = pathDirectories.Skip(1).ToArray();

                return GetNormalisedPath(pathDirectories);

            }

            return GetNormalisedPath(pathDirectories);
        }

        private bool IsPathAbsolute(string[] commands)
        {
            return commands.Length > 1 && commands[1].ToLower().StartsWith("c:") || commands[1].StartsWith("/") || commands[1].StartsWith("\\");
        }

        private bool IsPathRelative(string[] commands)
        {
            return commands.Length > 1 && commands[1].StartsWith(".") || commands[1].StartsWith("..");
        }

        private void ShowDirCommand(string v)
        {
            if (!Directory.Exists(v))
            {
                LogError($"Directory {v} does not exist.");
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
            return "\\" + Path.Combine(path);
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
