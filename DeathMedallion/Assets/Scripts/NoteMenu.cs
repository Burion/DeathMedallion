using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NoteMenu : Menu, IMenu
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject scrollView;
    [SerializeField] TextMeshProUGUI mainText;
    public static bool wasOpened = false;

    private void OnEnable()
    {
        if (!wasOpened)
        {
            foreach (Note note in NoteHendler.notes)
            {
                var instaNote = Instantiate(notePrefab, scrollView.transform);
                instaNote.GetComponent<NotePrefab>().InitNote(note.Name, note.Text);
            }
        }
        wasOpened = true;
        currentMenu = Notes;

        choice = new CircleInt(0, currentMenu.Grid.transform.childCount);
        

        OnNoteChange();
        StartCoroutine("Blink");
        choice = new CircleInt(0, scrollView.transform.childCount);
        Debug.Log(choice.MaxValue);

    }
    void ClearNotes()
    {
        foreach(Transform trans in scrollView.transform)
        {
            Destroy(trans.gameObject);
        }
    }

    private void Update()
    {
        Debug.Log(Input.GetAxisRaw("UpDown"));
        CheckForPressing();
    }

    public void CheckForPressing()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StopCoroutine("Blink");
            OpenMenu(Pause);
           
        }
        if (!pressed)
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                choice++;
                OnNoteChange();
            }
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                choice--;
                OnNoteChange();
            }
            if (Input.GetAxisRaw("UpDown") == -1)
            {
                choice += scrollView.GetComponent<GridLayoutGroup>().constraintCount;
                OnNoteChange();
            }
            if (Input.GetAxisRaw("UpDown") == 1)
            {
                choice -= scrollView.GetComponent<GridLayoutGroup>().constraintCount;
                OnNoteChange();
            }
        }
        FixPressing();
    }
    void Start()
    {
        
    }


    
    void OnNoteChange()
    {
        pressed = true;
        mainText.text = scrollView.transform.GetChild(choice).GetComponent<NotePrefab>().text;
        StopCoroutine("Blink");
        for (int i = 0; i < scrollView.transform.childCount; i++)
        {
            scrollView.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        StartCoroutine("Blink");
    }
    IEnumerator Blink()
    {
        var currentNote = scrollView.transform.GetChild(choice).GetComponent<Image>();
        int alpha = 255;
        while (alpha > 155)
        {
            currentNote.color = new Color32(255, 255, 255, (byte)alpha);
            yield return new WaitForEndOfFrame();
            alpha-=8;
        }
        while (alpha < 254)
        {
            currentNote.color = new Color32(255, 255, 255, (byte)alpha);
            yield return new WaitForEndOfFrame();
            alpha+=8;
        }
        StartCoroutine("Blink");
    }
    

}
