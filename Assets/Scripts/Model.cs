using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private List<Movie> myList;

    Movie GetMovie(int index)
    {
        if (myList[index] == null)
        {
            return new Movie();
        }
        return myList[index];
    }
}

public class GetMyData
{
    
}