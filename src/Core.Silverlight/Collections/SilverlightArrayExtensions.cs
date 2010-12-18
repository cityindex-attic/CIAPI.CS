
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIAPI.Core.Collections
{
    /// <summary>
    /// http://michaelsync.net/2009/03/22/silverlight-3-array-helper
    /// </summary>
    public static class SilverlightArrayExtensions
    {
        public static List<T> ConvertAll<TInput, T>(this IEnumerable<TInput> array, Converter<TInput, T> converter)
        {
            if (array == null)
                throw new ArgumentException();

            return (from item in array select converter(item)).ToList();
        }

        public static bool Exists<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            return array.Any(item => match(item));
        }

        public static T Find<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            return (from item in array where match(item) select item).FirstOrDefault();
        }

        public static List<T> FindAll<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            return (from item in array where match(item) select item).ToList();
        }


        public static int RemoveAll<T>(this List<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            IEnumerable<T> result = (from item in array where match(item) select item);
            foreach (T item in result)
            {
                array.Remove(item);
            }
            return result.Count();
        }

        public static int FindIndex<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            try
            {
                var v = array
                    .Select((item, index) => new {item, position = index})
                    .Where(x => match(x.item)).FirstOrDefault();
                return v.position;
            }
            catch
            {
                return -1;
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> array, int startIndex, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            if (startIndex >= array.Count())
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                var v = array
                    .Select((item, index) => new {item, position = index})
                    .Where((x, index) => match(x.item) && (index >= startIndex)).FirstOrDefault();

                return v.position;
            }
            catch
            {
                return -1;
            }
        }

        public static int FindIndex<T>(this IEnumerable<T> array, int startIndex, int count, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            if (startIndex >= array.Count())
            {
                throw new ArgumentOutOfRangeException();
            }

            if (startIndex < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                var v = array
                    .Where((obj, index) => (index >= startIndex) && (index <= count))
                    .Select((item, index) => new {item, position = index})
                    .Where((x, index) => match(x.item) && (index >= startIndex) && (index <= count)).FirstOrDefault();

                return v.position;
            }
            catch
            {
                return -1;
            }
        }

        public static T FindLast<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            return array
                .Last(item => match(item));
        }

        public static int FindLastIndex<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();
            try
            {
                var v = array
                    .Select((item, index) => new {item, position = index})
                    .Last(x => match(x.item));

                return v.position;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static int FindLastIndex<T>(this IEnumerable<T> array, int startIndex, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            if (startIndex >= array.Count())
            {
                throw new ArgumentOutOfRangeException();
            }
            try
            {
                var v = array
                    .Where((obj, index) => index >= startIndex)
                    .Select((item, index) => new {item, position = index})
                    .Last(x => match(x.item));
                return v.position;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static int FindLastIndex<T>(this IEnumerable<T> array, int startIndex, int count, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            if (startIndex >= array.Count())
            {
                throw new ArgumentOutOfRangeException();
            }

            if (startIndex < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            try
            {
                var v = array
                    .Where((obj, index) => (index >= startIndex) && (index <= count))
                    .Select((item, index) => new {item, position = index})
                    .Last(x => match(x.item));
                return v.position;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        public static bool TrueForAll<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            if (array == null)
                throw new ArgumentException();

            return array.Any(item => match(item));
        }
    }
}
