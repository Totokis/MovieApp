using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using Lean.Transition.Method;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
   [SerializeField] UpdateMoviePanel updateMoviePanel;
   [SerializeField] DetailMoviePanel detailMoviePanel;
   [SerializeField] GameObject savedMoviePanel;
   [SerializeField] List<LeanButton> buttons;
   [SerializeField] List<GameObject> screens;

   public UpdateMoviePanel UpdateMoviePanel => updateMoviePanel;
   
   public DetailMoviePanel DetailMoviePanel => detailMoviePanel;

   public GameObject SavedMoviePanel => savedMoviePanel;
   

   void Awake()
   {
      AddListeners();
      screens.Add(detailMoviePanel.gameObject);
      screens.Add(updateMoviePanel.gameObject);
   }
   void Start()
   {
      SelectWelcomePage();
   }
   void AddListeners()
   {
      buttons[0].OnClick.AddListener(() => SwitchToScreen(screens[0]));
      buttons[1].OnClick.AddListener(() => SwitchToScreen(screens[1]));
      buttons[2].OnClick.AddListener(() => SwitchToScreen(screens[2]));
      buttons[3].OnClick.AddListener(() => SwitchToScreen(screens[3]));
   }
   void SwitchToScreen(GameObject screenToSwitch)
   {
      foreach (var screen in screens)
      {
         screen.SetActive(false);
      }
      screenToSwitch.SetActive(true);
   }
   void SelectWelcomePage()
   {
      buttons[1].OnClick.Invoke();
   }
}
