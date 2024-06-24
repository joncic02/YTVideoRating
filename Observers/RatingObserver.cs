using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YTVideoRating.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YTVideoRating.Observers
{
    //Rx has a helper class named Subject. Subject implements both IObservable<T>
    //and IObserver<T> so it can be both an Observer and an Observable.
    //Its OnNext(T value) method outputs value to all subscribed Observers.
    public class RatingObserver : IObserver<Rating>
    {
        private readonly string name;
        public RatingObserver(string name)
        {
            this.name = name;
        }

        //Provides the observer with new data.
        public void OnCompleted()
        {
            Console.WriteLine($"{name}: Svi komentari su uspesno pribavljeni.");
        }

        //Notifies the observer that the provider has experienced an error 
        public void OnError(Exception ex)
        {
            Console.WriteLine($"{name}: Doslo je do greske!: {ex.Message}");
        }

        //Notifies the observer that the provider has finished sending 
        public void OnNext(Rating rating) //By default, the ObserveOn operator ensures that OnNext will be called as many times as possible on the current thread.
        {
            Console.WriteLine(
                $"{name}: \n" +
                $"Video Title = {rating.VideoTitle},\n" +
                $"Channel = {rating.ChannelTitle},\n" +
                $"Views = {rating.ViewCount},\n" +
                $"Likes = {rating.LikeCount},\n" +
                $"Dislike = {rating.DislikeCount}\n");
        }
    }
}
