using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailMoviePanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    private Image image;
    [SerializeField] private Image backdropImage;
    [Header("Text fields")]
    [SerializeField]
    private TMP_Text titleTextField;
    [SerializeField] private TMP_Text releaseDateTextField;
    [SerializeField] private TMP_Text authorTextField;
    [SerializeField] private TMP_Text descriptionTextField;
    [SerializeField] private TMP_Text noteTextField;
    [SerializeField] private TMP_Text votes;
    [Header("Other")]
    [SerializeField]
    private Slider starBar;

    public SavedMovieItem SavedMovieItem { get; private set; }

    private void OnDisable()
    {
        titleTextField.text = "";
        releaseDateTextField.text = "";
        authorTextField.text = "";
        descriptionTextField.text = "";
        noteTextField.text = "";
    }
    public void LoadMovieDetails(SavedMovieItem savedMovieItem)
    {
        var movie = savedMovieItem.Movie;
        SavedMovieItem = savedMovieItem;
        gameObject.SetActive(true);
        TextureBase.Instance.AddToQueue(movie.ImageUrl, image);
        TextureBase.Instance.AddToQueue(movie.BackdropUrl, backdropImage);
        starBar.value = (float)(movie.VoteAverage / 10f);
        votes.text = votes.text = $"{ExtenstionMethods.FormatNumber(movie.VotesCount)} reviews";
        titleTextField.text = movie.Title;
        releaseDateTextField.text = movie.ReleaseDate;
        authorTextField.text = movie.Author;
        descriptionTextField.text = movie.Description;
        noteTextField.text = movie.Note;
    }
}
