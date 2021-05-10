using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using Lean.Transition.Method;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
   [SerializeField] UpdateMoviePanel updateMoviePanel;
   [SerializeField] GameObject savedMoviePanel;
   [SerializeField] AddedPopUp addedPopUp;
   [SerializeField] List<LeanButton> buttons;
   [SerializeField] List<RectTransform> screens;

   public UpdateMoviePanel UpdateMoviePanel => updateMoviePanel;

   public GameObject SavedMoviePanel => savedMoviePanel;
   
   public AddedPopUp AddedPopUp => addedPopUp;

   private void Awake()
   {
      buttons[0].OnClick.AddListener(()=>SignOutAction(screens[0]));
      buttons[1].OnClick.AddListener(()=>SearchMovieAction(screens[1]));
      buttons[2].OnClick.AddListener(()=>ShowMovieListAction(screens[2]));
      buttons[3].OnClick.AddListener(()=>SettingsAction(screens[3]));
   }
   private void SettingsAction(RectTransform screen)
   {
      SwitchToScreen(screen);
   }
   private void ShowMovieListAction(RectTransform screen)
   {
      SwitchToScreen(screen);
   }
   private void SearchMovieAction(RectTransform screen)
   {
      SwitchToScreen(screen);
   }
   private void SignOutAction(RectTransform screen)
   {
      SwitchToScreen(screen);
   }
   private void SwitchToScreen(RectTransform screenToSwitch)
   {
      foreach (var screen in screens)
      {
         screen.gameObject.SetActive(false);
      }
      screenToSwitch.gameObject.SetActive(true);
   }
   void Start()
   {
      SelectWelcomePage();
   }
   void SelectWelcomePage()
   {
      buttons[1].OnClick.Invoke();
   }
}
