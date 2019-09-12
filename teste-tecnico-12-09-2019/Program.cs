using controller;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace teste
{
    class Program
    {
        // lists to keep track of the contents
        public static List<Vendedor> vendedorList = new List<Vendedor>();
        public static List<Cliente> clienteList = new List<Cliente>();
        public static List<Venda> vendaList = new List<Venda>();

        // constants
        public static string inputpath = @"c:\data\in";
        public static string outputpath = @"c:\data\out";
        public static char separator = 'ç';

        // worker
        public static DirectoryWorker worker;

        // main scope
        static void Main(string[] args)
        {
            // starts watching for changes
            Watch(inputpath);
        }

        /// <summary>
        /// dir watcher 
        /// source: https://docs.microsoft.com/en-us/dotnet/api/system.io.filesystemwatcher?view=netframework-4.8
        /// </summary>
        private static void Watch(string inputPath)
        {
            // Create a new FileSystemWatcher and set its properties.
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = inputPath;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                     | NotifyFilters.LastWrite
                                     | NotifyFilters.FileName
                                     | NotifyFilters.DirectoryName;

                // Only watch text files.
                watcher.Filter = "*.txt";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Monitoring directory {0} for *.txt files.", inputPath);
                Console.WriteLine("Press 'q' to quit.");
                while (Console.Read() != 'q') ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

            // inits worker
            worker = worker = new DirectoryWorker(new DirectoryInfo(inputpath), new DirectoryInfo(outputpath), separator);

            //runs the worker
            worker.defaultRoutine();
        }
    }
}
