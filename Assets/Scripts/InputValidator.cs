using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

class InputValidator: MonoBehaviour
{
    enum InputType
    {
        Password,
        ConfirmPassword,
        Email
    }
    
    [SerializeField] InputType myInput;
    [SerializeField] TMP_Text label;
    [SerializeField] TMP_InputField password;
    
    TMP_InputField inputField;
    string failureMessage;
    string defaultLabel;
    string _value;
    public string Value => _value;

    public bool IsValid => CheckValidty();
    
    void Awake()
    {
        defaultLabel = label.text;
        failureMessage = "";
        inputField = GetComponent<TMP_InputField>();
    }
    
    void Update()
    {
        if (failureMessage != "")
        {
            label.text = failureMessage;
            label.color = Color.red;
        }
        else
        {
            label.text = defaultLabel;
            label.color = Color.black;
        }
    }
    bool CheckEmailValidity()
    {
        if (inputField.text.Length >= 6)
        {
            _value = inputField.text;
            failureMessage = "";
            return true;
        }
        failureMessage = "Short Password";
        return false;
    }

    bool CheckPasswordMatch()
    {
        if (password.text == inputField.text)
        {
            failureMessage = "";
            return true;
        }
        failureMessage = "Passwords dont match";
        return false;
    }
    
    bool CheckPasswordValidity()
    {
      
            if (inputField.text.Length >= 6)
            {
                _value = inputField.text;
                failureMessage = "";
                return true;
            }
            failureMessage = "Short Password";
            return false;
        
    }
    bool CheckValidty()
    {
        bool isValid = true;
        switch (myInput)
        {
            case InputType.ConfirmPassword:
                isValid = CheckPasswordMatch();
                break;
            case InputType.Email:
                isValid= CheckEmailValidity();
                break;
            case InputType.Password:
                isValid= CheckPasswordValidity();
                break;
        }
        return isValid;
    } 
    
}