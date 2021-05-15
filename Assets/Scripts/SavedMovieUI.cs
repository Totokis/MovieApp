using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SavedMovieUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] SavedMovieItem savedMovieItem;
    UnityEvent onClickEvent;
    void Awake()
    {
        onClickEvent = new UnityEvent();
        onClickEvent.AddListener(() => FindObjectOfType<GUIManager>().DetailMoviePanel.LoadMovieDetails(savedMovieItem));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent.Invoke();
    }
}
