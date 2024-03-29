using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Collider2D))]
public class EnemyController : MonoBehaviour
{
    public string Name;

    public bool isChasing;

    public float acceleration;
    public float maxMoveSpeed;
    public float brakeForce; // 0 = The player keeps moving almost forever after let go of key, 1 = instant stop
    public float nextWaypointDistance;
    public float patrolTime;
    public float patrolRadius;
    public float rotationSpeed = 10;
    public bool flipSprite = false;
    public float distanceForCloseAmbient;
    public float killDistance;
    public float rememberTime = 0.5f;

    private float rememberTimer;

    [Header("Cone of Vision")]

    public int sightRaycasts = 8;
    [Range(0, 360)]
    public int sightAngle = 135;

    public float sightDistance = 5;

    public Transform target;
    public UnityEngine.Experimental.Rendering.Universal.Light2D visionCone;

    private bool reachedEndOfPath;

    private int currentWaypointIndex;

    private float nextPatrolTime;

    private Vector2 direction;
    public Vector2 targetPosition;
    private Rigidbody2D rb;
    private Path path;
    private Seeker seeker;
    private Collider2D coll;
    private AudioController audioController;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        GameObject aC = GameObject.Find("AudioController");
        if(aC != null) audioController = aC.GetComponent<AudioController>();
    }

    private void Start()
    {
        visionCone.pointLightOuterRadius = sightDistance;
        visionCone.pointLightOuterAngle = sightAngle;

        InvokeRepeating("CreatePath", 0, 0.25f);
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0.5f)
        {
            visionCone.transform.rotation = Quaternion.Lerp(visionCone.transform.rotation,
                Quaternion.LookRotation(Vector3.forward, rb.velocity.normalized), rotationSpeed * Time.deltaTime);

            FlipSprite();
        }
        else
        {
            Vector3 euler = visionCone.transform.rotation.eulerAngles;
            euler.z += Mathf.Sin(Time.time * 0.25f) * 0.25f;
            visionCone.transform.rotation = Quaternion.Euler(euler);
        }

        Think();

        KillPlayer();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void KillPlayer()
    {
        if (DistanceToTarget() <= killDistance)
        {
            PlayerController player = target.GetComponent<PlayerController>();
            if (player != null) player.Die();
        }
    }
    private void FlipSprite()
    {
        if (flipSprite)
        {
            if(rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
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

        if (Vector2.Distance(currentWaypoint, rb.position) < nextWaypointDistance)
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
        else if(rememberTimer <= 0) OnPatrol();

        if(rememberTimer > 0) rememberTimer -= Time.deltaTime;

        UpdatePath();

        if (reachedEndOfPath) OnIdle();
    }

    private void OnIdle()
    {
        direction = Vector2.zero;
        if (audioController != null && DistanceToTarget() > distanceForCloseAmbient)
        {
            audioController.SetAudioState(AudioController.AudioState.Normal, Name);
        }
        else if (audioController != null)
        {
            audioController.SetAudioState(AudioController.AudioState.Close, Name);
        }
    }

    private void OnChase()
    {
        rememberTimer = rememberTime;
        isChasing = true;
        targetPosition = target.position;
        if(audioController != null) audioController.SetAudioState(AudioController.AudioState.Chased, Name);
    }

    private void OnPatrol()
    {
        isChasing = false;

        if (Time.time >= nextPatrolTime)
        {
            targetPosition = rb.position + Random.insideUnitCircle * patrolRadius;
            nextPatrolTime = Time.time + patrolTime;
        }
        if (audioController != null && DistanceToTarget() > distanceForCloseAmbient) {
            audioController.SetAudioState(AudioController.AudioState.Normal, Name);
        }
        else if (audioController != null)
        {
            audioController.SetAudioState(AudioController.AudioState.Close, Name);
        }
    }

    private float DistanceToTarget()
    {
        return Vector2.Distance(transform.position, target.transform.position);
    }

    private bool CanSeeTarget()
    {
        int halfSightAngle = sightAngle / 2;
        int angleStep = sightAngle / sightRaycasts;

        Vector2 sightDirection = visionCone.transform.up;

        for (int i = -halfSightAngle; i <= halfSightAngle; i += angleStep)
        {
            Vector2 raycastDirection = Quaternion.Euler(0, 0, i) * sightDirection;
            RaycastHit2D hit = Physics2D.Raycast(coll.ClosestPoint(rb.position + sightDirection), 
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
