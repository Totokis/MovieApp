using System.Runtime.InteropServices.ComTypes;

public class Movie
{
    string title;
    string imageUrl;
    string description;
    bool seen;

    public string Title => title;
    public string ImageUrl => imageUrl;
    public string Description => description;
    public bool Seen => seen;

    public Movie()
    {
        title = "Not defined";
        imageUrl = "";
        description = "No description";
        seen = false;
    }
    public Movie(string title, string imageUrl, string description, bool seen)
    {
        this.title = title;
        this.imageUrl = imageUrl;
        this.description = description;
        this.seen = seen;
    }
}
