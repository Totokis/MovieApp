using System;
using Lean.Gui;
using UnityEngine;

public class SearchItemUI : MonoBehaviour
{
    [SerializeField] LeanButton addButton;
    private LeanShake leanShake;
    private SearchedItem searchedItem;
    
    private void Awake()
    {
        leanShake = GetComponent<LeanShake>();
        searchedItem = GetComponent<SearchedItem>();
        addButton.OnClick.AddListener(() => leanShake.Shake(10f));
        addButton.OnClick.AddListener(AddSearchedItem);
    }
    void AddSearchedItem()
    {
        var movie = searchedItem.Movie;
        DatabaseManager.Instance.SaveMovie(new Movie(movie.title, movie.imageUrl, movie.backdropImageUrl, movie.releaseDate, false, movie.voteAverage,
            movie.votesCount, movie.description));
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f);
    }

    private void OnDestroy()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.1f);

    }
}
