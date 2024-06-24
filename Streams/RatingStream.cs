using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Subjects;
using YTVideoRating.Domain;
using YTVideoRating.Services;
using Google.Apis.YouTube.v3.Data;
using Newtonsoft.Json.Linq;

namespace YTVideoRating.Streams
{
    // We can pull values out of an IEnumerable<T> by executing a foreach loop, but an IObservable<T> will push values into our code.
    // If we want to receive   the events it has to offer, we can subscribe to it. (We can also unsubscribe: the Subscribe method returns
    // an IDisposable, and if we call Dispose on that it cancels our subscription.) The Subscribe method requiresus to pass in an implementation of IObserver<T>

    // The source expects us to supply an IObserver<T> and it will then push its values into that observer
    public class RatingStream : IObservable<Rating> // IObservable<T> provides values when they become available.
    {
        private readonly Subject<Rating> ratingSubject = new Subject<Rating>();
        private readonly RatingService ratingService = new RatingService();

        public IDisposable Subscribe(IObserver<Rating> observer)
        {
            return ratingSubject.Subscribe(observer);
        }

       public async Task GetRatingsAsync(IEnumerable<string> videoIds) // IEnumerable<T> enables code to retrieve values 
       {
            try
            {
                var ratings = await ratingService.FetchRatingsAsync(videoIds);
                foreach (var rating in ratings)
                {
                    ratingSubject.OnNext(rating);
                }
                ratingSubject.OnCompleted();
            }
            catch (Exception ex)
            {
                ratingSubject.OnError(ex);
            }
            
       }
    }
}
