using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpdateMoviePanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image image;
    [SerializeField] Image backdropImage;
    [SerializeField] Sprite defaultSprite;
    [Header("Input fields")]
    [SerializeField] TMP_InputField titleInputField;
    [SerializeField] TMP_InputField releaseDateInputField;
    [SerializeField] TMP_InputField authorInputField;
    [SerializeField] TMP_InputField descriptionInputField;
    [SerializeField] TMP_InputField noteInputField;
    [Header("Other")]
    [SerializeField] Slider starBar;
    
    [Header("Buttons")]
    [SerializeField] LeanButton doneButton;
    [SerializeField] LeanButton returnButton;

    Texture2D backdropTexture;
    SavedMovieItem savedMovieItem;
    GUIManager guiManager;

    void Awake()
    {
        doneButton.OnClick.AddListener(EditDone);
        returnButton.OnClick.AddListener(Return);
        guiManager = FindObjectOfType<GUIManager>();
    }
    void Return()
    {
        gameObject.SetActive(false);
    }
    
    void EditDone()
    {
        gameObject.SetActive(false);
        savedMovieItem.UpdateMovie(new Movie(
            titleInputField.text, 
            savedMovieItem.Movie.ImageUrl, 
            savedMovieItem.Movie.BackdropUrl, 
            releaseDateInputField.text,
            savedMovieItem.Movie.Seen, 
            (starBar.value*10f),
            savedMovieItem.Movie.VotesCount, 
            descriptionInputField.text, 
            authorInputField.text, 
            noteInputField.text));
        guiManager.DetailMoviePanel.LoadMovieDetails(savedMovieItem);
    }

    async void  LoadMovieToEdit(SavedMovieItem savedMovieItem)
    {
        var movie = savedMovieItem.Movie;
        this.savedMovieItem = savedMovieItem;
        gameObject.SetActive(true);
        TextureBase.Instance.AddToQueue(movie.ImageUrl,image);
        TextureBase.Instance.AddToQueue(movie.BackdropUrl,backdropImage);
        starBar.value = (float)(movie.VoteAverage / 10f);
        titleInputField.text = movie.Title;
        releaseDateInputField.text = movie.ReleaseDate;
        authorInputField.text = movie.Author;
        descriptionInputField.text = movie.Description;
        noteInputField.text = movie.Note;
    }
    
    void OnDisable()
    {
        image.sprite = defaultSprite;
        backdropImage.sprite = defaultSprite;
    }
    
    static string FormatNumber(int num) {
        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";
        if (num >= 10000) {
            return (num / 1000D).ToString("0.#") + "K";
        }
        return num.ToString("#,0");
    }
    
    public void UpdateMovie(SavedMovieItem savedMovieItem)
    {
        gameObject.SetActive(true);
        LoadMovieToEdit(savedMovieItem);
    }
}
