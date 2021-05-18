using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;

public class SettingsScreen : MonoBehaviour
{
  [SerializeField] LeanButton instructionButton;
  [SerializeField] LeanWindow instructionWindow;

  private void Awake()
  {
    instructionButton.OnClick.AddListener(ShowInstruction);
  }
  private void ShowInstruction()
  {
    instructionWindow.On = true;
  }
}
