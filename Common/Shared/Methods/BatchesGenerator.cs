
namespace Shared.Methods
{
    public class BatchesGenerator
    {
        /// <summary>
        /// Method which generates a batches using entity generation function
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="batchSize"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> GenerateBatches<T>(int total, int batchSize, Func<T> func)
            where T : class
        {
            for (int i = 0; i < total; i += batchSize)
            {
                var batch = new List<T>(batchSize);

                for (int j = 0; j < batchSize; j++)
                {
                    batch.Add(func());
                }

                yield return batch;
            }
        }

        /*/// <summary>
        /// Asynchronous Method overload
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="total"></param>
        /// <param name="batchSize"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async IAsyncEnumerable<List<T>> GenerateBatchesAsync<T>(int total, int batchSize, Func<Task<T>> func)
            where T : class
        {
            for (int i = 0; i < total; i += batchSize)
            {
                var batch = new List<T>(batchSize);

                for (int j = 0; j < batchSize; j++)
                {
                    var entity = await func();
                    batch.Add(entity);
                }

                yield return batch;
            }
        }*/
    }
}
