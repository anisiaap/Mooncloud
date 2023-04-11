using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject thingToFollow;
    public GameObject coll1;
    void addStatus(int i){
        switch(i){
            case 0:
                coll1.gameObject.SetActive(true);
                break;
            /*case 1:
            case 2:
            case 3:*/
            default:
            break;
        }
    }
    void Start(){
        Instantiate(coll1, new Vector3(5, 4, -4), Quaternion.identity);
        coll1.gameObject.SetActive(false);
    }
    void Update()
    {
        transform.position = thingToFollow.transform.position + new Vector3(0f, 0f, -7f);

    }
}
