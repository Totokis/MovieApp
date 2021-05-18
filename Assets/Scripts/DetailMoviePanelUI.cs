using Lean.Gui;
using UnityEngine;

public class DetailMoviePanelUI : MonoBehaviour
{
    [SerializeField] private LeanButton returnButton;
    [SerializeField] private LeanButton seenButton;
    [SerializeField] private LeanButton deleteButton;
    [SerializeField] private LeanButton updateButton;
    private DetailMoviePanel detailMoviePanel;
    private GUIManager guiManager;
    private SeenButtonLogic seenButtonLogic;

    private void Awake()
    {
        seenButtonLogic = seenButton.GetComponent<SeenButtonLogic>();
        guiManager = FindObjectOfType<GUIManager>();
        detailMoviePanel = GetComponent<DetailMoviePanel>();
        returnButton.OnClick.AddListener(ReturnToList);
        seenButtonLogic.stateChanged.AddListener(UpdateState);
        deleteButton.OnClick.AddListener(DeleteMovie);
        updateButton.OnClick.AddListener(UpdateMovie);
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f);
        SetButtonView();
    }
    
    private void SetButtonView()
    {
        seenButtonLogic.SetState(detailMoviePanel.SavedMovieItem.Movie.Seen);
    }
    private void UpdateMovie()
    {
        var updateMoviePanel = guiManager.UpdateMoviePanel;
        var savedMovieItem = detailMoviePanel.SavedMovieItem;
        updateMoviePanel.UpdateMovie(savedMovieItem);
    }
    private void DeleteMovie()
    {
        detailMoviePanel.SavedMovieItem.DeleteRecord();
        gameObject.SetActive(false);
    }
    private void UpdateState()
    {
        detailMoviePanel.SavedMovieItem.UpdateSeen(seenButtonLogic.GetState());
    }

    private void ReturnToList()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setOnComplete(() => gameObject.SetActive(false));
    }
}
