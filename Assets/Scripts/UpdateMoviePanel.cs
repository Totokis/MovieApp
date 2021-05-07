using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoviePanel : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_InputField titleInputField;
    [SerializeField] TMP_InputField releaseDateInputField;
    [SerializeField] TMP_InputField authorInputField;
    [SerializeField] TMP_InputField descriptionInputField;
    [SerializeField] TMP_InputField noteInputField;

    [Header("Button")]
    [SerializeField] Button doneButton;

    SavedMovieItem savedMovieItemReference;
    Movie movieReference;

    public void EditDone()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GUIManager>().SavedMoviePanel.gameObject.SetActive(true);
        var editedMovie = new Movie(
            titleInputField.text, 
            movieReference.ImageUrl, 
            releaseDateInputField.text, 
            movieReference.Seen,
            descriptionInputField.text, 
            authorInputField.text, 
            noteInputField.text);
            savedMovieItemReference.UpdateMovie(editedMovie);
        
    }

    public void LoadMovieToEdit(Movie movie, SavedMovieItem savedMovieItem)
    {
        movieReference = movie;
        savedMovieItemReference = savedMovieItem;
        gameObject.SetActive(true);
        image.sprite = movie.Sprite;
        titleInputField.text = movie.Title;
        releaseDateInputField.text = movie.ReleaseDate;
        authorInputField.text = movie.Author;
        descriptionInputField.text = movie.Description;
        noteInputField.text = movie.Note;
    }
}
