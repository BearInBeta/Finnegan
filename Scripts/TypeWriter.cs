using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string textToWrite;
    public string lastWritten;
    public int clip;
    [SerializeField] GameObject next, exit;
    [SerializeField] private float delay = 0.1f;
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioSource speech;
    public IEnumerator coroutine;
    public bool isTyping = false;

    private void Awake()
    {
        coroutine = ShowText();
    }
    public void stopTyping()
    {
        
        StopCoroutine(coroutine);
        isTyping = false;
        speech.Stop();
        textMeshPro.text = lastWritten;
        
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
        speech.clip = clips[clip];
        bool textToContinue = false;
        while (textToWrite.IndexOf("{") > -1)
        {
            char flag = textToWrite[textToWrite.IndexOf("{") + 1];
            if (!PlayerPrefs.HasKey("flag-" + flag) || PlayerPrefs.GetInt("flag-" + flag) != 1)
            {
                textToWrite = textToWrite.Substring(0, textToWrite.IndexOf("{"));
            }
            else
            {
                textToWrite = textToWrite.Substring(textToWrite.IndexOf("}") + 1);
            }
        }
        while (textToWrite.IndexOf("[") > -1)
        {
            
            char flag = textToWrite[textToWrite.IndexOf("[") + 1];
            var regex = new Regex(Regex.Escape("[" + flag + "]"));
            textToWrite = regex.Replace(textToWrite, "",1);
            PlayerPrefs.SetInt("flag-" + flag, 1);
        }

      
        string textToWriteClone = (string)textToWrite.Clone();
        if (textToWrite.IndexOf("+") + 1 > 0)
        {
            lastWritten = textToWrite.Substring(0, textToWrite.IndexOf("+"));
            textToWrite = textToWrite.Substring(textToWrite.IndexOf("+") + 1);
            textToContinue = true;
        }
        else
        {
            lastWritten = textToWrite;
            textToWrite = "";
        }
        next.SetActive(false);
        exit.SetActive(false);
        textMeshPro.text = "";
        isTyping = true;
        speech.Play();
        foreach (char c in textToWriteClone)
        {
            if(c == '+')
            {
                break;
            }
            
            textMeshPro.text += c;
            yield return new WaitForSeconds(delay);
        }
        speech.Stop();
        isTyping = false;
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