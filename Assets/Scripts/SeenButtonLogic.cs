using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SeenButtonLogic : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Sprite seen;
    [SerializeField] Sprite unseen;
    [SerializeField] Image image;
    bool state; 
    
    public UnityEvent stateChanged = new UnityEvent();
    
    void Awake()
    {
        stateChanged.AddListener(ChangeSprite);
        SetState(true);
    }
    void ChangeSprite()
    {
        if (state)
            image.sprite = seen;
        else
            image.sprite = unseen;
    }
    void SwitchState()
    {
        SetState(!state);
    }

    public void SetState(bool movieSeen)
    {
        state = movieSeen;
        stateChanged.Invoke();
    }
    
    
    public bool GetState()
    {
        return state;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchState();
    }
}
