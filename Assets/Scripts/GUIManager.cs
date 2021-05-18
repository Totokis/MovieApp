using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private UpdateMoviePanel updateMoviePanel;
    [SerializeField] private DetailMoviePanel detailMoviePanel;
    [SerializeField] private GameObject savedMoviePanel;
    [SerializeField] private List<LeanButton> buttons;
    [SerializeField] private List<GameObject> screens;

    public UpdateMoviePanel UpdateMoviePanel => updateMoviePanel;

    public DetailMoviePanel DetailMoviePanel => detailMoviePanel;
    


    private void Awake()
    {
        AddListeners();
        screens.Add(detailMoviePanel.gameObject);
        screens.Add(updateMoviePanel.gameObject);
    }
    private void Start()
    {
        SelectWelcomePage();
    }
    private void AddListeners()
    {
        buttons[0].OnClick.AddListener(() => SwitchToScreen(screens[0]));
        buttons[1].OnClick.AddListener(() => SwitchToScreen(screens[1]));
        buttons[2].OnClick.AddListener(() => SwitchToScreen(screens[2]));
        buttons[3].OnClick.AddListener(() => SwitchToScreen(screens[3]));
    }
    private void SwitchToScreen(GameObject screenToSwitch)
    {
        foreach (var screen in screens)
            screen.SetActive(false);
        screenToSwitch.SetActive(true);
    }
    private void SelectWelcomePage()
    {
        buttons[1].OnClick.Invoke();
    }
}
