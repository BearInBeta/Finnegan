using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextAsset csvFile, infoFile; // assign the csv file from the Inspector
    private List<string> stringList = new List<string>();
    private List<string> indexList = new List<string>();
    [SerializeField] GameObject journal;
    [SerializeField] TMP_InputField journalField;

    string[,] csvData;
    string[] searchArray;
    [SerializeField] TypeWriter typewriter;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject next, exit, ask;
    public int character;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text placeholder;
    // Start is called before the first frame update
    void Start()
    {
      
        if(PlayerPrefs.HasKey("journal"))
            journalField.text = PlayerPrefs.GetString("journal");
        endDialogue();
        csvData = ReadCSVFile(csvFile);
        searchArray = SplitFirstColumn(csvData);
        string[] rows = infoFile.text.Split('\n');

        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].IndexOf(",") > -1)
            {
                string[] columns = rows[i].Split(',');
                stringList.Add(columns[1].Replace("/c",",")); // add column data to the list
                indexList.Add(columns[0]);
            }
        }

        if (!PlayerPrefs.HasKey("continue") || PlayerPrefs.GetInt("continue") != 1)
        {
            PlayerPrefs.SetInt("continue", 1);
            showInfo("tutorial");
        }
        // do something with the csvData array...
    }
    public void saveJournal()
    {
        PlayerPrefs.SetString("journal", journalField.text);
    }
    public void showInfo(string s)
    {
        character = 0;
        showDialogue();
        ask.SetActive(false);
        inputField.gameObject.SetActive(false);
        typewriter.clip = 0;
        typewriter.StartTyping(getInfo(s));
    }

    public void addToJournal()
    {
        journal.SetActive(true);
        journalField.text += "\n\n" + typewriter.lastWritten;
        saveJournal();
    }
    public string getInfo(string s)
    {
        return getInfo(indexList.IndexOf(s));
    }

    public string getInfo(int i)
    {
        return stringList[i];
    }

    public void showDialogue()
    {
        ask.SetActive(true);
        inputField.gameObject.SetActive(true);

        if (character == 10)
        {
            placeholder.text = "A keypad, with a paper that reads \"Whyauly'z Ipyaokhf\". Enter PIN?";
        }
        else
        {
            placeholder.text = "What do you want to ask about?";
        }
        typewriter.textMeshPro.text = "";
        dialogueBox.SetActive(true);
        
        inputField.Select();
    }
    public void selectInput()
    {
        inputField.Select();
    }
    public void endDialogue()
    {
        typewriter.stopTyping();
        typewriter.textToWrite = "";
        typewriter.lastWritten = "";
        StopCoroutine(typewriter.coroutine);
        next.SetActive(false);
        exit.SetActive(true);
        emptyField();
        dialogueBox.SetActive(false);
        typewriter.textMeshPro.text = "";
       
    }
    public void nextDialogue()
    {
        typewriter.StartTyping();
    }
    void emptyField()
    {
        inputField.text = "";
    }
    public void startDialogue()
    {
        typewriter.clip = character;
        if (typewriter.lastWritten != "")
        {
            if (typewriter.isTyping)
            {
                typewriter.stopTyping();
            }
            else
            {
                if (typewriter.textToWrite != "")
                    nextDialogue();
                else
                    endDialogue();
            }
        }
        else if(inputField.text != "" && character == 10)
        {
            typewriter.clip = 0;
            if (inputField.text.ToLower() == "0305")
            {
                typewriter.textToWrite = "The PIN is correct. Unfortunately, this is where the prototype ends.+Thank you for testing my prototype.*Your feedback would be much appreciated.";
                nextDialogue();
            }
            else
            {
                typewriter.textToWrite = "Wrong PIN.";
                nextDialogue();
            }
        }else if(inputField.text != "" && character < csvData.GetLength(1) && character > 0)
            sendQuery(inputField.text.ToLower(), character);
        else 
        {
            endDialogue();
        }
        emptyField();
    }
    void sendQuery(string text, int character)
    {
        typewriter.StartTyping(response(FindClosestMatch(searchArray, text), character));
    }

    string response(int query, int target)
    {
        string responseString = csvData[query, target];

        if(responseString.Equals("?") || responseString.Equals("") || responseString.Equals(null))
        {
            return "I am not sure what you're talking about.";
        }
        return responseString;
    }
    T[] SplitFirstColumn<T>(T[,] array)
    {
        int numRows = array.GetLength(0);
        T[] firstColumn = new T[numRows];

        for (int i = 0; i < numRows; i++)
        {
            firstColumn[i] = array[i, 0];
        }

        return firstColumn;
    }


    void PrintArray<T>(T[,] array)
    {
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                Debug.Log(array[i, j]);
            }
        }
    }
    void PrintArray<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            
                Debug.Log(array[i]);
            
        }
    }
    string[,] ReadCSVFile(TextAsset file)
    {
        string[,] dataArray;

        string[] rows = file.text.Split('\n');
        int numRows = rows.Length;

        string[] headers = rows[0].Split(',');
        int numCols = headers.Length;

        dataArray = new string[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            string[] rowValues = rows[i].Split(',');
            for (int j = 0; j < rowValues.Length; j++)
            {

                dataArray[i, j] = rowValues[j].Replace("/c",",").Trim();
            }
        }

        return dataArray;
    }
    public int FindClosestMatch(string[] array, string searchString)
    {
        int minDistance = int.MaxValue;
        int closestMatch = 0;

        for (int i = 1; i < array.Length; i++)
        {
            string s = array[i];
            int distance = ComputeLevenshteinDistance(s, searchString);

            if (LevenshteinPercentage(s, searchString, distance) > 0.5f && distance < minDistance)
            {
                minDistance = distance;
                closestMatch = i;
            }
        }

        return closestMatch;
    }

    float LevenshteinPercentage(string str1, string str2, int distance)
    {
        int maxLength = Math.Max(str1.Length, str2.Length);
        float percentage = (1 - (float)distance / maxLength) * 100;
        return percentage;
    }

    public int ComputeLevenshteinDistance(string s, string t)
    {
        int[,] d = new int[s.Length + 1, t.Length + 1];

        for (int i = 0; i <= s.Length; i++)
        {
            d[i, 0] = i;
        }

        for (int j = 0; j <= t.Length; j++)
        {
            d[0, j] = j;
        }

        for (int j = 1; j <= t.Length; j++)
        {
            for (int i = 1; i <= s.Length; i++)
            {
                int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost
                );
            }
        }

        return d[s.Length, t.Length];
    }
}
