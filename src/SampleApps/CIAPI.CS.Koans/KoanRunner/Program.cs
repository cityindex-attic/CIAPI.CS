using System;
using System.Linq;
using System.Reflection;
using Common.Logging;

namespace CIAPI.CS.Koans.KoanRunner
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            try
            {
                var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
                foreach (var type in assemblyTypes.Where(type => HasAttribute(type, typeof(KoanCategoryAttribute))))
                {
                    WriteGoodLine("Learning about {0}", type.Name);
                    var koanCategory = Activator.CreateInstance(type);
                    foreach (var method in type.GetMethods().Where(method => HasAttribute(method, typeof(KoanAttribute))))
                    {
                        var koan = (Action)Delegate.CreateDelegate(typeof(Action), koanCategory, method);
                        koan();
                        WriteGoodLine("\t+1 Your karma has increased by learning {0}", method.Name);
                    }
                    WriteGoodLine("\n=====================================\n");
                }

                WriteGoodLine("\n\nYou have reached enlightenment!");
            }
            catch (FailedToReachEnlightenmentException e)
            {
                Console.WriteLine();
                WriteBadLine(e.Message);
                var koanLocation = e.StackTrace.Split('\n')[1]; //The second line contains the info we want
                WriteBadLine(koanLocation);
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }

            Console.ReadKey();
        }

        private static void WriteGoodLine(string text, params string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(String.Format(text, args));
            Console.ResetColor();
        }
        private static void WriteBadLine(string text, params string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(String.Format(text, args));
            Console.ResetColor();
        }

        private static bool HasAttribute(Type type, Type attributeType)
        {
            return type.GetCustomAttributes(false).Count(x => x.GetType().FullName == attributeType.FullName)!=0;
        }

        private static bool HasAttribute(MethodInfo method, Type attributeType)
        {
            return method.GetCustomAttributes(false).Count(x => x.GetType().FullName == attributeType.FullName) != 0;
        }
    }
}
