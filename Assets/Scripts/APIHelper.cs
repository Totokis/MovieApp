using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public static class APIHelper
{
    public static List<Result> GetNewResult(string title = "Shrek")
    {
        //TODO Get API from firebase;
        var request = WebRequest.Create(
            $"https://api.themoviedb.org/3/search/movie?api_key=61f3437d009eac7fd9c283bc9bf1b3ad&language=en-US&query={title}&page=1&include_adult=false") as HttpWebRequest;
        var response = request.GetResponse() as HttpWebResponse;
        var reader = new StreamReader(response.GetResponseStream());
        var json = reader.ReadToEnd();
        var myDeserializedClass = JsonUtility.FromJson<Root>(json);
        return myDeserializedClass.results;
    }
}
