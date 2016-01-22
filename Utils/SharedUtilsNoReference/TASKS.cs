using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedUtilsNoReference
{
    public static class TASKS
    {
        public static T RetryOnFault<T>(Func<T> function, int MaxTries)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    return function();
                }
                catch
                {
                    if (i == MaxTries - 1)
                        throw;
                }
            }
            return default(T);
        }


        /// <summary>
        /// string page = await RetryOnFaultAsync( ()=>DownloadStringAsync(url), 3, TimeSpan.FromSeconds(30));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <param name="MaxTries"></param>
        /// <param name="WaitAfterExc">TimeSpan.FromSeconds(30)</param>
        /// <returns></returns>
        public static async Task<T> RetryOnFaultAsync<T>(Func<Task<T>> function, int MaxTries, TimeSpan? WaitAfterExc = null)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    return await function().ConfigureAwait(false);
                }
                catch
                {
                    if (i == MaxTries - 1)
                        throw;

                    if (WaitAfterExc.HasValue && WaitAfterExc.Value != TimeSpan.Zero)
                        await Task.Delay(WaitAfterExc.Value).ConfigureAwait(false);
                }
            }
            return default(T);
        }
        // string page = await RetryOnFaultAsync( ()=>DownloadStringAsync(url), 3);
        public static async Task<T> RetryOnFaultAsync<T>(Func<Task<T>> function, int MaxTries)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    return await function().ConfigureAwait(false);
                }
                catch
                {
                    if (i == MaxTries - 1)
                        throw;
                }
            }
            return default(T);
        }

        /*
           Lancia tutti i metodi, ritorna il primo metodo e annulla tutti gli altri.
         * 
         * double currentPrice = await GetFirstResult( ct => MetodoAsync("param", ct),
         *                                             ct => Metodo2Async("param", ct),
         *                                             ct => Metodo3Async("param", ct));
        */
        public static async Task<T> GetFirstResult<T>(params Func<CancellationToken, Task<T>>[] functions)
        {
            var cts = new CancellationTokenSource();
            var tasks = (from function in functions
                         select function(cts.Token))
                         .ToArray();
            var completed = await Task.WhenAny(tasks).ConfigureAwait(false);
            cts.Cancel();

            /*logging
            foreach (var task in tasks)
            {
                var ignored = task.ContinueWith(t => Log(t), TaskContinuationOptions.OnlyOnFaulted);
            }*/

            return completed.Result; //è sbagliato!
        }
    }
}
