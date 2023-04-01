using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemCollector : MonoBehaviour
{
    public int score;
     void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Cherry")){
        Destroy(other.gameObject);
        score++;
        }

    }

}
