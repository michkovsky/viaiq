using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tweet = Coding.ServiceClient.Tweet;
using TwitterResult = Coding.Models.TwitterUglyService.TwitterResult;
using TwitterUglyServiceClient = Coding.ServiceClient.TwitterUglyServiceClient;
using ITwitterUglyService = Coding.ServiceClient.ITwitterUglyService;
using Microsoft.Extensions.Configuration;

namespace Coding.Controllers.Strategy
{
    public class TwitterUglyServiceStrategy {
        ITwitterUglyService _twitterService;

        public TwitterUglyServiceStrategy(ITwitterUglyService service){
            _twitterService = service;
        }
        protected virtual int timeframeLength { get { return 6; } }
        protected virtual int getTimeIntervalsCount(DateTime startDate, DateTime endDate)
        {
            var ret = 1 + (endDate - startDate).Days / timeframeLength;
            return ret;
        }
        protected virtual async Task<TwitterResult> getTweetPortion(DateTime startDate, DateTime endDate)
        {
            var result = new TwitterResult
            {
                StartDate = startDate,
                EndDate = endDate,
            };
            var tweetList = await _twitterService.ApiV1TweetsGetAsync(startDate, endDate);
            if (tweetList.Any())
            {
                result.Result.AddRange(tweetList);
            }
            return result;
        }
        protected virtual IEnumerable<Task<T>> createInitialTaskList<T>(DateTime startDate, DateTime endDate, Func<DateTime, DateTime, Task<T>> cmdRun)
        {
            var timeperiods = getTimeIntervalsCount(startDate, endDate);
            var ret = Enumerable.Range(0, timeperiods)
            .Select(periodNumber => new
            {
                StartDate = startDate.AddDays(periodNumber * timeframeLength),
                EndDate = startDate.AddDays((periodNumber + 1) * timeframeLength).AddHours(1),/* we have no ideas about actual time accuracy*/
            })
            .Select(async period =>
            {
                return await cmdRun(period.StartDate, period.EndDate);
            })
            .AsEnumerable();

            return ret;
        }
        public virtual async Task<IEnumerable<Tweet>> GetTweets(DateTime startDate, DateTime? endDate = null)
        {
            //idea:
            // 1. split time period on time ranges
            // 2. create async tasks to request data
            // 3. collect this time-groups
            // 4. recurively normalize all timegroups (any length should be < 100 ) and fetch next portion
            // 5. sort-filter result
            endDate = endDate ?? DateTime.Now;
            
            //to increase performance we can provide access to tweetGroups for any async task
            //but for now leave it as is
            var tweetGroups = new List<TwitterResult>();
            

            var tasks = createInitialTaskList(startDate, endDate.Value, getTweetPortion);
            tweetGroups.AddRange(await Task.WhenAll(tasks));

            while (tweetGroups.Any(x => x.IsIncomplete))//recurcively fill incomplete ranges with CreateFetchNext
            {
                tasks = tweetGroups.Where(x => x.IsIncomplete).Select(x => x.CreateFetchNext(getTweetPortion)).AsEnumerable();
                tweetGroups.AddRange(await Task.WhenAll(tasks));
            }

            //filter-out gready requests
            var tweetResult = tweetGroups
            .SelectMany(g => g.Result)
            .GroupBy(g => g.Id)
            .Select(g => g.First())
            .Where(el => el.Stamp >= startDate && el.Stamp <= endDate)
            .OrderBy(el => el.Stamp)
            .ToList();


            return tweetResult;
        }
    }
}
