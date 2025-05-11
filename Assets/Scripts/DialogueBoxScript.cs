using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxScript : MonoBehaviour
{

    public Dialogue[] dialogue, angryAnswears;
    public int sentenceIndex = 0, dialogueIndex = 0;
    private bool finished=false;
    [SerializeField]
    private float endWait;

    [SerializeField]
    private TMP_Text dialogueText, visibleText, dotText;
    [SerializeField]
    private Image cloud, angryCloud;
    public GameObject dialogBox;
    public GameObject self;
    public float speedtypeing;
    public GameObject wife;
    private GameObject parent;


    
    void Start()
    {
        parent = GameObject.Find("MainCanvas");
        wife = GameObject.Find("Wife");
        //endWait =Random.Range(1.5f, 3.0f);
    }

    

    public void WriteDialogue()
    {
        StartCoroutine(typeSentence(dialogue[dialogueIndex].sentences[sentenceIndex]));
    }

    public void WriteAngryDialogue()
    {
        StartCoroutine(typeAngrySentence(angryAnswears[Random.Range(0, angryAnswears.Length)].sentences[sentenceIndex]));
    }

    IEnumerator typeAngrySentence(string sentence)
    {
        cloud.enabled = false;
        angryCloud.enabled = true;
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            GetComponentInChildren<TMP_Text>(dialogueText).text += c;
            visibleText.text += c;
            GameObject.Find("Car").GetComponentInChildren<Audio>().wifeVoicePlay();
            yield return new WaitForSeconds(speedtypeing);
        }
        yield return new WaitForSeconds(0.6f);
        cloud.enabled = true;
        angryCloud.enabled = false;


        dialogueIndex = Random.Range(0, dialogue.Length);
        sentenceIndex = 0;
        visibleText.text = "";
        dialogueText.text = "";
        WriteDialogue();
    }

    IEnumerator typeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char c in sentence.ToCharArray())
        {
            GetComponentInChildren<TMP_Text>(dialogueText).text += c;
            visibleText.text += c;
            GameObject.Find("Car").GetComponentInChildren<Audio>().wifeVoicePlay();
            yield return new WaitForSeconds(speedtypeing);
        }
        yield return new WaitForSeconds(1);

        if (dialogue[dialogueIndex].question)
        {
            wife.GetComponent<Wife>().QuestionAppear(dialogue[dialogueIndex].answer, gameObject);
        }
        else if (dialogue[dialogueIndex].sentences.Length - 1 > sentenceIndex)
        {
            sentenceIndex++;
            visibleText.text = "";
            dialogueText.text = "";
            WriteDialogue();
        }
        else
        {
            dialogueIndex = Random.Range(0, dialogue.Length);
            sentenceIndex = 0;
            visibleText.text = "";
            dialogueText.text = "";
            WriteDialogue();
        }

    }

    public void HideAppearText()
    {
        visibleText.enabled = !visibleText.enabled;
        dotText.enabled = !dotText.enabled;
    }

    public void NewDialogue(bool hit)
    {
        dialogueIndex = Random.Range(0, dialogue.Length);
        sentenceIndex = 0;
        visibleText.text = "";
        dialogueText.text = "";
        if(hit)
            WriteAngryDialogue();
        else
            WriteDialogue();
    }
}
