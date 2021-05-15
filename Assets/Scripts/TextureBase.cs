using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TextureBase : MonoBehaviour
{
   public static TextureBase Instance { get; private set; }
   
   [SerializeField] public Sprite defaultSprite;

   static Dictionary<string, Texture2D> texturesDictionary;
   static Queue<Action> queuedTask;

   void Awake()
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

   public async Task<Sprite> GetTexture(string url)
   {
      if (texturesDictionary.ContainsKey(url))
      {
#if DEBUG
         Debug.Log($"Image found");
#endif
         var texture = texturesDictionary[url];
         return Sprite.Create(texture,new Rect(0f,0f,texture.width,texture.height),Vector2.zero);
      }
      else
      {
#if DEBUG
         Debug.Log($"Image not found, loading");
#endif
         var newTexture = await GetRemoteTexture(url);
         texturesDictionary.Add(url,newTexture);
         return Sprite.Create(newTexture,new Rect(0f,0f,newTexture.width,newTexture.height),Vector2.zero);
      }
   }

   async Task<Texture2D> GetRemoteTexture(string url)
   {
      using var www = UnityWebRequestTexture.GetTexture(url);
      var asyncOp = www.SendWebRequest();

      while (asyncOp.isDone == false)
         await Task.Delay(1000 / 30);

      if (www.result == UnityWebRequest.Result.Success)
         return DownloadHandlerTexture.GetContent(www);
#if DEBUG
      Debug.Log($"{www.error}, URL: {www.url}");
#endif
      return defaultSprite.texture;
   }

   public async void AddToQueue(string url, Image image)
   {
      if (queuedTask.Count == 0)
      {
         image.sprite = await GetTexture(url);
      }
      else
      {
         queuedTask.Enqueue(()=>SetSprite(url,image));
      }
   }

   async void SetSprite(string url, Image image)
   {
      image.sprite = await GetTexture(url);
   }

   void Update()
   {
      if (queuedTask.Count != 0)
      {
         Debug.Log("QueueTask: DEQUEUEING");
         var func = queuedTask.Dequeue();
         func.Invoke();
      }
   }

   private void OnDestroy()
   {
      texturesDictionary.Clear();
   }
}




