using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Serialization;


public class DatabaseManager : MonoBehaviour
{
    [FormerlySerializedAs("savedMoviesManager")] [SerializeField] SavedMoviesPanel savedMoviesPanel;
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
            savedMoviesPanel.SetMovies(new List<Movie>());
        }
        else
        {
            Debug.Log("Loading movies...");
            var tempList = new List<Movie>();
            DataSnapshot snapshot = DBCheckUserExists.Result;
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                var tempSnapshot = childSnapshot.Value as IDictionary;
                if(tempSnapshot!=null)
                {
                    Debug.Log($"Title-Null?: {tempSnapshot["title"]==null}");
                    Debug.Log($"ImageUrl-Null?: {tempSnapshot["imageUrl"]==null}");
                    Debug.Log($"ReleaseDate-Null?: {tempSnapshot["releaseDate"]==null}");
                    Debug.Log($"Seen-Null?: {tempSnapshot["seen"]==null}");
                    
                    tempList.Add(new Movie(
                        tempSnapshot["title"].ToString(),
                        tempSnapshot["imageUrl"].ToString(),
                        tempSnapshot["backdropUrl"].ToString(),
                        tempSnapshot["releaseDate"].ToString(),
                        (bool)tempSnapshot["seen"],
                        Double.Parse(tempSnapshot["voteAverage"].ToString()),
                        int.Parse( tempSnapshot["votesCount"].ToString()),
                        tempSnapshot["description"].ToString()
                        ));
                   
                }
                else
                {
                    Debug.Log($"Null TempSnapshot: {tempSnapshot}");
                }
            }
            Debug.Log("User is already in database !");
            savedMoviesPanel.SetMovies(tempList);
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
        yield return SetBackdropUrl(movie.Title,movie.BackdropUrl);
        yield return SetReleaseDate(movie.Title,movie.ReleaseDate);
        yield return SetSeen(movie.Title, movie.Seen);
        yield return SetDescription(movie.Title, movie.Description);
        yield return SetAuthor(movie.Title, movie.Author);
        yield return SetNote(movie.Title, movie.Note);
        yield return SetVoteAverage(movie.Title, movie.VoteAverage);
        yield return SetVoteCount(movie.Title, movie.VotesCount);
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
    
    IEnumerator SetBackdropUrl(string title, string imageUrl)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("backdropUrl").SetValueAsync(imageUrl);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetReleaseDate(string title,string releaseDate)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("releaseDate").SetValueAsync(releaseDate);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetSeen(string title, bool seen)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("seen").SetValueAsync(seen);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    
    IEnumerator SetDescription(string title, string description)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("description").SetValueAsync(description);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    IEnumerator SetAuthor(string title, string author)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("author").SetValueAsync(author);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    IEnumerator SetVoteAverage(string title, double votesAverage)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("voteAverage").SetValueAsync(votesAverage);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    IEnumerator SetVoteCount(string title, int votesCount)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("votesCount").SetValueAsync(votesCount);
        return new WaitUntil(predicate: () => DBTask.IsCompleted);
    }
    IEnumerator SetNote(string title, string note)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("note").SetValueAsync(note);
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