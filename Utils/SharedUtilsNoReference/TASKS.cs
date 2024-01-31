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
        public static T RetryOnFault<T>(Func<T> function, int MaxTries, TimeSpan? WaitAfterExc = null)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    return function();
                }
                catch(Exception)
                {
                    if (i == MaxTries - 1)
                        throw;

                    if (WaitAfterExc.HasValue && WaitAfterExc.Value != TimeSpan.Zero)
                        Task.Delay(WaitAfterExc.Value).Wait();
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
        public static async Task<T> RetryOnFaultAsync<T>(Func<Task<T>> function, int MaxTries = 3, TimeSpan? WaitAfterExc = null)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    return await function().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    if (i == MaxTries - 1)
                        throw;

                    if (WaitAfterExc.HasValue && WaitAfterExc.Value != TimeSpan.Zero)
                        await Task.Delay(WaitAfterExc.Value).ConfigureAwait(false);
                }
            }
            return default(T);
        }

        /// <summary>
        /// version without return value
        /// await RetryOnFaultAsync(async ()=> await DownloadStringAsync(url), 3, TimeSpan.FromSeconds(30));
        /// </summary>
        /// <param name="function"></param>
        /// <param name="MaxTries"></param>
        /// <param name="WaitAfterExc">TimeSpan.FromSeconds(30)</param>
        /// <returns></returns>
        public static async Task RetryOnFaultAsync(Func<Task> function, int MaxTries = 3, TimeSpan? WaitAfterExc = null)
        {
            for (int i = 0; i < MaxTries; i++)
            {
                try
                {
                    await function().ConfigureAwait(false);
                    return;
                }
                catch (Exception)
                {
                    if (i == MaxTries - 1)
                        throw;

                    if (WaitAfterExc.HasValue && WaitAfterExc.Value != TimeSpan.Zero)
                        await Task.Delay(WaitAfterExc.Value).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Lancia tutti i metodi, ritorna il primo metodo e annulla tutti gli altri.
        /// double currentPrice = await GetFirstResult(ct => MetodoAsync("param", ct), ct => Metodo2Async("param", ct), ct => Metodo3Async("param", ct));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functions"></param>
        /// <returns></returns>
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

            return completed.Result; //todo: è sbagliato!
        }
    }
}
