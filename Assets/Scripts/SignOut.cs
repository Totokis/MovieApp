using Firebase.Auth;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour
{
   FirebaseAuth _auth;
   void Awake()
   {
      _auth = FirebaseAuth.DefaultInstance;
   }
   public void SignOutAndGoToLoginPage()
   {
      _auth.SignOut();
      SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
   }
}
