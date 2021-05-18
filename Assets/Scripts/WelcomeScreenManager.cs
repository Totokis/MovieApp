using UnityEngine;

public class WelcomeScreenManager : MonoBehaviour
{
    [SerializeField] private InputController loginPage;
    [SerializeField] private InputController registerPage;

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
