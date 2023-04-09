using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Controller : MonoBehaviour
{
    Rigidbody2D body;
    [SerializeField] TMP_InputField inputField;
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    Animator anim;
    public float runSpeed = 20.0f;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("dialogue") !=  null && GameObject.FindGameObjectWithTag("dialogue").activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().startDialogue();
            }
            return;
        }
        // Gives a value between -1 and 1
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down


        if (horizontal > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            anim.SetInteger("Direction", 3);
        }else if(horizontal < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            anim.SetInteger("Direction", 1);
        }
        else if (vertical > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

            anim.SetInteger("Direction", 2);
        }
        else if (vertical < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;

            anim.SetInteger("Direction", 0);
        }


        float speed = Mathf.Sqrt(Mathf.Pow(horizontal, 2) + Mathf.Pow(vertical, 2));
        anim.SetFloat("Speed", Mathf.Abs(speed));

    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }
}
