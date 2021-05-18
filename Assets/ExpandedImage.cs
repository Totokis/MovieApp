using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandedImage : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pointerDown;
    private float pointerDownTimer;

    public float requiredDownTime;
    public UnityEvent OnLongClick;
    [SerializeField] Image fillImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    private void Update()
    {
        if (pointerDown)
        { 
            ImageFrame.Instance.Open(fillImage.sprite);
        }
    }

    void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0f;
        fillImage.fillAmount = 1;
        ImageFrame.Instance.Close();
    }
    
}
