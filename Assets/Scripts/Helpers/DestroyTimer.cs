using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
   public float timeToDestroy = 1f;
   private float timer;
   
   
   void Start()
   {
      timer = timeToDestroy;
   }

   private void Update()
   {
      timer -= Time.deltaTime;
      if (timer <= 0)
      {
         Destroy(gameObject);
      }
   }
}
