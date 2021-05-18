using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

internal class SearchedItem : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text releaseDate;
    [SerializeField] TMP_Text votes;
    [SerializeField] Image image;
    [SerializeField] Sprite deafulSprite;
    [SerializeField] Slider slider;

    private SearchedMovie movie;
    public SearchedMovie Movie => movie;
    private Texture2D texture;
    
    public void SetMovie(SearchedMovie movie)
    {
        this.movie = movie;
        title.text = this.movie.title;
        releaseDate.text = this.movie.releaseDate;
        slider.value = (float)(movie.voteAverage / 10f);
        votes.text = $"{ExtenstionMethods.FormatNumber(movie.votesCount)} reviews";
        TextureBase.Instance.AddToQueue(movie.imageUrl, image);
    }

    
}
