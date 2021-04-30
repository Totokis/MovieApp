using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Validator : MonoBehaviour
{
    [Header("Login")]
    [SerializeField] InputValidator emailLoginField;
    [SerializeField] InputValidator passwordLoginField;
    
    [Header("Register")]
    [SerializeField] InputValidator emailRegisterField;
    [SerializeField] InputValidator passwordRegisterField; 
    [SerializeField] InputValidator confirmPasswordRegister;

    public void Register(Func<string, string, IEnumerator> register)
    {
        if (emailRegisterField.IsValid 
            && passwordRegisterField.IsValid
            && confirmPasswordRegister.IsValid
            )
        {
            StartCoroutine(register(emailRegisterField.Value, passwordRegisterField.Value));
        }
    }
   
    public void Login(Func<string, string, IEnumerator> login)
    {
        if (emailLoginField.IsValid && passwordLoginField.IsValid)
        {
            StartCoroutine(login(emailLoginField.Value, passwordLoginField.Value));
        }
    }

    public void RegisterAndLogin(Func<string, string, IEnumerator> registerAndLogin)
    {
        if (emailRegisterField.IsValid 
            && passwordRegisterField.IsValid
            && confirmPasswordRegister.IsValid
        )
        {
            StartCoroutine(registerAndLogin(emailRegisterField.Value, passwordRegisterField.Value));
        }
    }
}