using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

class SearchedItem : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] TMP_Text text;
    [SerializeField] TMP_Text releaseDate;
    [SerializeField] Sprite deafulSprite;
    [SerializeField] Image image;
    [SerializeField] Shader shader; 
    [SerializeField] Material newMaterial;

    SearchedMovie _movie;
    Sprite _spriteFromRequest; 
    Texture _texture;
    float _clicked = 0;
    float _clicktime = 0;
    float _clickdelay = 0.5f;
    
    void Awake()
    {
        newMaterial = new Material(shader);
    }
    
    IEnumerator GetImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_movie.imageUrl))
        {
             yield return uwr.SendWebRequest();
             if (uwr.result != UnityWebRequest.Result.Success)
             {
                 _spriteFromRequest = deafulSprite;
             }
             else
             {
                 _texture = DownloadHandlerTexture.GetContent(uwr);
                 //spriteFromRequest = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 10f);
             }
        }
        SetImage();
    }
    
    void SetImage()
    {
        newMaterial.SetTexture("_MainTexture",_texture);
        newMaterial.SetFloat("_PixelateAmount",0.8f);
        image.material = newMaterial;
    }
    
    public void SetMovie(SearchedMovie movie)
    {
        _movie = movie;
        text.text = _movie.title;
        releaseDate.text = _movie.releaseDate;
        StartCoroutine(GetImage());
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
    public void OnPointerDown(PointerEventData eventData)
    {
        /*
         * Code from here:
         * https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
         */
        _clicked++;
        if (_clicked == 1) _clicktime = Time.time;
 
        if (_clicked > 1 && Time.time - _clicktime < _clickdelay)
        {
            _clicked = 0;
            _clicktime = 0;
            Debug.Log("Double CLick: "+this.GetComponent<RectTransform>().name);
 
        }
        else if (_clicked > 2 || Time.time - _clicktime > 1)
        {
            _clicked = 0;
            Debug.Log("One CLick: "+this.GetComponent<RectTransform>().name);
        }
    }
}
