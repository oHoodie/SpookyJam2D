using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    public float acceleration;
    public float maxMoveSpeed;
    public float brakeForce; // 0 = The player keeps moving almost forever after let go of key, 1 = instant stop
    public float nextWaypointDistance;
    public float patrolTime;
    public float patrolRadius;

    [Header("Cone of Vision")]

    public int sightRaycasts = 8;
    [Range(0, 360)]
    public int sightAngle = 135;

    public float sightDistance = 5;

    public Transform target;

    private bool reachedEndOfPath;

    private int currentWaypointIndex;

    private float nextPatrolTime;

    private Vector2 direction;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Path path;
    private Seeker seeker;
    private Collider2D coll;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        InvokeRepeating("CreatePath", 0, 0.25f);
    }

    void Update()
    {
        Think();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void CreatePath()
    {
        seeker.StartPath(rb.position, targetPosition, OnPathComplete);
    }

    private void UpdatePath()
    {
        if (path == null) return;

        if(currentWaypointIndex >= path.vectorPath.Count)
        {
            direction = Vector2.zero;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 currentWaypoint = path.vectorPath[currentWaypointIndex];
        direction = (currentWaypoint - rb.position).normalized;

        if(Vector2.Distance(currentWaypoint, rb.position) < nextWaypointDistance)
        {
            currentWaypointIndex++;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypointIndex = 0;
        }
    }

    private void Think()
    {
        if (CanSeeTarget()) OnChase();
        else OnPatrol();

        UpdatePath();

        if (reachedEndOfPath) OnIdle();
    }

    private void OnIdle()
    {
        direction = Vector2.zero;
    }

    private void OnChase()
    {
        targetPosition = target.position;
    }

    private void OnPatrol()
    {
        if (Time.time >= nextPatrolTime)
        {
            targetPosition = rb.position + Random.insideUnitCircle * patrolRadius;
            nextPatrolTime = Time.time + patrolTime;
        }
    }

    private bool CanSeeTarget()
    {
        int halfSightAngle = sightAngle / 2;
        int angleStep = sightAngle / sightRaycasts;

        Vector2 targetDirection = ((Vector2)target.position - rb.position).normalized;

        for (int i = -halfSightAngle; i <= halfSightAngle; i += angleStep)
        {
            Vector2 raycastDirection = Quaternion.Euler(0, 0, i) * targetDirection;
            RaycastHit2D hit = Physics2D.Raycast(coll.ClosestPoint(rb.position + targetDirection), 
                raycastDirection, sightDistance);

            if (hit.transform == target.transform) return true;
        }

        return false;
    }

    private void Move()
    {
        rb.AddForce(new Vector2(direction.x * acceleration, direction.y * acceleration));

        // Speed Limit
        if (rb.velocity.magnitude > maxMoveSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxMoveSpeed;
        }

        // Brakes
        rb.velocity = new Vector2(direction.x == 0 ? rb.velocity.x * brakeForce : rb.velocity.x, direction.y == 0 ? 
            rb.velocity.y * brakeForce : rb.velocity.y);
    }
}
