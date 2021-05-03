using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Serialization;

public class DatabaseManager : MonoBehaviour
{
    [SerializeField] SavedMoviesManager savedMoviesManager;
    FirebaseAuth _auth;
    FirebaseUser _user;
    DatabaseReference _databaseReference;
    List<Movie> _movies;

    void Awake()
    {
        _auth = FirebaseAuth.DefaultInstance;
        _user = _auth.CurrentUser;
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
        _movies = new List<Movie>();
        // _databaseReference.ChildAdded += HandleChildAdded;
        // _databaseReference.ValueChanged += HandleValueChanged;
        StartCoroutine(LoadUserData());
    }

    // void OnDestroy()
    // {
    //     _databaseReference.ChildAdded -= HandleChildAdded;
    //     _databaseReference.ValueChanged -= HandleValueChanged;
    // }
    // void HandleValueChanged(object sender, ValueChangedEventArgs e)
    // {
    //     Debug.Log("HandleValueChanged");
    //     StartCoroutine(LoadUserData());
    // }
    // void HandleChildAdded(object sender, ChildChangedEventArgs e)
    // {
    //     Debug.Log("HandleChildAdded");
    //     StartCoroutine(LoadUserData());
    // }
    
    public void SaveMovie(Movie movie)
    {
        StartCoroutine(SaveAndReloadDatabase(movie));
    }

    IEnumerator SaveAndReloadDatabase(Movie movie)
    {
        yield return SaveUserData(movie);
        yield return LoadUserData();
    }
    
    
    
    IEnumerator SaveUserData(Movie movie)
    {
        //var DBTask = _databaseReference.Child("users").Child(_user.UserId).Child("movies").SetValueAsync(movie.Title);
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
            savedMoviesManager.SetMovies(new List<Movie>());
        }
        else
        {
            var tempList = new List<Movie>();
            var movie = new Movie();
            DataSnapshot snapshot = DBCheckUserExists.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                var tempSnapshot = childSnapshot.Value as IDictionary;
                tempList.Add(new Movie( tempSnapshot["title"].ToString(),
                    tempSnapshot["imageUrl"].ToString(),
                    tempSnapshot["description"].ToString(),
                    (bool)tempSnapshot["seen"]
                    ));
                Debug.Log($"Movie {tempSnapshot["title"]} ");
            }
            Debug.Log("User is already in database !");
            savedMoviesManager.SetMovies(tempList);
        }
    }

    public void UpdateOneMovie(Movie movie, SavedMovieItem item)
    {
        StartCoroutine(SaveAndGetOneMovie(movie, item));
    }
    IEnumerator SaveAndGetOneMovie(Movie movie,SavedMovieItem item)
    {
        yield return SaveUserData(movie);
        
        var DBGetMovie = _databaseReference.Child("users").Child(_user.UserId).Child("movies").Child(movie.Title).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBGetMovie.IsCompleted);
        if (DBGetMovie.Exception != null)
        {
            Debug.LogWarning(message:$"Failed to register task with exception: {DBGetMovie.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBGetMovie.Result;
            Debug.Log(snapshot.Child("title"));
            Debug.Log(snapshot.Child("seen"));
        }
        
    }
}