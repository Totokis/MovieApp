using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageWithUrl : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image image;
    [SerializeField] TMP_InputField input;
    [SerializeField] bool editable = true;
    bool selected;
    string url = "";

    void Awake()
    {
        input.gameObject.SetActive(false);
        input.onSubmit.AddListener(inputText => SetUrl(inputText, this));
        if (image == null)
            image = GetComponent<Image>();
        if (image == null)
            throw new Exception("There is no image component");
    }


    void UrlChanged()
    {
        LoadImage();
    }

    void LoadImage()
    {
        try
        {
            TextureBase.Instance.AddToQueue(url, image);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Exception {e}");
            throw;
        }
    }
    public void SetUrl(string arg0, ImageWithUrl caller)
    {
        if (caller.selected)
        {
            if (!url.Equals(arg0))
            {
                url = arg0;
                UrlChanged();
            }
            input.gameObject.SetActive(false);
            selected = false;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (editable)
        {
            selected = true;
            input.gameObject.SetActive(true);
            input.text = url;

        }
    }

    public string GetUrl()
    {
        return url ??= "";
    }
}
