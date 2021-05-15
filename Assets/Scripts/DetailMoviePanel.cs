using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DetailMoviePanel : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] Image image;
    [SerializeField] Image backdropImage;
    [SerializeField] Sprite defaultSprite;
    [Header("Text fields")]
    [SerializeField] TMP_Text titleTextField; 
    [SerializeField] TMP_Text releaseDateTextField;
    [SerializeField] TMP_Text authorTextField;
    [SerializeField] TMP_Text descriptionTextField;
    [SerializeField] TMP_Text noteTextField;
    [SerializeField] TMP_Text votes;
    [Header("Other")]
    [SerializeField] Slider starBar;

    Texture2D backdropTexture;
    SavedMovieItem savedMovieItem;

    public SavedMovieItem SavedMovieItem => savedMovieItem;
    public async void LoadMovieDetails(SavedMovieItem savedMovieItem)
    {
        var movie = savedMovieItem.Movie;
        this.savedMovieItem = savedMovieItem;
        gameObject.SetActive(true);
        TextureBase.Instance.AddToQueue(movie.ImageUrl,image);
        TextureBase.Instance.AddToQueue(movie.BackdropUrl, backdropImage);
        starBar.value = (float)(movie.VoteAverage / 10f);
        votes.text = votes.text = $"{FormatNumber(movie.VotesCount)} reviews";
        titleTextField.text = movie.Title;
        releaseDateTextField.text = movie.ReleaseDate;
        authorTextField.text = movie.Author;
        descriptionTextField.text = movie.Description;
        noteTextField.text = movie.Note;
    }
  
    void OnDisable()
    {
        titleTextField.text = "";
        releaseDateTextField.text = "";
        authorTextField.text = "";
        descriptionTextField.text = "";
        noteTextField.text = "";
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
}
