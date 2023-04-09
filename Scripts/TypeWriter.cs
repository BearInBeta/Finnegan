using System.Collections;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string textToWrite;
    [SerializeField] GameObject next, exit;
    [SerializeField] private float delay = 0.1f;

    private IEnumerator coroutine;

    private void Awake()
    {
        coroutine = ShowText();
    }

    public void StartTyping(string textToWrite)
    {
        this.textToWrite = textToWrite;
        StartTyping();
    }
    public void StartTyping()
    {
        StopCoroutine(coroutine);
        coroutine = ShowText();
        StartCoroutine(coroutine);
    }
    private IEnumerator ShowText()
    {
        bool textToContinue = false;
        string textToWriteClone = (string) textToWrite.Clone();
        if (textToWrite.IndexOf("*") + 1 > 0)
        {
            textToWrite = textToWrite.Substring(textToWrite.IndexOf("*") + 1);
            textToContinue = true;
        }
        else
        {
            textToWrite = "";
        }
        next.SetActive(false);
        exit.SetActive(false);
        textMeshPro.text = "";
        foreach (char c in textToWriteClone)
        {
            if(c == '*')
            {
                break;
            }
            
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
        if (textToContinue)
        {
            next.SetActive(true);
            exit.SetActive(false);
        }
        else
        {
            
            next.SetActive(false);
            exit.SetActive(true);
            GameObject.FindGameObjectWithTag("scripts").GetComponent<Dialogue>().selectInput();
        }
    }
}