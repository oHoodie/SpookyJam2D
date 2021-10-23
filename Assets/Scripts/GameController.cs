using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public SpriteRenderer blackScreen;
    public float secondsToBlackScreen;

    private bool isGameOver = false;
    private float gameOverCounter;
    private float secondsSinceSceneStart = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        secondsSinceSceneStart += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameOver = true;
        }

        HandleBlackScreeen();
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    private void HandleBlackScreeen()
    {
        if (isGameOver)
        {
            gameOverCounter += Time.deltaTime;
            if (true)
            {
                blackScreen.color = new Color(0, 0, 0, gameOverCounter / secondsToBlackScreen);
            }
            if (gameOverCounter / secondsToBlackScreen >= 1)
            {
                if(SceneManager.GetActiveScene().name == "MainMenu")
                {
                    SceneManager.LoadScene("TestScene");
                }
                else{
                    SceneManager.LoadScene("MainMenu");
                }
            }
        }
        else
        {
            float alpha = secondsSinceSceneStart >= secondsToBlackScreen ? 0 : 1 - (secondsSinceSceneStart / secondsToBlackScreen);
            blackScreen.color = new Color(0, 0, 0, alpha);
        }
    }
}
