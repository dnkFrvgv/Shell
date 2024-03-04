using System;
using System.IO;
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
                ShowDirectories(_currentDirectory);
                ShowFiles(_currentDirectory);
            }
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
