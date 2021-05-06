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
    FirebaseAuth auth;
    FirebaseUser user;
    DatabaseReference databaseReference;
    
    void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    void Start()
    {
        StartCoroutine(LoadUserData());
    }
    
    
    IEnumerator LoadUserData()
    {
        var DBCheckUserExists = databaseReference.Child("users").Child(user.UserId).Child("movies").GetValueAsync();
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
    
    IEnumerator SaveOneMovie(Movie movie)
    {
        yield return SaveUserData(movie);
    }
    
    IEnumerator SaveAndReloadDatabase(Movie movie)
    {
        yield return SaveUserData(movie);
        yield return LoadUserData();
    }
    
    IEnumerator SaveUserData(Movie movie)
    {
        yield return SetTitle(movie.Title);
        yield return SetImageUrl(movie.Title,movie.ImageUrl);
        yield return SetDescription(movie.Title,movie.ReleaseDate);
        yield return SetSeen(movie.Title, movie.Seen);
        Debug.Log("Saved data");
    }
    
    IEnumerator SetTitle(string title)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("title").SetValueAsync(title);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetImageUrl(string title, string imageUrl)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("imageUrl").SetValueAsync(imageUrl);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetDescription(string title,string description)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("description").SetValueAsync(description);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetSeen(string title, bool seen)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("seen").SetValueAsync(seen);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator DeleteRecord(Movie movie, SavedMovieItem savedMovieItem)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(movie.Title).RemoveValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);
        
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message:$"Failed to register task with exception: {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Item deleted!");
            Destroy(savedMovieItem.gameObject);
        }
    }
    
    public void SaveMovie(Movie movie)
    {
        StartCoroutine(SaveAndReloadDatabase(movie));
    }
    
    public void UpdateOneMovie(Movie movie)
    {
        StartCoroutine(SaveOneMovie(movie));
    }
    
    public void DeleteOneMovie(Movie movie, SavedMovieItem savedMovieItem)
    {
        StartCoroutine(DeleteRecord(movie, savedMovieItem));
    }
}