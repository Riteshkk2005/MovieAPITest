using MovieInfoClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Http.Headers;
 
namespace MovieInfoClient
{
    public partial class _Default : Page
    {
        static HttpClient client;
        static List<Movies> movies = new List<Movies>();

        private void setClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44314/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnTitle_Click(object sender, EventArgs e)
        {
            setClient();
            string title = mtitle.Text;
            //HTTP GET
            var responseTask = client.GetAsync("GetMoviesByTitle?title=" + title);
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Movies>>();
                readTask.Wait();
                grdMoviesByTitle.DataSource = readTask.Result;
                grdMoviesByTitle.DataBind();
                message.Text = result.StatusCode.ToString();
            }
            else
            {
                grdMoviesByTitle.DataSource = null;
                message.Text = result.ReasonPhrase + result.StatusCode;
            }
        }

        protected void btnGenres_Click(object sender, EventArgs e)
        {
            setClient();
            string gen = mGenres.Text;
            //HTTP GET
            var responseTask = client.GetAsync("GetMoviesByGenres?genres=" + gen);
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Movies>>();
                readTask.Wait();
                grdMoviesByGenres.DataSource = readTask.Result;
                grdMoviesByGenres.DataBind();
                message.Text = result.StatusCode.ToString();
            }
            else
            {
                grdMoviesByGenres.DataSource = null;
                message.Text = result.ReasonPhrase + result.StatusCode;
            }
        }

        protected void btnYear_Click(object sender, EventArgs e)
        {
            setClient();
            string year = mYear.Text;
            //HTTP GET
            var responseTask = client.GetAsync("GetMoviesByYear?year=" + year);
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Movies>>();
                readTask.Wait();
                grdYear.DataSource = readTask.Result;
                grdYear.DataBind();
                message.Text = result.StatusCode.ToString();
            }
            else
            {
                grdYear.DataSource = null;
                message.Text = result.ReasonPhrase + result.StatusCode;
            }
        }

        protected void btnTotalMovies_Click(object sender, EventArgs e)
        {
            setClient();
            //HTTP GET
            var responseTask = client.GetAsync("GetTopRatedMovies");
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Movies>>();
                readTask.Wait();
                grdTotalMovies.DataSource = readTask.Result;
                grdTotalMovies.DataBind();
                message.Text = result.StatusCode.ToString();
            }
            else
            {
                grdTotalMovies.DataSource = null;
                message.Text = result.ReasonPhrase + result.StatusCode;
            }
        }

        protected void btnUserRating_Click(object sender, EventArgs e)
        {
            setClient();
            string user= txtUserId.Text;
            //HTTP GET
            var responseTask = client.GetAsync("GetTopRatedMoviesByUser?userId=" + user );
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<List<Movies>>();
                readTask.Wait();
                grdUserRating.DataSource = readTask.Result;
                grdUserRating.DataBind();
                message.Text = result.StatusCode.ToString();
            }
            else
            {
                grdUserRating.DataSource = null;
                message.Text = result.ReasonPhrase + result.StatusCode;
            }
        }

        protected void btnRate_Click(object sender, EventArgs e)
        {
            setClient();
            string user = txtUserId.Text;

            MovieRating movieRating = new MovieRating() { MovieId = txtMovieId.Text, UserId = txtUsrId.Text, Rating = txtRate.Text };
            //HTTP GET
            var responseTask = client.PostAsJsonAsync("CreateMovieRating", movieRating );
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                message.Text = result.StatusCode.ToString();
            }
            else
                message.Text = result.ReasonPhrase + result.StatusCode;
        }

        protected void btnUpdateRate_Click(object sender, EventArgs e)
        {
            setClient();
            string user = txtUserId.Text;

            MovieRating movieRating = new MovieRating() { MovieId = txtMovieId.Text, UserId = txtUsrId.Text, Rating = txtRate.Text };
            //HTTP GET
            var responseTask = client.PutAsJsonAsync("UpdateMovieRating", movieRating);
            responseTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                message.Text = result.StatusCode.ToString();
            }
            else
                message.Text = result.ReasonPhrase + result.StatusCode;
        }
    }
}