using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BestConsole
{
    class Program
    {
        private static readonly Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();

        static void Main(string[] args)
        {
            LoadPlugins();

            while (true)
            {
                Console.Write("Enter command: ");
                string input = Console.ReadLine();
                if (input.ToLower() == "exit")
                    break;

                string[] commandArgs = input.Split(' ');

                if (commandArgs.Length == 0)
                {
                    Console.WriteLine("No command entered.");
                    continue;
                }

                string command = commandArgs[0].ToLower();
                switch (command)
                {
                    case "help":
                        DisplayHelp();
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                    default:
                        ExecutePlugin(command, commandArgs);
                        break;
                }
            }
        }

        private static void LoadPlugins()
        {
            string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
                Console.WriteLine("Plugins directory created.");
            }

            foreach (var file in Directory.GetFiles(pluginsPath, "*.dll"))
            {
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                foreach (var type in types)
                {
                    var plugin = (IPlugin)Activator.CreateInstance(type);
                    plugins[plugin.CommandName.ToLower()] = plugin;
                }
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("- help");
            Console.WriteLine("- clear");
            Console.WriteLine("- exit");
            foreach (var command in plugins.Keys)
            {
                Console.WriteLine($"- {command}");
            }
        }

        private static void ExecutePlugin(string command, string[] commandArgs)
        {
            if (plugins.TryGetValue(command, out var plugin))
            {
                plugin.Run(commandArgs);
            }
            else
            {
                Console.WriteLine($"No plugin found for command: {command}");
            }
        }
    }
}
