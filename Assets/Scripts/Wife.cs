using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class Wife : MonoBehaviour
{
    public Transform wifeAppearPoint;
    public int health, maxHealth, correctAnswear;
    public Sprite[] wifeSprites;
    [TextArea]
    public string[] answears;
    public GameObject background, dialogueInit, gameOver, victoryPanel;

    [SerializeField]
    private int moveSpeed;
    [SerializeField]
    private Image wifeImage;
    [SerializeField]
    private TMP_Text[] choseButtonText;

    public static bool PlayerMovementAllowed = true;
    public static float RecommendedTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        health = 3;
        //StartCoroutine(MoveIn());
    }

    public IEnumerator MoveIn()
    {
        float tolerance = 0.5f; 

        while (Vector3.Distance(transform.position, wifeAppearPoint.position) > tolerance)
        {
            transform.position = Vector2.MoveTowards(transform.position, wifeAppearPoint.transform.position, moveSpeed * Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        int dialogue = Random.Range(0, dialogueInit.GetComponent<DialogueBoxScript>().dialogue.Length);
        dialogueInit.SetActive(true);
        dialogueInit.GetComponent<DialogueBoxScript>().dialogueIndex = dialogue;
        dialogueInit.GetComponent<DialogueBoxScript>().WriteDialogue();
    }
    public void CheckAnswear(int answear)
    {
        background.SetActive(false);
        bool hit = false;
        if (answear != correctAnswear)
        {
            healthDecrese();
            hit= true;
        }
        if (health == 0)
            return;
        PlayerMovementAllowed = true;
        Time.timeScale = 1;
        RecommendedTime = 1;
        dialogueInit.GetComponent<DialogueBoxScript>().HideAppearText();
        dialogueInit.GetComponent<DialogueBoxScript>().NewDialogue(hit);
    }

    public void QuestionAppear(int answearIndex, GameObject question)
    {
        dialogueInit = question;
        dialogueInit.GetComponent<DialogueBoxScript>().HideAppearText();
        List<string> possibleAnswear = new List<string>();
        possibleAnswear.Add(answears[answearIndex]);
        while (possibleAnswear.Count !=3)
        {
            int randomAnswear = Random.Range(0, answears.Length);
            if (!possibleAnswear.Contains(answears[randomAnswear]))
                possibleAnswear.Add((answears[randomAnswear]));
        }
        correctAnswear = Random.Range(0, 3);
        string temp = possibleAnswear[correctAnswear];
        possibleAnswear[correctAnswear] = possibleAnswear[0];
        possibleAnswear[0] = temp;


        for(int i = 0; i < 3; i++)
        {
            choseButtonText[i].text = possibleAnswear[i];
        }
        background.SetActive(true);
        PlayerMovementAllowed = false;
        Time.timeScale = 0.1F;
        RecommendedTime = 0;
    }
    
    public void healthDecrese()
    {
        health--;
        if (health <= 0)
        {
            health = 0;
            PlayerMovementAllowed = false;
            Time.timeScale = 0;
            if(gameOver)
            {
                gameOver.SetActive(true);
            }
            return;
        }

        wifeImage.sprite = wifeSprites[health - 1];
    }

    public void EndGame()
    {
        wifeImage.sprite = wifeSprites[2];
        victoryPanel.SetActive(true);
        dialogueInit.SetActive(false);
        Time.timeScale = 0;
        PlayerMovementAllowed=false;
    }
}
