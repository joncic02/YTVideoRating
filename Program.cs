using Google.Apis.YouTube.v3;
using System;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using YTVideoRating.Observers;
using YTVideoRating.Services;
using YTVideoRating.Streams;


namespace YTVideoRating
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string url = "http://localhost:5050/";

            HttpListener listener = new HttpListener();

            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Server running at " + url);

            // Kreiranje TaskPoolScheduler-a
            var scheduler = TaskPoolScheduler.Default;

            var ratingStream = new RatingStream(); //Kreiramo Stream


            var cobyObserver = new RatingObserver("Coby Observer"); //Kreiramo Observer za Cobijeve pesme
            var cobyStream = ratingStream
                .Where(rating => rating.VideoTitle.Contains("Coby") || rating.ChannelTitle.Contains("Coby")) //Filtriramo Stream za Cobijeve pesme
                .ObserveOn(scheduler); // Planira izvršavanje na TaskPoolScheduler-u

            var cobySubscription = cobyStream.Subscribe(cobyObserver); //Pretplacujemo Coby Observera na Stream


            var prijovicObserver = new RatingObserver("Prijovicka Observer");
            var prijovicStream = ratingStream
                .Where(rating => rating.VideoTitle.Contains("Prijovic") || rating.ChannelTitle.Contains("Prijovic"))
                .ObserveOn(scheduler);

            var prijovicSubscription = prijovicStream.Subscribe(prijovicObserver);


            var teaObserver = new RatingObserver("Tea Tairovic Observer");
            var teaStream = ratingStream
                .Where(rating => rating.VideoTitle.Contains("Tea Tairovic") || rating.ChannelTitle.Contains("Tea Tairovic"))
                .ObserveOn(scheduler);

            var teaSubscription = teaStream.Subscribe(teaObserver);

            var videoIds = new List<string>
            {
                "S3Oto6845sszjjU", "TgHi0LxiPfg", "n1HDBeN0pJA", 
                "rzzwzvYbcbk", "Evt2depoBrY", "uYrI0Zf_Gno",
                "40e5oH1pDlM", "BuVnUTW-0ps", "mdJQZHodyxo", //Coby
                "nVxixq98XFE", "8Ym7kBN0eAY", "IWti5VDb2-U", //Prijovicka
                "gBILlF4ahAU", "jTG84Uv71hA", "PfB8vsFm2Qk"  //Tea Tairovic
            };

            //Pribavljamo podatke
            await ratingStream.GetRatingsAsync(videoIds);

            Console.ReadLine();

            //Unsubscribe the subscribers
            cobySubscription.Dispose();
            prijovicSubscription.Dispose();
            teaSubscription.Dispose();

        }
    }

}