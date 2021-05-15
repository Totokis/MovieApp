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
    private void ShowErrorMessage()
    {
        errorMessage.ShowMessage(errorNotification.Message);
    }
    void Return()
    {
        gameObject.SetActive(false);
    }

}
