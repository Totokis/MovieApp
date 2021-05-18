using System;
using System.Collections.Generic;
using UnityEngine;

internal class SavedMoviesPanel : MonoBehaviour
{
    [SerializeField] private GameObject savedMovieItemPrefab;
    private List<Movie> _moviesList;

    
    public void SetMovies(List<Movie> movies)
    {
        DestroyChildren();
        foreach (var movie in movies)
        {
            var listElement = Instantiate(savedMovieItemPrefab, transform);
            listElement.GetComponent<SavedMovieItem>().SetMovie(movie);
        }
    }
    private void DestroyChildren()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }
}
