using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddedPopUp : MonoBehaviour
{
   public void OnEnable()
   {
      LeanTween.scale(gameObject, new Vector3(1, 1, 1), 0.3f).setDelay(0.5f).setOnComplete(OnComplete);
   }

   void OnComplete()
   {
      LeanTween.scale(gameObject, new Vector3(0, 0, 0), 0.3f).setOnComplete(DestroyMe);
   }

   void DestroyMe()
   {
      gameObject.SetActive(false);
   }
}
