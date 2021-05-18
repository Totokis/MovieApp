using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Validator : MonoBehaviour
{
    [Header("Login")]
    [SerializeField]
    private InputValidator emailLoginField;
    [SerializeField] private InputValidator passwordLoginField;

    [Header("Register")]
    [SerializeField]
    private InputValidator emailRegisterField;
    [SerializeField] private InputValidator passwordRegisterField;
    [SerializeField] private InputValidator confirmPasswordRegister;

    public static Validator Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void Register(Func<string, string, IEnumerator> register)
    {
        if (emailRegisterField.IsValid
            && passwordRegisterField.IsValid
            && confirmPasswordRegister.IsValid
        )
            StartCoroutine(register(emailRegisterField.Value, passwordRegisterField.Value));
    }

    public void Login(Func<string, string, IEnumerator> login)
    {
        if (emailLoginField.IsValid && passwordLoginField.IsValid)
            StartCoroutine(login(emailLoginField.Value, passwordLoginField.Value));
    }

    public void RegisterAndLogin(Func<string, string, IEnumerator> registerAndLogin)
    {
        if (emailRegisterField.IsValid
            && passwordRegisterField.IsValid
            && confirmPasswordRegister.IsValid
        )
            StartCoroutine(registerAndLogin(emailRegisterField.Value, passwordRegisterField.Value));
    }
}
