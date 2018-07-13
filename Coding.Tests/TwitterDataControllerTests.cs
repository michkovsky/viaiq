using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Coding.ServiceClient;
using Moq;
using Xunit;


namespace Coding.Controllers.Tests
{
    public class TwitterDataControllerTests
    {
        [Fact]
        public async void GetTweets_check_concatenation()
        {
            var mockService = new Mock<ITwitterUglyService>();
            mockService
            .Setup(svc => svc.ApiV1TweetsGetAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .Returns<DateTime, DateTime>(async (a, b) => { return await GetTop100Ugly(a, b); });
            var controller = new TwitterDataController(null, mockService.Object);
            var result = await controller.GetTweets(startDate, startDate.AddYears(2));
            Assert.Equal(tweets, result.ToList());
        }
        const int minutes = 10000;
        static DateTime startDate = new DateTime(2017, 1, 1);
        List<Tweet> tweets = Enumerable.Range(0, 10000)
        .Select(i => new Tweet
        {
            Id = string.Format("{0:00000000}", i),
            Stamp = startDate.AddMinutes(i),
            Text = string.Format("text--{0:X}--", i),
        }).ToList();
        private async Task<ObservableCollection<Tweet>> GetTop100Ugly(DateTime startDate, DateTime endDate)
        {
            var result = tweets.Where(t => t.Stamp >= startDate && t.Stamp <= endDate).Take(100);
            return new ObservableCollection<Tweet>(result);
        }
    }
}
