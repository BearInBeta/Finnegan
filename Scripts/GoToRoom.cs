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

        if (PlayerPrefs.HasKey("camx"))
            main_cam.transform.position = new Vector3(PlayerPrefs.GetFloat("camx"), main_cam.transform.position.y, main_cam.transform.position.z);

        if (PlayerPrefs.HasKey("camy"))
            main_cam.transform.position = new Vector3(main_cam.transform.position.x, PlayerPrefs.GetFloat("camy"), main_cam.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("roomsound").GetComponent<AudioSource>().Play();
            collision.gameObject.transform.position = new Vector3(spawn.position.x, spawn.position.y, collision.gameObject.transform.position.z);
            main_cam.transform.position = new Vector3(room.position.x, room.position.y, main_cam.transform.position.z);
            PlayerPrefs.SetFloat("camx", main_cam.transform.position.x);
            PlayerPrefs.SetFloat("camy", main_cam.transform.position.y);
        }
    }
}
