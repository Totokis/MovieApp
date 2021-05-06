using System;
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
    Movie _movie;
    Texture2D _texture;
    bool _imageIsNotSet;
    bool _loaded;
    
    public void SetMovie(Movie movie)
    {
        _movie = movie;
        title.text = movie.Title;
        seen.isOn = movie.Seen;
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(GetImage());
        }
        else
        { 
            _imageIsNotSet = true;
        }
        _loaded = true;
    }

    void OnEnable()
    {
        if (_imageIsNotSet)
        {
            StartCoroutine(GetImage());
        }
    }

    IEnumerator GetImage()
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_movie.ImageUrl))
        {
            yield return uwr.SendWebRequest();
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                _texture = deafultSprite.texture;
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
        _loaded = true;
    }

    public void UpdateValues()
    {
        Debug.Log("UpdateValues");
        FindObjectOfType<DatabaseManager>().UpdateOneMovie(
            new Movie(
            _movie.Title, 
            _movie.ImageUrl, 
            _movie.ReleaseDate, 
            seen.isOn
        ));
    }

    public void EditValues()
    {
        FindObjectOfType<GUIManager>().UpdateMoviePanel.gameObject.SetActive(true);
        FindObjectOfType<GUIManager>().SavedMoviePanel.gameObject.SetActive(false);
    }
    

    public void DeleteRecord()
    {
        FindObjectOfType<DatabaseManager>().DeleteOneMovie(_movie, this);
    }
}
