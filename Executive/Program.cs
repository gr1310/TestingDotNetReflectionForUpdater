using System;
using System.Reflection;
using Module1;

namespace Executive
{
    public class Program
    {
        static void PrintTypes(string path)
        {
            Assembly assembly = Assembly.LoadFrom(path);
            Console.WriteLine($"Assembly: {assembly.FullName}");
            Console.WriteLine("=========================");

            Type analyzerInterface = typeof(IAnalyzer);
            Type[] types = assembly.GetTypes();

            foreach (Type type in types)
            {
                if (analyzerInterface.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                {
                    Console.WriteLine(type.FullName);
                    MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    foreach (MethodInfo method in methods)
                    {
                        Console.WriteLine($"\t{method.Name}");
                        ParameterInfo[] parameters = method.GetParameters();
                        foreach (ParameterInfo parameter in parameters)
                        {
                            Console.WriteLine($"\t\tParameter={parameter.Name}");
                            Console.WriteLine($"\t\t\tType={parameter.ParameterType}");
                            Console.WriteLine($"\t\t\tPosition={parameter.Position}");
                            Console.WriteLine($"\t\t\tOptional={parameter.IsOptional}");
                        }
                    }
                }
            }
        }

        static bool IsDLLFile(string path)
        {
            return Path.GetExtension(path).Equals(".dll", StringComparison.OrdinalIgnoreCase);
        }

        static void Main(string[] args)
        {
            string folder = @"C:\temp";
            try
            {
                string[] files = Directory.GetFiles(folder);

                foreach (string file in files)
                {
                    if (IsDLLFile(file))
                    {
                        try
                        {
                            PrintTypes(file);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
