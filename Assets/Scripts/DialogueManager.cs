using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();
    }

    public void WriteDialogue(Dialogue dialogue)
    {
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplaySentence();
    }
    public void DisplaySentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
    }
    public void EndDialogue()
    {
        sentences.Clear();
    }
}
