using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] TextAsset csvFile; // assign the csv file from the Inspector
    string[,] csvData;
    string[] searchArray;
    [SerializeField] TypeWriter typewriter;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject next, exit;
    public int character;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TMP_Text placeholder;
    // Start is called before the first frame update
    void Start()
    {
        endDialogue();
        csvData = ReadCSVFile(csvFile);
        searchArray = SplitFirstColumn(csvData);
        // do something with the csvData array...
    }
    public void showDialogue()
    {
        if (character == 6)
        {
            placeholder.text = "Enter the password. It's the initials of each person in the order they were seated.";
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

        if (typewriter.textToWrite != "")
        {
            nextDialogue();
        }
        else if(inputField.text != "" && character == 6)
        {
            if(inputField.text.ToLower() == "cbjrl")
            {
                typewriter.textToWrite = "Correct Password.*Thank you for testing my prototype.*Your feedback would be much appreciated.";
                nextDialogue();
            }
            else
            {
                typewriter.textToWrite = "Wrong password. Try again.";
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
