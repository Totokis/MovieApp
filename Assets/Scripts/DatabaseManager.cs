using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    FirebaseAuth _auth;
    FirebaseUser _user;
    DatabaseReference _databaseReference;
    List<Movie> _movies = new List<Movie>();
    SavedMoviesManager _savedMoviesManager;
    
    void Awake()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _user = _auth.CurrentUser;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        _movies = new List<Movie>();
    }
    void Start()
    {
        StartCoroutine(LoadUserData());
    }

    public void SaveMovie()
    {
        Movie movie = new Movie("Shrek", "google.com/shrek", "Kocham Julke", false);
        StartCoroutine(SaveUserData(movie));
    }
    
    IEnumerator SaveUserData(Movie movie)
    {
        var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").SetValueAsync(movie.Title);
        yield return SetTitle(movie.Title);
        yield return SetImageUrl(movie.Title,movie.ImageUrl);
        yield return SetDescription(movie.Title,movie.Description);
        yield return SetSeen(movie.Title, movie.Seen);
        Debug.Log("Saved data");
    }
    
    IEnumerator SetTitle(string title)
    {
        var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").Child(title).Child("title").SetValueAsync(title);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetImageUrl(string title, string imageUrl)
    {
        var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").Child(title).Child("imageUrl").SetValueAsync(imageUrl);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetDescription(string title,string description)
    {
        var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").Child(title).Child("description").SetValueAsync(description);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetSeen(string title, bool seen)
    {
        var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").Child(title).Child("seen").SetValueAsync(seen);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }

    IEnumerator LoadUserData()
    {
        var DBCheckUserExists = _databaseReference.Child("users").Child(_user.UserId).Child("movies").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBCheckUserExists.IsCompleted);

        if (DBCheckUserExists.Exception != null)
        {
            Debug.LogWarning(message:$"Failed to register task with exception: {DBCheckUserExists.Exception}");
        }
        else if (DBCheckUserExists.Result.Value == null)
        {
            Debug.Log("There is no user data");
            _movies.Clear();
            _savedMoviesManager.SetMovies(_movies);
        }
        else
        {
            DataSnapshot snapshot = DBCheckUserExists.Result;
            _movies.Clear();
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                var temp = new Movie(
                    childSnapshot.Child("title").Value.ToString(),
                    childSnapshot.Child("imageURL").Value.ToString(),
                    childSnapshot.Child("description").Value.ToString(),
                    Boolean.Parse(childSnapshot.Child("seen").Value.ToString())
                    );
                _movies.Add(temp);
            }
            Debug.Log("User is already in database !");
            _savedMoviesManager.SetMovies(_movies);
        }
    }
    
}