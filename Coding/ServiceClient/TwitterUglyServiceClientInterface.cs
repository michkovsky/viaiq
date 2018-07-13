using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Coding.ServiceClient
{
    public interface ITwitterUglyService
    {
        Task<ObservableCollection<Tweet>> ApiV1TweetsGetAsync(DateTime startDate, DateTime endDate);

    }
    public partial class TwitterUglyServiceClient:ITwitterUglyService
    {
    }
}