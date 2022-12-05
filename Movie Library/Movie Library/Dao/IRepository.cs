using MovieLibraryEntities.Models;

namespace MovieLibraryEntities.Dao
{
    public interface IRepository
    {
        void Display();
        void AddMovie(string movieTitle);
        bool SearchMovie(string searchString);
        void UpdateMovie(string searchString, string newMovieName);
        void DeleteMovie(string searchString);
        void AddUser(int userAge, string userGender, string userZipcode, int userOccupationID);
        bool ValidateUserData(string userAge, string userGender, string userZipcode, string userOccupationID);
        void AddMovieRating(string userId, string movieName, string movieRating);
        bool ValidateRatingData(string userId, string movieName, string movieRating);
    }
}