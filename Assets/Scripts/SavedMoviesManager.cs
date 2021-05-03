using System.Collections.Generic;
using UnityEngine;

class SavedMoviesManager: MonoBehaviour
{
    [SerializeField] GameObject savedMovieItemPrefab;
    List<Movie> _moviesList;
    public void SetMovies(List<Movie> movies)
    { 
        DestroyChildren();
        foreach (var movie in movies)
        {
            var listElement = Instantiate(savedMovieItemPrefab,transform);
            listElement.GetComponent<SavedMovieItem>().SetMovie(movie);
        }
    }
    void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
