using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CIAPI.Streaming;

using Salient.ReflectiveLoggingAdapter;


namespace CIAPI.CS.Koans.KoanRunner
{
    class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Program));
        static void Main(string[] args)
        {
            LogManager.CreateInnerLogger = (logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
                => new SimpleTraceAppender(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat);

            

            PrepareConsole();
            object koanCategory = null;
            try
            {
                var assemblyTypes = Assembly.GetExecutingAssembly().GetTypes();
                foreach (var type in assemblyTypes.Where(IsRunnableKoanCategory).OrderBy(t => t.GetCustomAttributes(typeof(KoanCategoryAttribute), false).Cast<KoanCategoryAttribute>().First().Order))
                {
                    WriteGoodLine("Learning about {0}", Spacify(type.Name));
                    koanCategory = Activator.CreateInstance(type);
                    foreach (var method in type.GetMethods().Where(IsRunnableKoan).OrderBy(t => t.GetCustomAttributes(typeof(KoanAttribute), false).Cast<KoanAttribute>().First().Order))
                    {
                        WriteGoodLine("{0}...", Spacify(method.Name));
                        var koan = (Action)Delegate.CreateDelegate(typeof(Action), koanCategory, method);
                        koan();
                        WriteGoodLine(" +1 Your karma has increased by learning about {0}", Spacify(method.Name));
                    }

                    WriteGoodLine("\n=====================================\n");
                }

                WriteGoodLine("\n\nYou have reached enlightenment!");
                new AnimatedFireworks().Animate();
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
            finally
            {
                if (koanCategory is IDisposable) ((IDisposable)koanCategory).Dispose();
            }

            Console.ReadKey();
        }

        private static void PrepareConsole()
        {
            Console.BufferWidth = 250;
            Console.Title = "CIAPI.CS.Koans > searching for CIAPI enlightenment";
        }

        private static string Spacify(string pascalCaseString)
        {
            return Regex.Replace(pascalCaseString, "([A-Z]{1,1}|[0-9]+)", " $1").TrimStart();
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

        private static bool IsRunnableKoanCategory(Type type)
        {

            return type.GetCustomAttributes(false).Count(attribute =>
                    attribute.GetType().FullName == typeof(KoanCategoryAttribute).FullName
                 && !((KoanCategoryAttribute)attribute).Ignore
                ) != 0;
        }

        private static bool IsRunnableKoan(MethodInfo method)
        {
            return method.GetCustomAttributes(false).Count(attribute =>
                   attribute.GetType().FullName == typeof(KoanAttribute).FullName
                && !((KoanAttribute)attribute).Ignore
                ) != 0;
        }
    }
}
