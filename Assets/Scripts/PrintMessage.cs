using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using UnityEngine;

public class PrintMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"Current User ID : {FirebaseAuth.DefaultInstance.CurrentUser.UserId}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
