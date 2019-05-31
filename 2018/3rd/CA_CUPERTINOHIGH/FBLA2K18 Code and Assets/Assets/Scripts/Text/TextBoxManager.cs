using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {
    public GameObject textBox;
    public Text theText;

    public Dictionary<string, string> textFiles;
    public string textFileName;
    public string[] textLines;

    public int currentLine;
    public int endAtLine;

    private bool showText = false;
    public bool finishFile;
    //public GameObject textBox;
    //public PlayerController player;
    // Use this for initialization
    private void Awake()
    {
        textBox = GameObject.Find("Dialog Panel");
        theText = textBox.GetComponentInChildren<Text>();
        finishFile = true;
    }
    private void Start()
    {
        textBox.SetActive(false);
    }
    

    public void LoadTextFile()
    {
        if (textFileName.Equals(""))
        {
            textBox.SetActive(false);
            currentLine = 0;
            endAtLine = 0;
        }
        else
        {
            textFileName += ".txt";

            textFiles = new Dictionary<string, string>();
            DirectoryInfo d = new DirectoryInfo("./Assets/Text");
            FileInfo[] Files = d.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                textFiles.Add(file.Name, "./Assets/Text/" + file.Name);
                //Debug.Log(file.Name + " " + textFiles[file.Name]);
            }


            textLines = File.ReadAllLines(textFiles[textFileName]);
            
            if (endAtLine == 0)
            {
                endAtLine = textLines.Length - 1;
            }
        }
    }

    public void SetText(string text)
    {
        //Debug.Log(text);
        textFileName = text;
        textBox.SetActive(true);
        LoadTextFile();
        finishFile = false;
        showText = true;
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(textLines);
        if (!textFileName.Equals("") && showText)
        {
            //Debug.Log(endAtLine + " " + currentLine);
            theText.text = textLines[currentLine];
            //Debug.Log(textLines);
            if (Input.GetKeyDown(KeyCode.Return))
            {
                currentLine++;
            }
            if (currentLine > endAtLine)
            {
                textBox.SetActive(false);
                currentLine = 0;
                endAtLine = 0;
                finishFile = true;
               // Debug.Log("DONE");
                showText = false;
            }
        }
    }
}
