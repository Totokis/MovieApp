using UnityEngine;

public class Movie
{
    public Movie(string title, string imageUrl, string backdropUrl, string releaseDate, bool seen, double voteAverage, int votesCount, string description = "No description",
        string author = "No author", string note = "No note")
    {
        
        Title = title != "" ? title : "No title";
        ImageUrl = imageUrl != "" ? imageUrl : "/";
        BackdropUrl = backdropUrl != "" ? backdropUrl : "/";
        ReleaseDate = releaseDate != "" ? releaseDate : "No date";
        Seen = seen;
        Description = description != "" ? description : "No description";
        Author = author != "" ? author : "No author";
        Note = note != "" ? note : "No note";
        VoteAverage = voteAverage;
        VotesCount = votesCount;
        PosterSprite = Sprite.Create(new Texture2D(10, 10), new Rect(0f, 0f, 10, 10), Vector2.zero);
        BackdropSprite = Sprite.Create(new Texture2D(10, 10), new Rect(0f, 0f, 10, 10), Vector2.zero);
    }

    public string Title { get; }

    public string ImageUrl { get; }

    public string BackdropUrl { get; }

    public string ReleaseDate { get; }

    public bool Seen { get; }

    public string Description { get; }

    public string Author { get; }

    public string Note { get; }

    public Sprite PosterSprite { get; private set; }

    public double VoteAverage { get; }

    public int VotesCount { get; }


    public Sprite BackdropSprite { get; private set; }
    
}
