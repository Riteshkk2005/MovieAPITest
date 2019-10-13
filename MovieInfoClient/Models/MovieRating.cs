using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieInfoClient.Models
{
    public class MovieRating
    {
        public int Id { get; set; }
        public string Rating { get; set; }
        public string UserId { get; set; }
        public string MovieId { get; set; }
    }
}