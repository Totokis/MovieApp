using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SavedMovieUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SavedMovieItem savedMovieItem;
    private UnityEvent onClickEvent;
    private void Awake()
    {
        onClickEvent = new UnityEvent();
        onClickEvent.AddListener(() => FindObjectOfType<GUIManager>().DetailMoviePanel.LoadMovieDetails(savedMovieItem));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent.Invoke();
    }
}
