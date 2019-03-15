using System.Collections;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using UnityEngine;
using System.Runtime.CompilerServices;
using Unity;
public static class NoteHendler
{
    public static List<Note> notes;
    public static List<Note> availableNotes = new List<Note>();
    public static GameObject scrollView;

    public static List<Note> AvailableNotes
    {
        get
        {
            availableNotes.Sort();
            return availableNotes;
        }
    }

    static NoteHendler()
    {
        notes = JsonConvert.DeserializeObject<List<Note>>(System.IO.File.ReadAllText(@"e:\notes.json"));
        scrollView = GameObject.Find("ScrollNotes");
    }


    static void Sort()
    {
        //TODO sort logic 
    }
    public static void Ping()
    {
        Debug.Log("ping");
    }
    public static void AddNote(Note note)
    {
        notes.Add(note);
    }
    public static Note GetNote(int x)
    {
        
        foreach (Note note in notes)
        {
            if(note.No == x)
            {
                return note;
            }
        }
        throw new Exception("There is no note with this number");
        
    }
}
