
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public static class APIHelper
{
   public static List<Result> GetNewResult(string title="Shrek")
   {
      HttpWebRequest request = WebRequest.Create($"https://api.themoviedb.org/3/search/movie?api_key=61f3437d009eac7fd9c283bc9bf1b3ad&language=en-US&query={title}&page=1&include_adult=false") as HttpWebRequest;
      HttpWebResponse response = request.GetResponse() as HttpWebResponse;
      StreamReader reader = new StreamReader(response.GetResponseStream());
      string json = reader.ReadToEnd();
      Root myDeserializedClass = JsonUtility.FromJson<Root>(json);
      return myDeserializedClass.results;
   } 
}
