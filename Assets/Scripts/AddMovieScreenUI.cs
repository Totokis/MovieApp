using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;

public class AddMovieScreenUI : MonoBehaviour
{
    [SerializeField] LeanButton editDoneButton;
    [SerializeField] LeanButton returnButton;
    [SerializeField] AddMovieScreen addMovieScreen;
    [SerializeField] ErrorMessage errorMessage;
    
    ErrorNotification errorNotification;
    void Awake()
    {
        errorNotification = addMovieScreen.errorNotification;
        errorNotification.MessageSet.AddListener(ShowErrorMessage);
        editDoneButton.OnClick.AddListener(addMovieScreen.SaveMovie);
        returnButton.OnClick.AddListener(Return);
    }
    void ShowErrorMessage()
    {
        errorMessage.ShowMessage(errorNotification.Message);
    }
    void Return()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setOnComplete(() => gameObject.SetActive(false));
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f);
    }

}
