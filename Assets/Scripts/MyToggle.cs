using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyToggle: MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SavedMovieItem savedMovieItem;
    public void OnPointerClick(PointerEventData eventData)
    {
        savedMovieItem.UpdateSeen();
    }
}
