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
    public static GameObject scrollView;
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
}
