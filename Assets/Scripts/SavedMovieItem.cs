using System;
using System.Collections;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SavedMovieItem : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text relaseDate;
    [SerializeField] TMP_Text votes;
    [Header("Controls")]
    [SerializeField] Slider slider;
    [SerializeField] Button seenButton;
    [SerializeField] SeenButtonLogic seenButtonLogic;
    [SerializeField] Button deleteButton;
    [Header("Images")]
    [SerializeField] Sprite deafultSprite;
    [SerializeField] Image image;
    Movie movie;
    bool imageIsNotSet;

    public Movie Movie => movie;

    void Awake()
    {
        seenButtonLogic.stateChanged.AddListener(SwitchSeen);
        deleteButton.onClick.AddListener(DeleteRecord);
    }
    
    void SwitchSeen()
    {
        UpdateSeen();
    }
    async void OnEnable()
    {
        if (gameObject.activeInHierarchy)
        {
            SetButtonView();
        }
        if (imageIsNotSet)
        {
            TextureBase.Instance.AddToQueue(movie.ImageUrl,image);
        }
    }
    
    void SetButtonView()
    {
        Debug.Log($"***MovieSeen***: {movie.Seen}");
        Debug.Log($"***Button logic***: {seenButtonLogic==null}");
        seenButtonLogic.SetState(movie.Seen);
        
    }
    
   async void SetUI()
    {
        title.text = movie.Title;
        relaseDate.text = movie.ReleaseDate;
        votes.text ="Votes: "+FormatNumber(movie.VotesCount);
        slider.value = (float)(movie.VoteAverage / 10f);
        if (gameObject.activeInHierarchy)
        {
            TextureBase.Instance.AddToQueue(movie.ImageUrl,image);
        }
        else
        {
            Debug.Log("activeInHierarchy: false");
            imageIsNotSet = true;
        }
    }
   
    void UpdateValues()
    {
        Debug.Log("UpdateValues");
        this.movie = new Movie(
            movie.Title,
            movie.ImageUrl,
            movie.BackdropUrl,
            movie.ReleaseDate,
            seenButtonLogic.GetState(),
            movie.VoteAverage,
            movie.VotesCount,
            movie.Description,
            movie.Author,
            movie.Note
        );
        FindObjectOfType<DatabaseManager>().UpdateOneMovie(this.movie);
    }

    public void DeleteRecord()
    {
        FindObjectOfType<DatabaseManager>().DeleteOneMovie(movie, this);
    }
    
    public void SetMovie(Movie movie)
    {
        this.movie = movie;
        Debug.Log($"DESCRIPTION: {movie.Description}");
        SetUI();
    }
    
    public void UpdateMovie(Movie movie)
    {
        if (movie.Title != this.movie.Title)
        {
            this.movie = movie;
            UpdateValuesAndReload();
            SetUI();
        }
        else
        {
            this.movie = movie;
            UpdateValues();
            SetUI();
        }
    }
    void UpdateValuesAndReload()
    {
        Debug.Log("UpdateValues");
        this.movie = new Movie(
            movie.Title,
            movie.ImageUrl,
            movie.BackdropUrl,
            movie.ReleaseDate,
            seenButtonLogic.GetState(),
            movie.VoteAverage,
            movie.VotesCount,
            movie.Description,
            movie.Author,
            movie.Note
        );
        FindObjectOfType<DatabaseManager>().SaveMovie(this.movie);
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
    
    static string FormatNumber(int num) {
        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";
        if (num >= 10000) {
            return (num / 1000D).ToString("0.#") + "K";
        }
        return num.ToString("#,0");
    }
    
}
