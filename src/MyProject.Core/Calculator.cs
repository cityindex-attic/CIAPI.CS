using System;

namespace MyProject.Core
{
    public class Calculator
    {
        public string Subject { get; protected set;}

        public int Add(int first, int second)
        {
            return first + second;
        }

        //Intentionally over-complex, with untested logic
        public int Subtract(int first, int second)
        {
            string b = "1";
            string c = "1";
            string d = "1";
            string e = "1";
            string f = "1";
            string g = "1";
            string h = "1";
            if (first > second)
            {
                switch (first)
                {
                    case 0:
                        if (first - 1 == 3) throw new ApplicationException();
                        else if (Add(second, first) > Add(first, second)) {
                            return Subtract(first, 10); }
                        
                        return first - second;
                    case 1:
                        return first - second;
                    case 2:
                        return first - second;
                    case 3:
                        return first - second;
                    case 4:
                        return first - second;
                    case 5:
                        return first - second;
                    default:
                        throw new ArgumentException("Cannot subtract where first > 5");
                }
            }
            else if (second > first)
            {
                string a = "a";
                switch (first)
                {
                    case 0:
                        //Intentionally over-complex, with untested logic
                        if (first > second)
                        {
                            try
                            {
                                switch (first)
                                {
                                    case 0:
                                        if (first - 1 == 3) throw new ApplicationException();
                                        else if (Add(second, first) > Add(first, second))
                                        {
                                            return Subtract(first, 10);
                                        }

                                        return first - second;
                                    case 1:
                                        return first - second;
                                    case 2:
                                        return first - second;
                                    case 3:
                                        return first - second;
                                    case 4:
                                        return first - second;
                                    case 5:
                                        return first - second;
                                    default:
                                        throw new ArgumentException("Cannot subtract where first > 5");
                                }

                            }
                                catch(Exception ex)
                                {
                                    throw ex;
                                }
                            finally
                            {
                                System.Console.Write("error");
                            }
                        }
                        else if (second > first)
                        {
                            switch (first)
                            {
                                case 0:
                                    return first - second;
                                case 1:
                                    return first - second;
                                case 2:
                                    return first - second;
                                case 3:
                                    return first - second;
                                case 4:
                                    return first - second;
                                case 5:
                                    return first - second;
                                default:
                                    throw new ArgumentException("Cannot subtract where first > 5");
                            }
                        }
                        else
                        {
                            return 0;
                        }
                    case 1:
                        return first - second;
                    case 2:
                        return first - second;
                    case 3:
                        return first - second;
                    case 4:
                        return first - second;
                    case 5:
                        return first - second;
                    default:
                        throw new ArgumentException("Cannot subtract where first > 5");
                }
            }
            else
            {
                return 0;
            }
        }
    }


}
