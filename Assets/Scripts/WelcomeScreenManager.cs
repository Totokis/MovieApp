using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeScreenManager : MonoBehaviour
{
    [SerializeField] InputController loginPage;
    [SerializeField] InputController registerPage;

    private void Awake()
    {
        loginPage.gameObject.SetActive(true);
        registerPage.gameObject.SetActive(false);
    }

    public void ClearRegisterPage()
    {
        registerPage.ClearInput();
    }
    
    public void ClearLoginPage()
    {
        loginPage.ClearInput();
    }
}
