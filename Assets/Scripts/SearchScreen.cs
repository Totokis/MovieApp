using TMPro;
using UnityEngine;

public class SearchScreen : MonoBehaviour
{
    [SerializeField] private AddMovieScreen addMovieScreen;

    public void AddNewMovie()
    {
        OpenAddPanel();
    }

    private void OpenAddPanel()
    {
        addMovieScreen.gameObject.SetActive(true);
    }
}
