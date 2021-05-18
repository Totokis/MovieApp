using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SavedMovieItem : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TMP_Text title;
    [SerializeField] private TMP_Text relaseDate;
    [SerializeField] private TMP_Text votes;
    [Header("Controls")]
    [SerializeField]
    private Slider slider;
    [SerializeField] private Button seenButton;
    [SerializeField] private SeenButtonLogic seenButtonLogic;
    [SerializeField] private Button deleteButton;
    [Header("Images")]
    [SerializeField] private Image image;
    private bool imageIsNotSet;

    public Movie Movie { get; private set; }

    private void Awake()
    {
        seenButtonLogic.stateChanged.AddListener(SwitchSeen);
        deleteButton.onClick.AddListener(DeleteRecord);
    }
    private void OnEnable()
    {
        if (gameObject.activeInHierarchy)
            SetButtonView();
        if (imageIsNotSet)
            TextureBase.Instance.AddToQueue(Movie.ImageUrl, image);
    }

    private void SwitchSeen()
    {
        UpdateSeen();
    }

    private void SetButtonView()
    {
        Debug.Log($"***MovieSeen***: {Movie.Seen}");
        Debug.Log($"***Button logic***: {seenButtonLogic == null}");
        seenButtonLogic.SetState(Movie.Seen);
    }

    private async void SetUI()
    {
        title.text = Movie.Title;
        relaseDate.text = Movie.ReleaseDate;
        votes.text = "Votes: " + ExtenstionMethods.FormatNumber(Movie.VotesCount);
        slider.value = (float)(Movie.VoteAverage / 10f);
        if (gameObject.activeInHierarchy)
        {
            TextureBase.Instance.AddToQueue(Movie.ImageUrl, image);
        }
        else
        {
            Debug.Log("activeInHierarchy: false");
            imageIsNotSet = true;
        }
    }

    private void UpdateValues()
    {
        Debug.Log("UpdateValues");
        Movie = new Movie(
            Movie.Title,
            Movie.ImageUrl,
            Movie.BackdropUrl,
            Movie.ReleaseDate,
            seenButtonLogic.GetState(),
            Movie.VoteAverage,
            Movie.VotesCount,
            Movie.Description,
            Movie.Author,
            Movie.Note
        );
        FindObjectOfType<DatabaseManager>().UpdateOneMovie(Movie);
    }

    public void DeleteRecord()
    {
        FindObjectOfType<DatabaseManager>().DeleteOneMovie(Movie, this);
    }

    public void SetMovie(Movie movie)
    {
        this.Movie = movie;
        Debug.Log($"DESCRIPTION: {movie.Description}");
        SetUI();
    }

    public void UpdateMovie(Movie movie)
    {
        if (movie.Title != this.Movie.Title)
        {
            this.Movie = movie;
            UpdateValuesAndReload();
            SetUI();
        }
        else
        {
            this.Movie = movie;
            UpdateValues();
            SetUI();
        }
    }
    private void UpdateValuesAndReload()
    {
        Debug.Log("UpdateValues");
        Movie = new Movie(
            Movie.Title,
            Movie.ImageUrl,
            Movie.BackdropUrl,
            Movie.ReleaseDate,
            seenButtonLogic.GetState(),
            Movie.VoteAverage,
            Movie.VotesCount,
            Movie.Description,
            Movie.Author,
            Movie.Note
        );
        FindObjectOfType<DatabaseManager>().SaveMovie(Movie);
    }

    public void UpdateSeen()
    {
        UpdateValues();
    }

    public void UpdateSeen(bool seen)
    {
        seenButtonLogic.SetState(seen);
        UpdateValues();
    }
}
