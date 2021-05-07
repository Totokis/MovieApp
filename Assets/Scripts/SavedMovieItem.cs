using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SavedMovieItem : MonoBehaviour
{
    [SerializeField] TMP_Text title;
    [SerializeField] Image image;
    [SerializeField] Toggle seen;
    [SerializeField] Sprite deafultSprite;
    Movie movie;
    Texture2D texture;
    bool imageIsNotSet;
    
    void OnEnable()
    {
        if (imageIsNotSet)
        {
            StartCoroutine(GetImage());
        }
    }
    
    void SetUI()
    {
        title.text = movie.Title;
        seen.isOn = movie.Seen;
        if (gameObject.activeInHierarchy)
        {
            Debug.Log("activeInHierarchy: true");
            StartCoroutine(GetImage());
        }
        else
        {
            Debug.Log("activeInHierarchy: false");
            imageIsNotSet = true;
        }
    }

    IEnumerator GetImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(movie.ImageUrl))
        {
            Debug.Log("Inside Get Image");
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                texture = deafultSprite.texture;
                Debug.Log("Setted default texture");
            }
            else
            {
                texture = DownloadHandlerTexture.GetContent(uwr); 
                Debug.Log("Setted downloaded texture");
            }
        }
        SetImage();
    }
    
    void SetImage()
    {
        image.sprite = Sprite.Create(texture,new Rect(0f,0f,texture.width,texture.height),Vector2.zero,10f);
        movie.SetSprite(image.sprite);
    }
    
    void UpdateValues()
    {
        Debug.Log("UpdateValues");
        this.movie = new Movie(
            movie.Title,
            movie.ImageUrl,
            movie.ReleaseDate,
            seen.isOn,
            movie.Description,
            movie.Author,
            movie.Note
        );
        FindObjectOfType<DatabaseManager>().UpdateOneMovie(this.movie);
    }

    public void EditValues()
    {
        FindObjectOfType<GUIManager>().UpdateMoviePanel.LoadMovieToEdit(movie,this);
        FindObjectOfType<GUIManager>().SavedMoviePanel.gameObject.SetActive(false);
    }
    
    public void DeleteRecord()
    {
        FindObjectOfType<DatabaseManager>().DeleteOneMovie(movie, this);
    }
    
    public void SetMovie(Movie movie)
    {
        this.movie = movie;
        SetUI();
    }
    
    public void UpdateMovie(Movie movie)
    {
        this.movie = movie;
        UpdateValues();
        SetUI();
    }

    public void UpdateSeen()
    {
        UpdateValues();
    }
    
}
