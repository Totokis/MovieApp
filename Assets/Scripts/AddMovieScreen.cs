using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddMovieScreen : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] ImageWithUrl image;
    [SerializeField] ImageWithUrl backdropImage;
    [Header("Input fields")]
    [SerializeField] TMP_InputField titleInputField;
    [SerializeField] TMP_InputField releaseDateInputField;
    [SerializeField] TMP_InputField authorInputField;
    [SerializeField] TMP_InputField descriptionInputField;
    [SerializeField] TMP_InputField noteInputField;
    [Header("Other")]
    [SerializeField] Slider starBar;
    [SerializeField] SeenButtonLogic seenButtonLogic;

    public readonly ErrorNotification errorNotification = new ErrorNotification();
    public void SaveMovie()
    {
        if (titleInputField.text == "")
        {
            errorNotification.Message = "You must enter title";
        }
        else
        {
            var movie = new Movie(
                titleInputField.text,
                image.GetUrl(),
                backdropImage.GetUrl(),
                releaseDateInputField.text,
                seenButtonLogic.GetState(),
                starBar.value * 10f,
                0,
                descriptionInputField.text,
                authorInputField.text,
                noteInputField.text);
            DatabaseManager.Instance.SaveMovie(movie);
        }
    }
}
