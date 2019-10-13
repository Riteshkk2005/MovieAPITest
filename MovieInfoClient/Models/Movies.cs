using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieInfoClient.Models
{
    public class Movies
    {
        public long MovieId { get; set; }
        public string Title { get; set; }
        public int? YearOfRelease { get; set; }
        public int runningTimeInMinutes { get; set; }
        public string Genres { get; set; }
        public decimal? Rating { get; set; }
        public long UserId { get; set; }
        public string Name { get; set; }
    }
}