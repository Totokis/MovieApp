using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextureBase : MonoBehaviour
{
    private static Dictionary<string, Texture2D> texturesDictionary;
    private static Queue<Action> queuedTask;

    [SerializeField] public Sprite defaultSprite;
    [SerializeField] bool Enqueue = false;

    public static TextureBase Instance { get; private set; }

    private void Awake()
    {
        queuedTask = new Queue<Action>();
        texturesDictionary = new Dictionary<string, Texture2D>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Enqueue)
        {
            if (queuedTask.Count != 0)
            {
                var func = queuedTask.Dequeue();
                func.Invoke();
            }
        }
        
    }

    private void OnDestroy()
    {
        texturesDictionary.Clear();
    }

    async Task<Sprite> GetTexture(string url)
    {
        if (texturesDictionary.ContainsKey(url))
        {
            var texture = texturesDictionary[url];
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero);
        }
        var newTexture = await GetRemoteTexture(url);
        if(!texturesDictionary.ContainsKey(url))
            texturesDictionary.Add(url, newTexture);
        return Sprite.Create(newTexture, new Rect(0f, 0f, newTexture.width, newTexture.height), Vector2.zero);
    }

    private async Task<Texture2D> GetRemoteTexture(string url)
    {
        using var www = UnityWebRequestTexture.GetTexture(url);
        var asyncOp =  www.SendWebRequest();

        while (asyncOp.isDone == false)
        {
#if DEBUG
#endif
            await Task.Delay(1000 / 30);
        }

        if (www.result == UnityWebRequest.Result.Success)
            return DownloadHandlerTexture.GetContent(www);
#if DEBUG
        Debug.Log($"{www.error}, URL: {www.url}");
#endif
        return defaultSprite.texture;
    }

    public void AddToQueue(string url, Image image)
    {
        image.sprite = defaultSprite;
        queuedTask.Enqueue(() => SetSprite(url, image));
    }

    private async Task SetSprite(string url, Image image)
    { 
        image.sprite = await GetTexture(url);
    }
}
