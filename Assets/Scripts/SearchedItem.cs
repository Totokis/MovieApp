using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

class SearchedItem : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text releaseDate;
    [SerializeField] Image image;
    [SerializeField] Sprite deafulSprite;

    SearchedMovie _movie;
    Texture2D _texture;
    float _clicked = 0;
    float _clicktime = 0;
    float _clickdelay = 0.5f;
    
    IEnumerator GetImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_movie.imageUrl))
        {
             yield return uwr.SendWebRequest();
             if (uwr.result != UnityWebRequest.Result.Success)
             {
                 _texture = deafulSprite.texture;
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
        image.sprite = Sprite.Create(_texture,new Rect(0f,0f,_texture.width,_texture.height),Vector2.zero,10f);
    }
    
    public void SetMovie(SearchedMovie movie)
    {
        _movie = movie;
        title.text = _movie.title;
        releaseDate.text = _movie.releaseDate;
        StartCoroutine(GetImage());
    }
    public void OnSelect(BaseEventData eventData)
    {
    }

    public void OnClickEvent()
    {
        Debug.Log("Clicked");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Debug.Log("OnPointerEnter");
        //image.materialForRendering.SetFloat("_PixelateAmount",0f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // Debug.Log("OnPointerExit");
        //image.materialForRendering.SetFloat("_PixelateAmount",0.8f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        /*
         * Code from here:
         * https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
         */
        _clicked++;
        if (_clicked == 1)
        {
            _clicktime = Time.time;
        }
        
        if (_clicked > 1 && Time.time - _clicktime < _clickdelay)
        {
            _clicked = 0;
            _clicktime = 0;
            Debug.Log("Double CLick: "+this.GetComponent<RectTransform>().name);
            FindObjectOfType<GUIManager>().AddedPopUp.gameObject.SetActive(true);
            AddSelectedItem();

        }
        else if (_clicked > 2 || Time.time - _clicktime > 1)
        {
            Debug.Log("One CLick: "+this.GetComponent<RectTransform>().name);
            _clicked = 0;
        }
    }
    void AddSelectedItem()
    {
        FindObjectOfType<DatabaseManager>().SaveMovie(new Movie(_movie.title,_movie.imageUrl,_movie.releaseDate,false));
    }
}
