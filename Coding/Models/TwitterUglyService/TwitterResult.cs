using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Coding.Models.TwitterUglyService
{
    public class TwitterResult
    {
        const int tweetOverflow = 100;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        static readonly object SyncRoot = new object();
        public List<ServiceClient.Tweet> Result { get; protected set; } = new List<ServiceClient.Tweet>();

        public bool IsIncomplete { get { return tweetOverflow == Result.Count; } }



        public async Task<TwitterResult> CreateFetchNext(Func<DateTime, DateTime, Task<TwitterResult>> fetchNextCommand)
        {
            TwitterResult t = null;
            DateTime newStartDate;
            DateTime newEndDate;
            if (IsIncomplete)
            {
                lock (SyncRoot)
                {
                    var redundant = Result.Last();
                    Result.Remove(redundant);
                    var lastTweet = Result.Last();
                    newEndDate = EndDate;
                    EndDate = lastTweet.Stamp.Value;
                    newStartDate = EndDate;
                }
                Func<DateTime, DateTime, Task<TwitterResult>> run = async (startDate1, endDate1) =>
                {
                    return await fetchNextCommand(startDate1, endDate1);
                };
                t = await run(newStartDate, newEndDate);
            }
            return t;

        }

    }
}