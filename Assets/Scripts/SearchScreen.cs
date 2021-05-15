using System;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SearchScreen : MonoBehaviour
{
    [SerializeField] TMP_InputField searchInput;
    [SerializeField] SearchMoviePanel searchMoviePanel;
    [SerializeField] AddMovieScreen addMovieScreen;

    public void AddNewMovie()
    { 
        OpenAddPanel();
    }
    
    private void OpenAddPanel()
    {
        addMovieScreen.gameObject.SetActive(true);
    }
}
