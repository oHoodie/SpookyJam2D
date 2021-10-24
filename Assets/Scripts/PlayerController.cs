using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxMoveSpeed;
    public float brakeForce; // 0 = The player keeps moving almost forever after let go of key, 1 = instant stop
    public float timeBetweenFootsteps;

    private Rigidbody2D rb;
    private FlashlightController flashlight;
    private PlaySoundFromList footsteps;

    private float footstepCounter = 0;
    private bool isDead = false;


// Start is called before the first frame update
void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashlight = GetComponentInChildren<FlashlightController>();
        footsteps = GetComponent<PlaySoundFromList>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleFootsteps();
    }

    // FixedUpdate is for Physics Stuff
    void FixedUpdate()
    {
        if (!isDead)
        {
            Move();
            Rotate();
        }

    }

    
    public void Die()
    {
        isDead = true;
        GameObject.Find("GameController").GetComponent<GameController>().GameOver();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        rb.AddForce(new Vector2(moveX * acceleration, moveY * acceleration));

        // Speed Limit
        if (rb.velocity.magnitude > maxMoveSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMoveSpeed;
        }

        // Brakes
        rb.velocity = new Vector2(moveX == 0 ? rb.velocity.x * brakeForce : rb.velocity.x, moveY == 0 ? rb.velocity.y * brakeForce : rb.velocity.y);
    }

    private void Rotate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 perpendicular = mousePos - transform.position;
        flashlight.transform.rotation = Quaternion.LookRotation(Vector3.forward, perpendicular) ;

    }

    private void HandleFootsteps()
    {
        footstepCounter -= Time.deltaTime;
        if(footstepCounter <= 0 && rb.velocity.magnitude > 1)
        {
            footsteps.PlayRandom();
            footstepCounter = timeBetweenFootsteps;
        }
    }
}
