using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFrame : MonoBehaviour
{
    public static ImageFrame Instance { get; private set; }

    [SerializeField] private Image image;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Open(Sprite sprite)
    {
        image.sprite = sprite;
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.1f);
    }

    public void Close()
    {
        LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.1f).setOnComplete(() => gameObject.SetActive(false));
    }
    

}
