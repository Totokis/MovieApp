using UnityEngine;

public class Movie
{
    Sprite posterSprite;
    Sprite backdropSprite;
    string title;
    string imageUrl;
    string backdropUrl;
    string releaseDate;
    bool seen;
    string description;
    string author;
    string note;
    double voteAverage;
    int votesCount;

    public string Title => title;
    public string ImageUrl => imageUrl;
    public string BackdropUrl => backdropUrl;
    public string ReleaseDate => releaseDate;
    public bool Seen => seen;
    public string Description => description;
    public string Author => author;
    public string Note => note;
    public Sprite PosterSprite => posterSprite;
    public double VoteAverage => voteAverage;
    public int VotesCount => votesCount;
    

    public Sprite BackdropSprite => backdropSprite;
    
    public Movie(string title, string imageUrl, string backdropUrl, string releaseDate, bool seen, double voteAverage,int votesCount,string description = "No description", string author = "No author", string note = "No note")
    {
        this.title = title != "" ? title : "No title";
        this.imageUrl = imageUrl != "" ? imageUrl : "/";
        this.backdropUrl = backdropUrl != "" ? backdropUrl : "/";
        this.releaseDate = releaseDate != "" ? releaseDate : "No date";
        this.seen = seen;
        this.description = description != "" ? description : "No description";
        this.author = author != "" ? author : "No author";
        this.note = note != "" ? note : "No note";
        this.voteAverage = voteAverage;
        this.votesCount = votesCount;
        this.posterSprite = Sprite.Create(new Texture2D(10,10),new Rect(0f,0f,10,10),Vector2.zero);
        this.backdropSprite = Sprite.Create(new Texture2D(10,10),new Rect(0f,0f,10,10),Vector2.zero);
    }

    public void SetPosterSprite(Sprite sprite)
    {
        this.posterSprite = sprite;
    }
    
    public void SetBackdropSprite(Sprite sprite)
    {
        this.backdropSprite = sprite;
    }
}
