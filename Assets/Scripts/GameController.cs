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
    private List<Task> tasks;

    private class Task
    {
        public string name;
        public string description;
        public bool completed = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        tasks = new List<Task>();
        tasks.Add(new Task() { name = "Curupira", description = "Take a picture of Curupira" });
        tasks.Add(new Task() { name = "Mula Sem Cabeca", description = "Take a picture of Mula Sem Cabeca" });
        tasks.Add(new Task() { name = "Leave", description = "Leave the forest once you have the pictures" });
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

    public void CompleteTask(string taskName)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].name == taskName && !tasks[i].completed)
            {
                tasks[i].completed = true;
                Debug.Log("TASK COMPLETED: " + tasks[i].name);
            }
        }
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
