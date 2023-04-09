using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInteractable : MonoBehaviour
{
    Interaction interaction;
    public int index;
    public int intData;
    GameObject dialgoue;
    // Start is called before the first frame update
    void Awake()
    {
        interaction = GameObject.FindGameObjectWithTag("scripts").GetComponent<Interaction>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(index == 0)
            {
                dialogueInteraction(intData);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            interaction.coroutine = null;


        }
    }
    public void dialogueInteraction(int character)
    {
        if(interaction != null)
        interaction.coroutine = dialogueCoroutine(character);
    }

    public IEnumerator dialogueCoroutine(int character)
    {
        GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().character = character;

        GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().showDialogue();
        yield return null;
    }
}
