using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToRoom : MonoBehaviour
{
    [SerializeField] Transform spawn, room;
    GameObject main_cam;
    // Start is called before the first frame update
    void Start()
    {
        main_cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.position = spawn.position;
            main_cam.transform.position = new Vector3(room.position.x, room.position.y, main_cam.transform.position.z);
        }
    }
}
