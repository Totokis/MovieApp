using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchMoviePanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SearchedItem listItemPrefab;

    void Awake()
    {
        inputField.onValueChanged.AddListener(_=>SearchMovie());
    }

    void SetItemsOnList()
    {
        if (inputField.text != "")
        {
            List<Result> results = APIHelper.GetNewResult(inputField.text);
            foreach (var result in results)
            {
                SearchedMovie searchedMovie = new SearchedMovie { 
                    title = result.title, 
                    imageUrl = "https://image.tmdb.org/t/p/w500"+result.poster_path,
                    releaseDate = result.release_date,
                    voteAverage = result.vote_average,
                    votesCount = result.vote_count,
                    backdropImageUrl = "https://image.tmdb.org/t/p/w500"+result.backdrop_path,
                    description = result.overview
                };
                var listElement = Instantiate(listItemPrefab,transform);
                listElement.GetComponent<SearchedItem>().SetMovie(searchedMovie);
            }
        }
    }
    
    void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    
    public void SearchMovie()
    {
        DestroyChildren();
        SetItemsOnList();
    }

}