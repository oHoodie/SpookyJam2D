using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxMoveSpeed;
    public float brakeForce; // 0 = The player keeps moving almost forever after let go of key, 1 = instant stop

    private Rigidbody2D rb;
    private FlashlightController flashlight;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flashlight = GetComponentInChildren<FlashlightController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    // FixedUpdate is for Physics Stuff
    void FixedUpdate()
    {
        Move();
        Rotate();
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
}
