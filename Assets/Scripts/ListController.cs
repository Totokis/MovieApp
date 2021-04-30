// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Firebase.Auth;
// using Firebase.Database;
// using UnityEngine;
// using UnityEngine.Networking;
// using UnityEngine.UI;
//
//
// public class ListController : MonoBehaviour
// {
//     [SerializeField] Image _image;
//     [SerializeField] Sprite _deafultIcon;
//     [SerializeField] InputField _input;
//     [SerializeField] GameObject _buttonTemplatePrefab;
//     List<Movie> _movies;
//     List<Sprite> _sprites;
//     // DatabaseReference _reference;
//
//     void Awake()
//     {
//         _movies = new List<Movie>();
//         _sprites = new List<Sprite>();
//     }
//     void Start()
//     {
//         GetMovies();
//     }
//     void GetMovies()
//     {
//         //GenerateMovies();
//         StartCoroutine(GetImages());
//     }
//     public void GetSearchedMovies()
//     {
//         _movies = new List<Movie>();
//         _sprites = new List<Sprite>();
//         foreach (Transform child in transform) {
//             GameObject.Destroy(child.gameObject);
//         }
//         foreach (var result in APIHelper.GetNewResult(_input.text))
//         {
//             // var movie = new Movie { Name = result.title, IconUrl = "https://image.tmdb.org/t/p/w500" + result.poster_path };
//             //_movies.Add(movie);
//         }
//         StartCoroutine(GetImages());
//     }
//     // void GenerateMovies()
//     // {
//     //     foreach (var result in APIHelper.GetNewResult())
//     //     {
//     //         var movie = new Movie { Name = result.title, IconUrl = "https://image.tmdb.org/t/p/w500" + result.poster_path };
//     //         _movies.Add(movie);
//     //         //Debug.Log($"Result URL: {movie.IconUrl}");
//     //     }
//     // }
//     // void DrawUI()
//     // {
//     //     GameObject buttonTemplate = _buttonTemplatePrefab;
//     //     GameObject gameObject;
//     //     for (int i = 0; i < _movies.Count; i++)
//     //     {
//     //         gameObject = Instantiate(_buttonTemplatePrefab, transform);
//     //         gameObject.transform.GetChild(1).GetComponent<Image>().sprite = _sprites[i];
//     //         gameObject.transform.GetChild(0).GetComponent<Text>().text = _movies[i].Name;
//     //         
//     //     }
//     // }
//
//     IEnumerator GetImages()
//     {
//         
//         for (int i = 0; i < _movies.Count; i++)
//         {
//             using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(_movies[i]))
//             {
//                 yield return uwr.SendWebRequest();
//                 if (uwr.result != UnityWebRequest.Result.Success)
//                 {
//                     Debug.Log(uwr.error);
//                     _sprites.Add(_deafultIcon);
//                 }
//                 else
//                 {
//                     // Get downloaded asset bundle
//                     var texture = DownloadHandlerTexture.GetContent(uwr);
//                     _sprites.Add(Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 10f));
//                 }
//             }
//         }
//         Debug.Log("Loading");
//         using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("https://miro.medium.com/max/1400/1*mk1-6aYaf_Bes1E3Imhc0A.jpeg"))
//         {
//             yield return uwr.SendWebRequest();
//             if (uwr.result != UnityWebRequest.Result.Success)
//             {
//                 Debug.Log(uwr.error);
//             }
//             else
//             {
//                 // Get downloaded asset bundle
//                 var texture = DownloadHandlerTexture.GetContent(uwr);
//                 _image.material.color = Color.white;
//                 gameObject.GetComponent<Image>().color = Color.blue;
//                 gameObject.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 10f);
//             }
//         }
//         //DrawUI();
//     }
//
//     // public void GetDataBase()
//     // {
//     //     if (_reference == null)
//     //     {
//     //
//     //         _reference = FirebaseDatabase.GetInstance("https://fir-test-73865-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
//     //         
//     //         Debug.Log("DataBase loaded");
//     //     }
//     //     var user = FirebaseAuth.DefaultInstance.CurrentUser;
//     //     _reference.Child("users").Child(user.UserId).Child("Movies").Child(_movies.First().Name).Child("Image").SetValueAsync(_movies.First().IconUrl);
//     //     _reference.Child("users").Child(user.UserId).Child("Movies").Child(_movies.First().Name).Child("Seen").SetValueAsync("false");
//     //
//     // }
// }
