using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PagedTextField : MonoBehaviour, IPointerClickHandler
{
    private int numberOfPages;
    private int pageIndex;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        pageIndex = 1;
    }

    private void OnGUI()
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
