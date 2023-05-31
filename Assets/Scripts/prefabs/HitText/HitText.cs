using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitText : MonoBehaviour
{
    //Camera cameraMain;

    public float DestroyTextTime;
    public Vector3 offset = new Vector3(0,1,0);
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //cameraMain = Camera.main;
        transform.LookAt(player.transform);
        transform.rotation = Quaternion.LookRotation(player.transform.forward);

        Destroy(gameObject, DestroyTextTime);
        transform.localPosition += offset;
    }
}
