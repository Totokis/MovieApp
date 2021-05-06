using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMoviePanel : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMP_InputField titleInputField;
    [SerializeField] TMP_InputField releaseDateInputField;
    [SerializeField] TMP_InputField authorInputField;
    [SerializeField] TMP_InputField descriptionInputField;
    [SerializeField] TMP_InputField noteInputField;

    [Header("Button")]
    [SerializeField] Button doneButton;

    public void EditDone()
    {
        gameObject.SetActive(false);
        FindObjectOfType<GUIManager>().SavedMoviePanel.gameObject.SetActive(true);
    }

    public void SetActive()
    {
        
    }
    
}
