using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputFieldCleaner : MonoBehaviour
{
    public void ClearInput()
    {
        GetComponent<TMP_InputField>().text = "";
    }
}
