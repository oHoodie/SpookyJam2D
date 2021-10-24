using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour
{

    private float opacity = 1;
    public bool opacityRising = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (opacityRising)
        {
            opacity += Time.deltaTime / 2;
        }
        else
        {
            opacity -= Time.deltaTime / 2;
        }

        if (opacity > 1) {
            opacity = 1;
            opacityRising = false;
        }
        else if (opacity < 0.5f)
        {
            opacity = 0.5f;
            opacityRising = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("WIIIIN");
            GameController gc = GameObject.Find("GameController").GetComponent<GameController>();
            if (gc.canLeave()) gc.Win();
            else
            {
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
