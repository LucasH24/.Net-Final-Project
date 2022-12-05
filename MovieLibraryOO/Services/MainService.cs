using ConsoleTables;
using Microsoft.Extensions.Logging;
using MovieLibraryEntities.Dao;
using MovieLibraryEntities.Models;
using MovieLibraryOO.Dto;
using System;

namespace MovieLibraryOO.Services
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IRepository _repository;


        public MainService(ILogger<MainService> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public void Invoke()
        {
            var menu = new Menu();

            Menu.MenuOptions menuChoice;
            do
            {
                menuChoice = menu.ChooseAction();

                switch (menuChoice)

                {
                    case Menu.MenuOptions.DisplayMovies:
                        _logger.LogInformation("Listing movies from database");
                        _repository.Display();
                        break;


                    case Menu.MenuOptions.AddMovie:
                        _logger.LogInformation("Adding a new movie");
                        var userMovieName = menu.GetUserResponse("Enter", "movie title:", "green");
                        _repository.AddMovie(userMovieName);
                        break;


                    case Menu.MenuOptions.SearchMovie:
                        _logger.LogInformation("Searching for a movie");
                        string userSearch = menu.GetUserResponse("Enter your", "search string:", "green");
                        bool searchedMovie = _repository.SearchMovie(userSearch);
                        break;


                    case Menu.MenuOptions.UpdateMovie:
                        _logger.LogInformation("Searching for movie to update");
                        string updateUserSearch = menu.GetUserResponse("Enter your", "search string:", "green");
                        bool updateTestSearch = _repository.SearchMovie(updateUserSearch);

                        if (updateTestSearch == true)
                        {
                            _logger.LogInformation("Updating movie");
                            string updateMovieName = menu.GetUserResponse("Enter your", "new movie name:", "green");
                            _repository.UpdateMovie(updateUserSearch, updateMovieName);
                        }

                        break;


                    case Menu.MenuOptions.DeleteMovie:
                        _logger.LogInformation("Searching for movie to delete");
                        string deleteUserSearch = menu.GetUserResponse("Enter your", "search string:", "green");
                        bool deleteTestSearch = _repository.SearchMovie(deleteUserSearch);

                        if (deleteTestSearch == true)
                        {
                            _logger.LogInformation("Deleting movie");
                            _repository.DeleteMovie(deleteUserSearch);
                        }

                        break;


                    case Menu.MenuOptions.AddUser:
                        _logger.LogInformation("Gathering user data");
                        string userAge = menu.GetUserResponse("Enter", "user age:", "green");
                        string userGender = menu.GetUserResponse("Enter", "user gender (M/F):", "green");
                        string userZipcode = menu.GetUserResponse("Enter", "user zipcode:", "green");
                        string userOccupationID = menu.GetUserResponse("Enter", "user occupation:", "green");

                        bool validateData = _repository.ValidateUserData(userAge, userGender, userZipcode, userOccupationID);

                        if (validateData == true)
                        {
                            int userAgeInt = Int32.Parse(userAge);
                            int userOccupationIDInt = Int32.Parse(userOccupationID);
                            _logger.LogInformation("Adding user");
                            _repository.AddUser(userAgeInt, userGender, userZipcode, userOccupationIDInt);
                        }
                        else
                        {
                            _logger.LogInformation("Invalid Data Entry");
                        }

                        break;




                    case Menu.MenuOptions.AddMovieRating:
                        _logger.LogInformation("Add a rating to a movie");
                        string ratingUserId = menu.GetUserResponse("Enter", "user ID:", "green");
                        string ratingUserMovie = menu.GetUserResponse("Enter", "movie name:", "green");
                        string ratingUserRating = menu.GetUserResponse("Enter", "movie rating:", "green");

                        bool validateData2 = _repository.ValidateRatingData(ratingUserId, ratingUserMovie, ratingUserRating);

                        if (validateData2 == true)
                        {
                            _logger.LogInformation("Rating movie");
                            _repository.AddMovieRating(ratingUserId, ratingUserMovie, ratingUserRating);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Data Entry");
                        }

                        break;

                }
            }
            while (menuChoice != Menu.MenuOptions.Exit);

            menu.Exit();


            Console.WriteLine("\nThanks for using the Movie Library!");

        }
    }
}