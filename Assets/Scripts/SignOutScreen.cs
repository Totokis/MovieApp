using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignOutScreen : MonoBehaviour
{
    [SerializeField] SignOut signOut;

    void OnEnable()
    {
        signOut.SignOutAndGoToLoginPage();
    }
}
