using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public SpriteRenderer blackScreen;
    public float secondsToBlackScreen;
    public GameObject taskUIPrefab;

    private bool isGameOver = false;
    private bool isWon = false;
    private float gameOverCounter;
    private float secondsSinceSceneStart = 0;
    private List<Task> tasks;
    private List<TaskUIController> taskUIControllers;


    private class Task
    {
        public string name;
        public string description;
        public bool completed = false;
        public TaskUIController ui;
    }

    // Start is called before the first frame update
    void Start()
    {
        tasks = new List<Task>();
        tasks.Add(new Task() { name = "Curupira", description = "Take a picture of Curupira" });
        tasks.Add(new Task() { name = "Mula", description = "Take a picture of Mula Sem Cabeça" });
        tasks.Add(new Task() { name = "Leave", description = "Leave the forest once you have the pictures" });

        GameObject taskContainer = GameObject.Find("TaskContainer");
        if (taskContainer != null)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                TaskUIController newTaskUI = Instantiate(taskUIPrefab, taskContainer.transform).GetComponent<TaskUIController>();
                newTaskUI.SetText(tasks[i].description);
                tasks[i].ui = newTaskUI;
            }
        }
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

    public bool canLeave()
    {
        int taskCompletedCount = 0;
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].completed) taskCompletedCount++;
        }
        return taskCompletedCount >= 2;
    }

    public void GameOver()
    {
        isGameOver = true;
    }

    public void Win()
    {
        if (!isWon && !isGameOver)
        {
            CompleteTask("Leave");
            isGameOver = true;
            isWon = true;
        }
        

    }

    public void CompleteTask(string taskName)
    {
        for (int i = 0; i < tasks.Count; i++)
        {
            if (tasks[i].name == taskName && !tasks[i].completed)
            {
                tasks[i].completed = true;
                tasks[i].ui.SetChecked(true);
                Debug.Log("TASK COMPLETED: " + tasks[i].name);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    private void HandleBlackScreeen()
    {
        if (isGameOver)
        {
            gameOverCounter += Time.deltaTime;
            blackScreen.color = new Color(0, 0, 0, gameOverCounter / secondsToBlackScreen);
            
            if (gameOverCounter / secondsToBlackScreen >= 1)
            {
                if (isWon)
                {
                    SceneManager.LoadScene("WinScreen");
                }
                else if(SceneManager.GetActiveScene().name == "MainMenu")
                {
                    SceneManager.LoadScene("pheron");
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
