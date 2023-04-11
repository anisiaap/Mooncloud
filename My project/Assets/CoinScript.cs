using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static playerMove;
using UnityEngine.SceneManagement;

public class CoinScript : MonoBehaviour
{
    public GameObject dropSoundCenterPrefab;
    public float InstantiationTimer = 0.2f;

    public AudioSource collected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            GameObject obj;
            obj = Instantiate(dropSoundCenterPrefab, transform.position + new Vector3(Random.Range(-0.4f,0.4f),Random.Range(-0.4f,0.4f), 0.0f), Quaternion.identity);
            Destroy(obj, 1f);
            InstantiationTimer = 0.2f;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            collected.Play();
            Destroy(gameObject,2f);
            playerMove.score++;
            
            if (playerMove.score == 4)
            {
                SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
            }
        }
    }
}
