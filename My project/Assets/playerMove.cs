using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class playerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2;

    float InstantiationTimer = 1f;
    float stepInterval = 1f;
    bool IsSneaking = false;
    public static int score = 0;

    [SerializeField] GameObject soundCenterPrefab;
    [SerializeField] GameObject clapSoundCenterPrefab;
    [SerializeField] GameObject sneakSoundCenterPrefab;
    [SerializeField] GameObject playerTrace;


    public AudioSource clap;
    public AudioSource concrete_step1;
    public AudioSource concrete_step2;
    public AudioSource concrete_step3;
    public AudioSource hapciu;
    

    void Start()
    {
        score = 0;
    }


    // Update is called once per frame
    void Update()
    {
        float direction_x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float direction_y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;;
        transform.Translate(direction_x, direction_y, 0);

        if(direction_x != 0 || direction_y != 0)
        {
            CreateSound();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            clap.Play();
            GameObject obj = Instantiate(clapSoundCenterPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 3f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Random.Range(1,300) == 124)
            {
                hapciu.Play();
                GameObject obj;
                obj = Instantiate(soundCenterPrefab, transform.position, Quaternion.identity);
                Destroy(obj, 2f);
            }
            moveSpeed = 1;
            IsSneaking = true;
            stepInterval = 0.7f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 2;
            IsSneaking = false;
            stepInterval = 1f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale != 0)
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
            }
        }
    }

    void CreateSound()
    {
        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            GameObject obj;
            if(IsSneaking)
            {
                concrete_step3.Play();
                obj = Instantiate(sneakSoundCenterPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                concrete_step1.Play();
                obj = Instantiate(soundCenterPrefab, transform.position, Quaternion.identity);
            }
            InstantiationTimer = stepInterval;
            Destroy(obj, 2f);

            obj = Instantiate(playerTrace, transform.position + new Vector3(0f,0f,-1f), Quaternion.identity);
            Destroy(obj, 3f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //SceneManager.UnloadScene("main_game_scene");
            SceneManager.LoadScene("DeadScene", LoadSceneMode.Single);
        }
    }
}
