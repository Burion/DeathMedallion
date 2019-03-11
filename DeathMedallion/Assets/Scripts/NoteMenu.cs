using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NoteMenu : Menu
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject scrollView;
    [SerializeField] TextMeshProUGUI mainText;

    private void OnEnable()
    {
        foreach (Note note in NoteHendler.notes)
        {
            var instaNote = Instantiate(notePrefab, scrollView.transform);
            instaNote.GetComponent<NotePrefab>().InitNote(note.Name, note.Text);
            //"L" + note.Level.ToString() + "-" + note.No.ToString()
        }
        OnNoteChange();
        StartCoroutine("Blink");
        choice = new CircleInt(0, scrollView.transform.childCount);
        Debug.Log(choice.MaxValue);

    }

    private void Update()
    {
        CheckForPressing();
    }

    void CheckForPressing()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            OpenMenu(transform.parent.Find("Pause").gameObject);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            choice++;
            OnNoteChange();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            choice += scrollView.GetComponent<GridLayoutGroup>().constraintCount;
            OnNoteChange();
        }
    }
    void Start()
    {
        
    }

    void OnNoteChange()
    {
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
