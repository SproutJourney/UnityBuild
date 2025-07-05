using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    void Start()
    {
       pauseMenu.SetActive(false); 
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }        
    }

    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void resumeGame()
    {
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
    }

    public void quitApp()
    {
        Application.Quit();
    }
    }
