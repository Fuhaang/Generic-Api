using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Helpers
{
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        /// convert the given source to a list
        /// </summary>
        /// <typeparam name="T">Type of List</typeparam>
        /// <param name="source">source to convert</param>
        /// <returns></returns>
        public static Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return ExecuteAsync();

            async Task<List<T>> ExecuteAsync()
            {
                var list = new List<T>();

                await foreach (var element in source)
                {
                    list.Add(element);
                }

                return list;
            }
        }
    }
}
