using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using MoviesInfo.Models;

namespace MoviesInfo.Controllers
{
    public class MoviesListsController : ApiController
    {
        private MovieDBEntities db = new MovieDBEntities();
        public MoviesListsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/GetTopRatedMovies
        [Route("api/GetTopRatedMovies")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult GetTopRatedMovies()
        {
            //To get top 5 rated movies
            List<Movies> moviesList = GetAllMovies().Take(5).ToList();
            if (moviesList.Count == 0) { ValidationError("No Recodrs Found.", HttpStatusCode.NotFound); }
            SetFormatter();
            return Ok(moviesList);
        }
        // GET: api/GetTopRatedMovies
        [Route("api/GetTopRatedMoviesByUser")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult GetTopRatedMoviesByUser(int userId)
        {
            //To get top 5 rated movies
            if (userId == 0) { ValidationError("UserId Required.", HttpStatusCode.BadRequest); }
            List<Movies> moviesList = GetMoviesForUser().Where(t => t.UserId == userId).Take(5).ToList();
            if (moviesList.Count == 0) { ValidationError("No Recodrs Found.", HttpStatusCode.NotFound); }
            SetFormatter();
            return Ok(moviesList);
        }

        #region "Movies Filter Criteria"

        // GET: api/GetMoviesListByTitle?title="troy"
        [Route("api/GetMoviesByTitle")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult GetMoviesByTitle(string title)
        {
            //Get all movies by Title
            if (title == null || title.Length == 0) { ValidationError("Movies Title Required.", HttpStatusCode.BadRequest); }
            List<Movies> moviesList = GetAllMovies().Where(ml => ml.Title.ToLower().StartsWith(title)).ToList(); 
            if (moviesList.Count ==0) { ValidationError("No Recodrs Found.", HttpStatusCode.NotFound); }
            SetFormatter();
            return Ok(moviesList);
        }
        
        // GET: api/GetMoviesByGenres?genres="Comedy"
        [Route("api/GetMoviesByGenres")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult GetMoviesByGenres(string genres)
        {
            //Get all movies by Genres
            if (genres == null || genres.Length == 0) { ValidationError("Movies Genres Required.", HttpStatusCode.BadRequest); }
            List<Movies> moviesList = GetAllMovies().Where(ml => ml.Genres.ToLower().StartsWith(genres)).ToList();
            if (moviesList.Count == 0) { ValidationError("No Recodrs Found.", HttpStatusCode.NotFound); }
            SetFormatter();
            return Ok(moviesList);
        }

        // GET: api/GetMoviesByYear?year="2008"
        [Route("api/GetMoviesByYear")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult GetMoviesByYear(int year)
        {
            //Get all movies by Genres
            if (year == 0) { ValidationError("Movies Release Year Required.", HttpStatusCode.BadRequest ); }
            List<Movies> moviesList = GetAllMovies().Where(ml => ml.YearOfRelease == year).ToList();
            if (moviesList.Count == 0) { ValidationError("No Recodrs Found.", HttpStatusCode.NotFound); }
            SetFormatter();
            return Ok(moviesList);
        }

        #endregion

        private List<Movies> GetMoviesForUser()
        {
            return (from ml in db.MoviesLists
                    join mr in db.MovieRatings on ml.MovieId equals mr.MovieId
                    join usr in db.UserLists on mr.UserId equals usr.UserId
                    group mr by new { ml.MovieId, ml.Title, ml.runningTimeInMinutes, ml.Genres, ml.YearOfRelease, usr.UserId, usr.UserName } into movie
                    orderby movie.Key.Title
                    select new Movies()
                    {
                        MovieId = movie.Key.MovieId,
                        Title = movie.Key.Title,
                        Genres = movie.Key.Genres,
                        Rating = Math.Round((double) movie.Average(t => t.Rating)),  //Round it
                        //Rating = movie.Average(t => t.Rating) ?? 0, 
                        runningTimeInMinutes = movie.Key.runningTimeInMinutes,
                        YearOfRelease = movie.Key.YearOfRelease,
                        UserId = movie.Key.UserId,
                        Name = movie.Key.UserName
                    }).ToList();
        }

        private List<Movies> GetAllMovies()
        {
            //To get all movies with rating
            return (from ml in db.MoviesLists
                    join mr in db.MovieRatings on ml.MovieId equals mr.MovieId
                    into movie1
                    from movie2 in movie1.DefaultIfEmpty()  // Left join
                    group movie2 by new { ml.MovieId, ml.Title, ml.runningTimeInMinutes, ml.Genres, ml.YearOfRelease } into movie
                    orderby movie.Average(t => t.Rating) descending
                    select new Movies()
                    {
                        MovieId = movie.Key.MovieId,
                        Title = movie.Key.Title,
                        Genres = movie.Key.Genres,
                        Rating = Math.Round((double)movie.Average(t => t.Rating)),  //Round it
                        runningTimeInMinutes = movie.Key.runningTimeInMinutes,
                        YearOfRelease = movie.Key.YearOfRelease
                    }).ToList();
        }

        // POST: api/RateMovie
        [Route("api/CreateMovieRating")]
        [ResponseType(typeof(MoviesList))]
        public IHttpActionResult PostMoviesList(MovieRating movieRating)
        {
            List<MovieRating> movie = db.MovieRatings.Where(r => r.MovieId == movieRating.MovieId && r.UserId == movieRating.UserId).ToList();
            
            if (movie.Count > 0) { ValidationError("Invalid Movie Id", HttpStatusCode.BadRequest); }
            if (movieRating.Rating > 5 && movieRating.Rating <= 0) { ValidationError("Invalid Rating, It should be 1 to 5 Only", HttpStatusCode.BadRequest); }
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            
            db.MovieRatings.Add(movieRating);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.OK);
        }

        // PUT: api/MoviesLists/
        [Route("api/UpdateMovieRating")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMoviesList(MovieRating movieRating)
        {
            List< MovieRating> rating = db.MovieRatings.AsNoTracking().Where(r=> r.MovieId == movieRating.MovieId && r.UserId == movieRating.UserId).ToList();
           
            if (movieRating.Rating > 5 && movieRating.Rating <= 0) { ValidationError("Invalid Rating, It should be 1 to 5 Only", HttpStatusCode.BadRequest); }
            if (!ModelState.IsValid) { return BadRequest(ModelState);}
            if (rating.Count == 0) { ValidationError("No Records Found", HttpStatusCode.NotFound); }

            db.Entry(movieRating).State = EntityState.Modified;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.OK);
        }
        
        // DELETE: api/MoviesLists/5
        [ResponseType(typeof(MoviesList))]
        public async Task<IHttpActionResult> DeleteMoviesList(long id)
        {
            MoviesList moviesList = await db.MoviesLists.FindAsync(id);
            if (moviesList == null)
            {
                return NotFound();
            }

            db.MoviesLists.Remove(moviesList);
            await db.SaveChangesAsync();

            return Ok(moviesList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoviesListExists(long id)
        {
            return db.MoviesLists.Count(e => e.MovieId == id) > 0;
        }

        private void ValidationError(string errorMsg, HttpStatusCode code)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(errorMsg),
                ReasonPhrase = errorMsg,
                StatusCode = code
            };
            throw new HttpResponseException(resp);
        }
        private void SetFormatter()
        {
            IContentNegotiator negotiator = this.Configuration.Services.GetContentNegotiator();
            ContentNegotiationResult result = negotiator.Negotiate(typeof(MoviesList), this.Request, this.Configuration.Formatters);

            if (result == null)
            {
                var response = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                throw new HttpResponseException(response);
            }
        }
    }
}