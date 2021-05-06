using System.Runtime.InteropServices.ComTypes;
using JetBrains.Annotations;

public class Movie
{
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

    public Movie()
    {
        title = "Not defined";
        imageUrl = "";
        releaseDate = "No release date";
        seen = false;
        description = "No description";
        author = "No author";
        note = "No notes";
    }
    public Movie(string title, string imageUrl, string releaseDate, bool seen)
    {
        this.title = title;
        this.imageUrl = imageUrl;
        this.releaseDate = releaseDate;
        this.seen = seen;
        description = "No description";
        author = "No author";
        note = "No notes";
    }
}
