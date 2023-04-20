using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetInteractable : MonoBehaviour
{
    Interaction interaction;
    public int index;
    public int intData;
    public string stringData;
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {

            if(index == 0)
            {
                dialogueInteraction(intData);
            }else if (index == 1)
            {
                infoInteraction(stringData);
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
        if(interaction != null && interaction.coroutine == null)
        interaction.coroutine = dialogueCoroutine(character);
    }
    public void infoInteraction(string item)
    {
        if (interaction != null && interaction.coroutine == null)
            interaction.coroutine = infoCoroutine(item);
    }

    public IEnumerator dialogueCoroutine(int character)
    {
        if (GameObject.FindGameObjectWithTag("dialogue") != null && GameObject.FindGameObjectWithTag("dialogue").activeInHierarchy)
        {

        }
        else
        {
            GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().character = character;

            GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().showDialogue();


        }
        interaction.coroutine = dialogueCoroutine(character);
        yield return null;

    }

    public IEnumerator infoCoroutine(string item)
    {
        

            GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().showInfo(item);
            interaction.coroutine = infoCoroutine(item);
            yield return null;

    }
}
