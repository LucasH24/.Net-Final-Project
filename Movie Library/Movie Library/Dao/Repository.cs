using Microsoft.EntityFrameworkCore;
using MovieLibraryEntities.Context;
using MovieLibraryEntities.Models;
using System.Linq;

namespace MovieLibraryEntities.Dao
{
    public class Repository : IRepository, IDisposable
    {
        private readonly IDbContextFactory<MovieContext> _contextFactory;
        private readonly MovieContext _context;

        public Repository(IDbContextFactory<MovieContext> contextFactory)
        {
            _contextFactory = contextFactory;
            _context = _contextFactory.CreateDbContext();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Display()
        {
            using (var db = new MovieContext())
            {
                Console.WriteLine("Here is the list of movies");
                foreach (var b in db.Movies)
                {
                    Console.WriteLine($"Movie: {b.Id}: {b.Title}");
                }
            }
        }

        public void AddMovie(string movieTitle)
        {
            var movie = new Movie();
            movie.Title = movieTitle;
            movie.ReleaseDate = DateTime.Now;


            using (var db = new MovieContext())
            {
                db.Movies.Add(movie);
                db.SaveChanges();
            }
        }

        public bool SearchMovie(string searchString)
        {
            searchString = searchString.ToLower();
            using (var db = new MovieContext())
            {
                var searchMovie = db.Movies.Include(a => a.MovieGenres).ThenInclude(x => x.Genre).FirstOrDefault(x => x.Title.ToLower().Contains(searchString));
                if (searchMovie != null)
                {
                    Console.WriteLine($"{searchMovie.Id}, {searchMovie.Title}");
                    foreach (var b in searchMovie.MovieGenres)
                    {
                        Console.WriteLine(b.Genre.Name);
                    }
                    return true;
                }
                else
                {
                    Console.WriteLine("No movie matches found");
                    return false;
                }
            }
        }

        public void UpdateMovie(string searchString, string newMovieName)
        {
            searchString = searchString.ToLower();
            using (var db = new MovieContext())
            {
                var movieUpdate = db.Movies.FirstOrDefault(x => x.Title.ToLower().Contains(searchString));

                movieUpdate.Title = newMovieName;

                db.Movies.Update(movieUpdate);
                db.SaveChanges();
            }
        }

        public void DeleteMovie(string searchString)
        {
            searchString = searchString.ToLower();
            using (var db = new MovieContext())
            {
                var deleteMovie = db.Movies.FirstOrDefault(x => x.Title.ToLower().Contains(searchString));

                db.Movies.Remove(deleteMovie);
                db.SaveChanges();
            }
        }

        public void AddUser(int userAge, string userGender, string userZipcode, int userOccupationID)
        {
            var user = new User();
            user.Age = userAge;
            user.Gender = userGender;
            user.ZipCode = userZipcode;

            using (var db = new MovieContext())
            {
                var occupation = db.Occupations.FirstOrDefault(x => x.Id == userOccupationID);
                user.Occupation = occupation;

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public bool ValidateUserData(string userAge, string userGender, string userZipcode, string userOccupationID)
        {
            bool isDataValid = true;

            try
            {
                int userAgeInt = Int32.Parse(userAge);
                int userZipcodeInt = Int32.Parse(userZipcode);
                int userOccupationInt = Int32.Parse(userOccupationID);

                if (userAgeInt > 120 || userAgeInt < 0) isDataValid = false;
                if (userGender != "M")
                {
                    if (userGender != "F") isDataValid = false;
                }
                if (userZipcode.Length != 5) isDataValid = false;
                if (userOccupationInt < 0 || userOccupationInt > 22) isDataValid = false;
            }
            catch (Exception e)
            {
                isDataValid = false;
            }

            return isDataValid;
        }

        public void AddMovieRating(string userId, string movieName, string movieRating)
        {
            int userIdInt = Int32.Parse(userId);
            int movieRatingInt = Int32.Parse(movieRating);

            var userMovie = new UserMovie();
            userMovie.Rating = movieRatingInt;
            userMovie.RatedAt = DateTime.Now;

            using (var db = new MovieContext())
            {
                movieName = movieName.ToLower();
                var dbMovie = db.Movies.FirstOrDefault(x => x.Title.ToLower().Contains(movieName));
                userMovie.Movie = dbMovie;

                var dbUser = db.Users.Include(x => x.Occupation).FirstOrDefault(x => x.Id == userIdInt);
                userMovie.User = dbUser;



                Console.WriteLine($"User ID: {dbUser.Id}, User Age: {dbUser.Age}, User Gender: {dbUser.Gender}, User Zipcode: {dbUser.ZipCode}, User Occupation: {dbUser.Occupation.Name}");

                db.UserMovies.Add(userMovie);
                db.SaveChanges();
            }
        }

        public bool ValidateRatingData(string userId, string movieName, string movieRating)
        {
            bool isDataValid = true;

            try
            {
                int userIdInt = Int32.Parse(userId);
                int movieRatingInt = Int32.Parse(movieRating);
                if (movieRatingInt > 5 || movieRatingInt < 1) isDataValid = false;
                bool testMovie = SearchMovie(movieName);
                if (testMovie == false) isDataValid = false;

                using (var db = new MovieContext())
                {
                    var searchUser = db.Users.FirstOrDefault(x => x.Id == userIdInt);
                    if (searchUser == null)
                    {
                        isDataValid = false;
                        Console.WriteLine("No user with that ID was found");
                    }
                }
            }

            catch (Exception e)
            {
                isDataValid = false;
            }

            return isDataValid;
        }

            
    }
}
