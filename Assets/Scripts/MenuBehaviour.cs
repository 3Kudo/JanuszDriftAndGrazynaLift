using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public CarScript carScript;
    public Wife wife;

    public GameObject buttonStart;
    public GameObject buttonCredits;
    public GameObject buttonExit;

    public GameObject panelStart;
    public GameObject panelPause;

    public float rotationSpeed = 100f;
    public float rotationRange = 10f;
    private float rotationOffset;

    public void StartGame()
    {
        Wife.PlayerMovementAllowed = true;
        carScript.Startup();
        wife.StartCoroutine(wife.MoveIn());
        panelStart.SetActive(false);
    }

    public void Resume()
    {
        Time.timeScale = Wife.RecommendedTime;
        panelPause.SetActive(false);
    }

    public void Stop()
    {
        Time.timeScale = 0f;
        panelPause.SetActive(true);
    }

    public void Restart()
    {
        Wife.PlayerMovementAllowed = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Credits()
    {
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        rotationOffset = Mathf.Sin(Time.time * rotationSpeed * Mathf.Deg2Rad) * rotationRange;

        //buttonStart.transform.rotation = Quaternion.Euler(0, 0, rotationOffset);
        //buttonCredits.transform.rotation = Quaternion.Euler(0, 0, rotationOffset);
        //buttonExit.transform.rotation = Quaternion.Euler(0, 0, rotationOffset);

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if(panelPause.activeSelf)
            {
                Resume();
            }
            else
            {
                Stop();
            }
        }
    }
}
