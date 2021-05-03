using System.Collections.Generic;
using UnityEngine;

class SavedMoviesManager: MonoBehaviour
{
    [SerializeField] GameObject savedMovieItemPrefab;
    List<Movie> _moviesList;
    public void SetMovies(List<Movie> movies)
    {
        foreach (var movie in movies)
        {
            var listElement = Instantiate(savedMovieItemPrefab,transform);
            listElement.GetComponent<SavedMovieItem>().SetMovie(movie);
        }
    }
}
