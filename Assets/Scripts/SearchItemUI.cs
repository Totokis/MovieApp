using Lean.Gui;
using UnityEngine;

public class SearchItemUI : MonoBehaviour
{
    LeanShake leanShake;
    SearchedItem searchedItem;
    void Awake()
    {
        leanShake = GetComponent<LeanShake>();
        searchedItem = GetComponent<SearchedItem>();
        searchedItem.onDoubleClick.AddListener(()=>leanShake.Shake(10f));
    }

}
