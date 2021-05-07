using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
   [SerializeField] UpdateMoviePanel updateMoviePanel;
   [SerializeField] GameObject savedMoviePanel;
   [SerializeField] AddedPopUp addedPopUp;

   public UpdateMoviePanel UpdateMoviePanel => updateMoviePanel;

   public GameObject SavedMoviePanel => savedMoviePanel;
   
   public AddedPopUp AddedPopUp => addedPopUp;

}
