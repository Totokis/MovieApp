using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PagedTextField : MonoBehaviour, IPointerClickHandler
{ 
    TMP_Text text;
    int numberOfPages;
    int pageIndex;

    void Awake()
    {
        text = GetComponent<TMP_Text>();
        pageIndex = 1;
    }
    
    void OnGUI()
    {
        numberOfPages = text.textInfo.pageCount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pageIndex == numberOfPages)
        {
            pageIndex = 1;
            text.pageToDisplay = pageIndex;
        }
        else
        {
            pageIndex++;
            text.pageToDisplay = pageIndex;
        }
    }
}
