using System;
using Lean.Gui;
using UnityEngine;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] LeanWindow leanWindow;
    
    public void ShowMessage(string errorNotificationMessage)
    {
        text.text = errorNotificationMessage;
        leanWindow.TurnOn();
    }
}
