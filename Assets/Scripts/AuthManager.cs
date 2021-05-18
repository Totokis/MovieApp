using System.Collections;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    private ErrorNotification _message;
    static FirebaseAuth Auth;
    static FirebaseUser User;
    [SerializeField] ErrorMessage errorMessage;

    [Header("Firebase")]
    public DependencyStatus dependencyStatus;


    private void Awake()
    {
        _message = new ErrorNotification();
        _message.MessageSet.AddListener(()=>errorMessage.ShowMessage(_message.Message));
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
                InitializeFirebase();
            else
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        Auth = FirebaseAuth.DefaultInstance;
    }

    private IEnumerator Login(string email, string password)
    {
        var LoginTask = Auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {LoginTask.Exception}");
            var firebaseException = LoginTask.Exception.GetBaseException() as FirebaseException;
            var errorCode = (AuthError)firebaseException.ErrorCode;


            var message = $"{LoginTask.Exception}";

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

            _message.Message = message;
        }
        else
        {
            User = LoginTask.Result;
            //TODO Add listener for changing state
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            _message.Message = "";
            _message.Message = "Logged in";
            yield return new WaitForSeconds(1);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }


    public void RegisterAndLogin()
    {
        Validator.Instance.RegisterAndLogin(RegisterAndLogin);
    }

    public void RegisterAndBack()
    {
        Validator.Instance.Register(Register);
    }

    public void Login()
    {
        Validator.Instance.Login(Login);
    }
    private IEnumerator Register(string email, string password)
    {
        _message.Message = "Registration started !";
        var RegisterTask = Auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

        if (RegisterTask.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with {RegisterTask.Exception}");
            var firebaseException = RegisterTask.Exception.GetBaseException() as FirebaseException;
            var errorCode = (AuthError)firebaseException.ErrorCode;

            var message = "Register Failed !";

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
            _message.Message = message;
        }
        else
        {
            _message.Message = "Registration completed !";
        }
    }

    private IEnumerator RegisterAndLogin(string email, string password)
    {
        yield return Register(email, password);
        yield return Login(email, password);
    }
}
