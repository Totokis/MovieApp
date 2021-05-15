using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class DetailMoviePanelUI : MonoBehaviour
{
    [SerializeField] LeanButton returnButton;
    [SerializeField] LeanButton seenButton;
    [SerializeField] LeanButton deleteButton;
    [SerializeField] LeanButton updateButton;
    DetailMoviePanel detailMoviePanel;
    GUIManager guiManager; 
    SeenButtonLogic seenButtonLogic;

    void Awake()
    {
        seenButtonLogic = seenButton.GetComponent<SeenButtonLogic>();
        guiManager = FindObjectOfType<GUIManager>();
        detailMoviePanel = GetComponent<DetailMoviePanel>();
        returnButton.OnClick.AddListener((ReturnToList));
        seenButtonLogic.stateChanged.AddListener(UpdateState);
        deleteButton.OnClick.AddListener(DeleteMovie);
        updateButton.OnClick.AddListener(UpdateMovie);
    }

    void OnEnable()
    {
       SetButtonView();
    }
    void SetButtonView()
    {
        seenButtonLogic.SetState(detailMoviePanel.SavedMovieItem.Movie.Seen);
    }
    void UpdateMovie()
    {
        var updateMoviePanel = guiManager.UpdateMoviePanel;
        var savedMovieItem = detailMoviePanel.SavedMovieItem;
        updateMoviePanel.UpdateMovie(savedMovieItem);
    }
    void DeleteMovie()
    {
        detailMoviePanel.SavedMovieItem.DeleteRecord();
        gameObject.SetActive(false);
    }
    void UpdateState()
    {
        detailMoviePanel.SavedMovieItem.UpdateSeen(seenButtonLogic.GetState());
    }

    void ReturnToList()
    {
        gameObject.SetActive(false);
    }
}
