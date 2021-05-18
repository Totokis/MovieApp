using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [SerializeField] private Selectable firstInput;
    [SerializeField] private Button submitButton;

    private EventSystem _system;
    private TouchScreenKeyboard mobileKeyboard;


    private void Awake()
    {
        _system = EventSystem.current;
        if (firstInput == null)
            firstInput = GetComponentInChildren<Selectable>();
        firstInput.Select();
        mobileKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.EmailAddress);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            CheckCurrentInput();
            SelectUpperInput();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            CheckCurrentInput();
            SelectDownInput();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || mobileKeyboard.status == TouchScreenKeyboard.Status.Done)
        {
            var currentButton = _system.currentSelectedGameObject.GetComponent<Button>();
            if (currentButton == submitButton)
            {
                //submitButton.onClick.Invoke();
            }
            else
            {
                CheckCurrentInput();
                SelectDownInput();
            }
        }
    }
    private void SelectDownInput()
    {

        var next = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
        if (next != null)
            next.Select();
    }
    private void SelectUpperInput()
    {
        var previous = _system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
        if (previous != null)
            previous.Select();
    }
    private void CheckCurrentInput()
    {

        if (_system.currentSelectedGameObject == null)
            firstInput.Select();

    }
    public void ClearInput()
    {
        foreach (var inputField in GetComponentsInChildren<TMP_InputField>())
            inputField.text = "";
    }
}
