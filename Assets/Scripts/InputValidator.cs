using TMPro;
using UnityEngine;

internal class InputValidator : MonoBehaviour
{

    [SerializeField] private InputType myInput;
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_InputField password;
    private string defaultLabel;
    private string failureMessage;

    private TMP_InputField inputField;

    public string Value { get; private set; }

    public bool IsValid => CheckValidty();

    private void Awake()
    {
        defaultLabel = label.text;
        failureMessage = "";
        inputField = GetComponent<TMP_InputField>();
    }

    private void Update()
    {
        if (failureMessage != "")
        {
            label.text = failureMessage;
            label.color = Color.red;
        }
        else
        {
            label.text = defaultLabel;
            label.color = Color.white;
        }
    }
    private bool CheckEmailValidity()
    {
        if (inputField.text.Length >= 6)
        {
            Value = inputField.text;
            failureMessage = "";
            return true;
        }
        failureMessage = "Email invalid";
        return false;
    }

    private bool CheckPasswordMatch()
    {
        if (password.text == inputField.text)
        {
            failureMessage = "";
            return true;
        }
        failureMessage = "Passwords dont match";
        return false;
    }

    private bool CheckPasswordValidity()
    {

        if (inputField.text.Length >= 6)
        {
            Value = inputField.text;
            failureMessage = "";
            return true;
        }
        failureMessage = "Short Password";
        return false;

    }
    private bool CheckValidty()
    {
        var isValid = true;
        switch (myInput)
        {
            case InputType.ConfirmPassword:
                isValid = CheckPasswordMatch();
                break;
            case InputType.Email:
                isValid = CheckEmailValidity();
                break;
            case InputType.Password:
                isValid = CheckPasswordValidity();
                break;
        }
        return isValid;
    }

    private enum InputType
    {
        Password,
        ConfirmPassword,
        Email
    }
}
