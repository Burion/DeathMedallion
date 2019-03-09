using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NoteMenu : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject scrollView;
    [SerializeField] TextMeshProUGUI mainText;
    public int currentPosInt;

    private void OnEnable()
    {
        if (scrollView.transform.GetChildCount() < 1)
            return;
        
        currentPosInt = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentPosInt != scrollView.transform.GetChildCount() - 1)
                currentPosInt++;
            else
                currentPosInt = 0;
            OnNoteChange();
        }
    }

    void CheckForPressing()
    {
        //pressLogic
    }
    void Start()
    {
        foreach(Note note in NoteHendler.notes)
        {
            var instaNote = Instantiate(notePrefab, scrollView.transform);
            instaNote.GetComponent<NotePrefab>().InitNote(note.Name, note.Text);
            //"L" + note.Level.ToString() + "-" + note.No.ToString()
        }
        OnNoteChange();
        StartCoroutine("Blink");
    }

    void OnNoteChange()
    {
        mainText.text = scrollView.transform.GetChild(currentPosInt).GetComponent<NotePrefab>().text;
        StopCoroutine("Blink");
        for (int i = 0; i < scrollView.transform.childCount; i++)
        {
            scrollView.transform.GetChild(i).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        StartCoroutine("Blink");
    }
    IEnumerator Blink()
    {
        var currentNote = scrollView.transform.GetChild(currentPosInt).GetComponent<Image>();
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
