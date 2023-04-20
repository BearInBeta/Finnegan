using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInfo : MonoBehaviour
{

    [SerializeField] TypeWriter typewriter;
    [SerializeField] GameObject dialogueBox;

    [SerializeField] TextAsset csvFile; // assign the csv file from the Inspector

    public int columnNumber; // column number to be extracted

    private List<string> stringList = new List<string>();
    private List<string> indexList = new List<string>();

    void Start()
    {
        string[] rows = csvFile.text.Split('\n');

        for (int i = 0; i < rows.Length; i++)
        {
            if (rows[i].IndexOf(",") > -1)
            {
                string[] columns = rows[i].Split(',');
                stringList.Add(columns[1]); // add column data to the list
                indexList.Add(columns[0]);
            }
        }

        // print the column array to the console
    }

    public void showInfo()
    {
        
        typewriter.textMeshPro.text = "";
        dialogueBox.SetActive(true);
    }

    public string getInfo(string s)
    {
        return getInfo(indexList.IndexOf(s));
    }

    public string getInfo(int i)
    {
        return stringList[i];
    }
}
