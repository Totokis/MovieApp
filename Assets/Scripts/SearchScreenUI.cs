using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;
using UnityEngine.Events;

public class SearchScreenUI : MonoBehaviour
{
  [SerializeField] SearchScreen searchScreen;
  [SerializeField] LeanButton addNewMovieButton;
  
  void Awake()
  { 
    addNewMovieButton.OnClick.AddListener(searchScreen.AddNewMovie);
  }
}
