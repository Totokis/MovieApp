using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

class SearchedItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text releaseDate;
    [SerializeField] TMP_Text votes;
    [SerializeField] Image image;
    [SerializeField] Sprite deafulSprite;
    [SerializeField] Slider slider;

    SearchedMovie movie;
    Texture2D texture;
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
    
    public UnityEvent onDoubleClick;

    void Awake()
    {
        onDoubleClick = new UnityEvent();
    }
    
    void AddSelectedItem()
    {
        FindObjectOfType<DatabaseManager>().SaveMovie(new Movie(movie.title,movie.imageUrl,movie.backdropImageUrl,movie.releaseDate,false,movie.voteAverage,movie.votesCount,movie.description));
    }
    
    public void SetMovie(SearchedMovie movie)
    {
        this.movie = movie;
        title.text = this.movie.title;
        releaseDate.text = this.movie.releaseDate;
        slider.value =  (float)(movie.voteAverage/10f);
        votes.text = $"{FormatNumber(movie.votesCount)} reviews";
        TextureBase.Instance.AddToQueue(movie.imageUrl,image);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        /*
         * Code from here:
         * https://forum.unity.com/threads/detect-double-click-on-something-what-is-the-best-way.476759/
         */
        clicked++;
        if (clicked == 1)
        {
            clicktime = Time.time;
        }
        
        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            clicked = 0;
            clicktime = 0;
            Debug.Log("Double CLick: "+this.GetComponent<RectTransform>().name);
            AddSelectedItem();
            onDoubleClick.Invoke();

        }
        else if (clicked > 2 || Time.time - clicktime > 1)
        {
            Debug.Log("One CLick: "+this.GetComponent<RectTransform>().name);
            clicked = 0;
        }
    }
    
    static string FormatNumber(int num) {
        if (num >= 100000)
            return FormatNumber(num / 1000) + "K";
        if (num >= 10000) {
            return (num / 1000D).ToString("0.#") + "K";
        }
        return num.ToString("#,0");
    }
}
