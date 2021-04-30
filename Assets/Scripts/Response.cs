using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
[System.Serializable]
public class Result
{
    public bool adult;
    public string backdrop_path;
    public List<int> genre_ids;
    public int id;
    public string original_language;
    public string original_title;
    public string overview;
    public double popularity;
    public string poster_path;
    public string release_date;
    public string title;
    public bool video;
    public double vote_average;
    public int vote_count; 
}

public class Root
{
    public int page;
    public List<Result> results;
    public int total_pages;
    public int total_results;
}



