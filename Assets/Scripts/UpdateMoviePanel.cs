using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoviePanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField]
    private Image image;
    [SerializeField] private Image backdropImage;
    [SerializeField] private Sprite defaultSprite;
    [Header("Input fields")]
    [SerializeField]
    private TMP_InputField titleInputField;
    [SerializeField] private TMP_InputField releaseDateInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField descriptionInputField;
    [SerializeField] private TMP_InputField noteInputField;
    [Header("Other")]
    [SerializeField]
    private Slider starBar;

    [Header("Buttons")]
    [SerializeField]
    private LeanButton doneButton;
    [SerializeField] private LeanButton returnButton;

    private Texture2D backdropTexture;
    private GUIManager guiManager;
    private SavedMovieItem savedMovieItem;

    private void Awake()
    {
        doneButton.OnClick.AddListener(EditDone);
        returnButton.OnClick.AddListener(Return);
        guiManager = FindObjectOfType<GUIManager>();
    }
    
    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f);
    }

    private void OnDisable()
    {
        image.sprite = defaultSprite;
        backdropImage.sprite = defaultSprite;
    }
    private void Return()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setOnComplete(() => gameObject.SetActive(false));

    }

    private void EditDone()
    {
        Return();
        savedMovieItem.UpdateMovie(new Movie(
            titleInputField.text,
            savedMovieItem.Movie.ImageUrl,
            savedMovieItem.Movie.BackdropUrl,
            releaseDateInputField.text,
            savedMovieItem.Movie.Seen,
            starBar.value * 10f,
            savedMovieItem.Movie.VotesCount,
            descriptionInputField.text,
            authorInputField.text,
            noteInputField.text));
        guiManager.DetailMoviePanel.LoadMovieDetails(savedMovieItem);
    }

    private async void LoadMovieToEdit(SavedMovieItem savedMovieItem)
    {
        var movie = savedMovieItem.Movie;
        this.savedMovieItem = savedMovieItem;
        gameObject.SetActive(true);
        TextureBase.Instance.AddToQueue(movie.ImageUrl, image);
        TextureBase.Instance.AddToQueue(movie.BackdropUrl, backdropImage);
        starBar.value = (float)(movie.VoteAverage / 10f);
        titleInputField.text = movie.Title;
        releaseDateInputField.text = movie.ReleaseDate;
        authorInputField.text = movie.Author;
        descriptionInputField.text = movie.Description;
        noteInputField.text = movie.Note;
    }
    
    public void UpdateMovie(SavedMovieItem savedMovieItem)
    {
        gameObject.SetActive(true);
        LoadMovieToEdit(savedMovieItem);
    }
}
