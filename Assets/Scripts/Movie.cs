using UnityEngine;

public class Movie
{
    Sprite sprite;
    string title;
    string imageUrl;
    string releaseDate;
    bool seen;
    string description;
    string author;
    string note;

    public string Title => title;
    public string ImageUrl => imageUrl;
    public string ReleaseDate => releaseDate;
    public bool Seen => seen;
    public string Description => description;
    public string Author => author;
    public string Note => note;
    public Sprite Sprite => sprite;
    
    public Movie(string title, string imageUrl, string releaseDate, bool seen, string description = "No description", string author = "No author", string note = "No note")
    {
        this.title = title;
        this.imageUrl = imageUrl;
        this.releaseDate = releaseDate;
        this.seen = seen;
        this.description = description;
        this.author = author;
        this.note = note;
        this.sprite = Sprite.Create(new Texture2D(10,10),new Rect(0f,0f,10,10),Vector2.zero);
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }
}
