using TMPro;
using UnityEngine;

public class SearchMoviePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private SearchedItem listItemPrefab;

    private void Awake()
    {
        inputField.onValueChanged.AddListener(_ => SearchMovie());
    }

    private void SetItemsOnList()
    {
        if (inputField.text != "")
        {
            var results = APIHelper.GetNewResult(inputField.text);
            foreach (var result in results)
            {
                var searchedMovie = new SearchedMovie
                {
                    title = result.title,
                    imageUrl = "https://image.tmdb.org/t/p/w500" + result.poster_path,
                    releaseDate = result.release_date,
                    voteAverage = result.vote_average,
                    votesCount = result.vote_count,
                    backdropImageUrl = "https://image.tmdb.org/t/p/w500" + result.backdrop_path,
                    description = result.overview
                };
                var listElement = Instantiate(listItemPrefab, transform);
                listElement.GetComponent<SearchedItem>().SetMovie(searchedMovie);
            }
        }
    }

    private void DestroyChildren()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);
    }

    public void SearchMovie()
    {
        DestroyChildren();
        SetItemsOnList();
    }
}
