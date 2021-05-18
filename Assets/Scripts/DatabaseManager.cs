using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DatabaseManager : MonoBehaviour
{ 
    [SerializeField] SavedMoviesPanel savedMoviesPanel;
   FirebaseAuth auth;
   DatabaseReference databaseReference;
   FirebaseUser user;

    public static DatabaseManager Instance { get; private set; }
    

    private void Awake()
    {
        if (savedMoviesPanel == null)
        {
            savedMoviesPanel = FindObjectOfType<SavedMoviesPanel>();
        }
        if (Instance == null)
        {
            Instance = this;
            InitializeFirebaseDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        if (savedMoviesPanel == null)
        {
            savedMoviesPanel = FindObjectOfType<SavedMoviesPanel>();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadUserData());
    }

    private void InitializeFirebaseDatabase()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private IEnumerator LoadUserData()
    {
        var dbCheckDataExists = databaseReference.Child("users").Child(user.UserId).Child("movies").GetValueAsync();
        yield return new WaitUntil(() => dbCheckDataExists.IsCompleted);

        if (dbCheckDataExists.Exception != null)
        {
            Debug.LogWarning($"Failed to register task with exception: {dbCheckDataExists.Exception}");
        }
        else if (dbCheckDataExists.Result.Value == null)
        {
            Debug.Log("There is no user data");
            savedMoviesPanel.SetMovies(new List<Movie>());
        }
        else
        {
            Debug.Log("Loading movies...");
            var tempList = new List<Movie>();
            var snapshot = dbCheckDataExists.Result;
            foreach (var childSnapshot in snapshot.Children)
            {
                var tempSnapshot = childSnapshot.Value as IDictionary;
                Debug.Log($"TITLE: {tempSnapshot["title"].ToString()}");
                if (tempSnapshot != null)
                    tempList.Add(new Movie(
                        tempSnapshot["title"].ToString(),
                        tempSnapshot["imageUrl"].ToString(),
                        tempSnapshot["backdropUrl"].ToString(),
                        tempSnapshot["releaseDate"].ToString(),
                        (bool)tempSnapshot["seen"],
                        double.Parse(tempSnapshot["voteAverage"].ToString()),
                        int.Parse(tempSnapshot["votesCount"].ToString()),
                        tempSnapshot["description"].ToString()
                    ));
                else
                    Debug.Log($"Null TempSnapshot: {tempSnapshot}");
            }
            savedMoviesPanel.SetMovies(tempList);
        }
    }

    private IEnumerator SaveOneMovie(Movie movie)
    {
        yield return SaveUserData(movie);
    }

    private IEnumerator SaveAndReloadDatabase(Movie movie)
    {
        yield return SaveUserData(movie);
        yield return LoadUserData();
    }

    private IEnumerator SaveUserData(Movie movie)
    {
        var title = Regex.Replace(movie.Title, @"[^\w\.@-\\% ]","");
        yield return SetTitle(title);
        yield return SetImageUrl(title, movie.ImageUrl);
        yield return SetBackdropUrl(title, movie.BackdropUrl);
        yield return SetReleaseDate(title, movie.ReleaseDate);
        yield return SetSeen(title, movie.Seen);
        yield return SetDescription(title, movie.Description);
        yield return SetAuthor(title, movie.Author);
        yield return SetNote(title, movie.Note);
        yield return SetVoteAverage(title, movie.VoteAverage);
        yield return SetVoteCount(title, movie.VotesCount);
    }

    private IEnumerator SetTitle(string title)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("title").SetValueAsync(title);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator SetImageUrl(string title, string imageUrl)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("imageUrl").SetValueAsync(imageUrl);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator SetBackdropUrl(string title, string imageUrl)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("backdropUrl").SetValueAsync(imageUrl);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator SetReleaseDate(string title, string releaseDate)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("releaseDate").SetValueAsync(releaseDate);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator SetSeen(string title, bool seen)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("seen").SetValueAsync(seen);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator SetDescription(string title, string description)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("description").SetValueAsync(description);
        return new WaitUntil(() => DBTask.IsCompleted);
    }
    private IEnumerator SetAuthor(string title, string author)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("author").SetValueAsync(author);
        return new WaitUntil(() => DBTask.IsCompleted);
    }
    private IEnumerator SetVoteAverage(string title, double votesAverage)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("voteAverage").SetValueAsync(votesAverage);
        return new WaitUntil(() => DBTask.IsCompleted);
    }
    private IEnumerator SetVoteCount(string title, int votesCount)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("votesCount").SetValueAsync(votesCount);
        return new WaitUntil(() => DBTask.IsCompleted);
    }
    private IEnumerator SetNote(string title, string note)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(title).Child("note").SetValueAsync(note);
        return new WaitUntil(() => DBTask.IsCompleted);
    }

    private IEnumerator DeleteRecord(Movie movie, SavedMovieItem savedMovieItem)
    {
        var DBTask = databaseReference.Child("users").Child(user.UserId).Child("movies").Child(movie.Title).RemoveValueAsync();
        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
            Debug.LogWarning($"Failed to register task with exception: {DBTask.Exception}");
        else
            Destroy(savedMovieItem.gameObject);
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
