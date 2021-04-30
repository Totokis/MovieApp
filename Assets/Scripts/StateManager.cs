using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Firebase State !");
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChanged;
        CheckUser();
    }

    private void OnDestroy()
    {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChanged;
    }
    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
       CheckUser(); 
    }
    private void CheckUser()
    {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
