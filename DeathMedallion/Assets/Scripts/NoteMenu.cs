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

    private void OnEnable()
    {
        StartCoroutine(Init());
    }
    void ClearNotes()
    {

        for (var i = scrollView.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(scrollView.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        CheckForPressing();
    }

    public void CheckForPressing()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            StopCoroutine("Blink");
            Debug.Log("Pause is opened");
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
        if (scrollView.transform.childCount == 0) return;
        pressed = true;
        mainText.text = scrollView.transform.GetChild(choice).GetComponent<NotePrefab>().text;
        //StopCoroutine("Blink");
        for (int i = 0; i < scrollView.transform.childCount; i++)
        {
            scrollView.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        scrollView.transform.GetChild(choice).GetComponent<Image>().color = new Color32(255, 255, 100, 255);
        
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
    IEnumerator Init()
    {
        ClearNotes();
        Time.timeScale = 1f;
        yield return new WaitForEndOfFrame();
        Time.timeScale = 0f;
        foreach (Note note in NoteHendler.AvailableNotes)
        {
            var instaNote = Instantiate(notePrefab, scrollView.transform);
            instaNote.GetComponent<NotePrefab>().InitNote(note.Name, note.Text);
        }
        currentMenu = Notes;
        choice = new CircleInt(0, currentMenu.Grid.transform.childCount);
        OnNoteChange();
        choice = new CircleInt(0, scrollView.transform.childCount);

    } 
    

}
