using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class SearchListPanel : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] SearchedItem listItemPrefab;
    [SerializeField] GameObject message;

    public void SearchMovie()
    {
        DestroyChildren();
        GetListOfMovies();
    }
   
    void DestroyChildren()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        message.SetActive(true);
    }

    void GetListOfMovies()
    {
        if (inputField.text != "")
        {
            message.SetActive(false);
            List<Result> results = APIHelper.GetNewResult(inputField.text);
            foreach (var result in results)
            {
                SearchedMovie searchedMovie = new SearchedMovie { 
                    title = result.title, 
                    imageUrl = "https://image.tmdb.org/t/p/w500"+result.poster_path,
                    releaseDate = result.release_date
                };
                var listElement = Instantiate(listItemPrefab,transform);
                listElement.GetComponent<SearchedItem>().SetMovie(searchedMovie);
            }
        }
        else
        {
            message.SetActive(true);
        }
    }
}