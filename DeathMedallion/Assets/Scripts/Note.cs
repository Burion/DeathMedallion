using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note
{
    public Note(string name, int level, int no, string text)
    {
        Name = name;
        Level = level;
        No = no;
        Text = text;
    }

    string name;
    int level;
    int no;
    string text;
    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
        }
    }
    public int No
    {
        get
        {
            return no;
        }
        set
        {
            no = value;
        }
    }
    public string Text
    {
        get
        {
            return text;
        }
        set
        {
            text = value;
        }
    }
}
