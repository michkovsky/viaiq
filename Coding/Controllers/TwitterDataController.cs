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
namespace Coding.Controllers
{
    [Route("api/[controller]")]
    public class TwitterDataController : Controller
    {
        IConfiguration _configuration;
        ITwitterUglyService _twitterService;
        public TwitterDataController(IConfiguration configuration, ITwitterUglyService twitterService){
            _configuration = configuration;
            _twitterService = twitterService;
        }



        [HttpGet("[action]")]
        public virtual async Task<IEnumerable<Tweet>> GetTweets(DateTime startDate, DateTime? endDate = null)
        {
            var strategy =new Strategy.TwitterUglyServiceStrategy(_twitterService); 

            return await strategy.GetTweets(startDate,endDate);
        }
    }
}
