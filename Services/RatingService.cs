using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YTVideoRating.Domain;

namespace YTVideoRating.Services
{
    //Klasa koja sluzi za enkapsulaciju logike prilikom pribavljanja podataka
    public class RatingService
    {
        private readonly HttpClient client = new HttpClient();
        private readonly string apiKey = "AIzaSyBLqwT2xVXWOfu2Brxq7wzkUuHFpRnpjsQ";
        public async Task<IEnumerable<Rating>> FetchRatingsAsync(IEnumerable<string> videoIds)
        {
            var ids = string.Join(",", videoIds);
            var url = $"https://www.googleapis.com/youtube/v3/videos?part=snippet,statistics&id={ids}&key={apiKey}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            var jsonResponse = JObject.Parse(content);
            var ratings = jsonResponse["items"];
            Console.WriteLine(ratings);
            
            if(ratings == null)
            {
                return Enumerable.Empty<Rating>();
            }

            //Select je LINQ metoda koja se koristi za projekciju svakog elementa iz kolekcije u novu formu
            return ratings.Select(rating => new Rating 
            {
                VideoTitle = (string)rating["snippet"]["title"],
                ChannelTitle = (string)rating["snippet"]["channelTitle"],
                ViewCount = (long)rating["statistics"]["viewCount"],
                LikeCount = (int)(rating["statistics"]["likeCount"] ?? 0),
                DislikeCount = (int)(rating["statistics"]["dislikeCount"] ?? 0) //Ako je DislikeCount null, dodeljujemo mu vrednost 0
            });
        }
    }
}
