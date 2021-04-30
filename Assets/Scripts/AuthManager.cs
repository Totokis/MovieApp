using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;


public class AuthManager : MonoBehaviour
{
    [SerializeField] Validator _validator;
    [SerializeField] TMP_Text _message;
    
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    static public FirebaseAuth Auth;
    static public FirebaseUser User;


    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        Auth = FirebaseAuth.DefaultInstance;
    }
    
    private IEnumerator Login(string email, string password)
    {
        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning(message:$"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;
            

            string message = $"{LoginTask.Exception}";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "User not found";
                    break;
            }
            
            _message.text = message;
        }
        else
        {
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",User.DisplayName, User.Email);
            _message.text = "";
            _message.text = "Logged in";
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
       
    }
    

    public void RegisterAndLogin()
    {
        _validator.RegisterAndLogin(RegisterAndLogin);
    }
    
    public void RegisterAndBack()
    {
        _validator.Register(Register);
    }
    
    public void Login()
    {
        _validator.Login(Login);
    }
    IEnumerator Register(string email, string password)
    {
        _message.text = "Registration started !";
        var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: (() => RegisterTask.IsCompleted));

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
            FirebaseException firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseException.ErrorCode;

            string message = "Register Failed !";

            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WeakPassword:
                    message = "Weak Password";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "Email aleady in use";
                    break;
            }
            _message.text = message;
        }
        else
        {
            _message.text = "Registration completed !";
        }
    }
    
    IEnumerator RegisterAndLogin(string email, string password)
    {
        yield return Register(email, password);
        yield return Login(email, password);
    }
    
}

