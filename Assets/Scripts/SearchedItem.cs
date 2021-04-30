using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;

class SearchedItem : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_Text _releaseDate;
    [SerializeField] Sprite _deafulSprite;
    [SerializeField] Image image;
    [SerializeField] Shader _shader; 
    [SerializeField] Material _newMaterial;

    SearchedMovie _movie;
    Sprite spriteFromRequest; 
    Texture texture;
    private void Awake()
    {
        _newMaterial = new Material(_shader);
    }
    public void SetMovie(SearchedMovie movie)
    {
        _movie = movie;
        _text.text = _movie.title;
        _releaseDate.text = _movie.releaseDate;
        StartCoroutine(GetImage());
    }
    IEnumerator GetImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_movie.imageUrl))
        {
             yield return uwr.SendWebRequest();
             if (uwr.result != UnityWebRequest.Result.Success)
             {
                 spriteFromRequest = _deafulSprite;
             }
             else
             {
                 texture = DownloadHandlerTexture.GetContent(uwr);
                 //spriteFromRequest = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 10f);
             }
        }
        SetImage();
    }
    void SetImage()
    {
        _newMaterial.SetTexture("_MainTexture",texture);
        _newMaterial.SetFloat("_PixelateAmount",0.8f);
        image.material = _newMaterial;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        //image.material.SetFloat("_PixelateAmount",0f);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("OnPointerEnter");
        image.materialForRendering.SetFloat("_PixelateAmount",0f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        image.materialForRendering.SetFloat("_PixelateAmount",0.8f);
    }
}
