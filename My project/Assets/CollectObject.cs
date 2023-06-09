using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObject : MonoBehaviour
{
   private Object thisObject;
   private void Awake(){
    thisObject = GetComponent<Object>();
   }

   private void OnTriggerEnter2D(Collider2D collision){
    if(collision.CompareTag("Player")){
        Destroy(gameObject);
    }
   }
}
