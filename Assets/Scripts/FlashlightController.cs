using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    public AudioClip pictureFailure;
    public GameObject targetPoint;
    public Text beingChasedText;

    private Light2D spotlight;

    private bool isTakingPicture;
    private float picAnimCounter;
    private bool photoTaken = false;

    private PlaySoundFromList camSound;

    // Start is called before the first frame update
    void Start()
    {
        beingChasedText.enabled = false;

        spotlight = GetComponentInChildren<Light2D>();
        camSound = GetComponent<PlaySoundFromList>();
    }

    // Update is called once per frame
    void Update()
    {
        // Take a picture
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !isTakingPicture)
        {
            isTakingPicture = true;
            camSound.PlayRandom();
            TurnOn(true);
        }

        HandleTakePicture();
        

    }

    private void HandleTakePicture()
    {
        if (isTakingPicture)
        {
            picAnimCounter += Time.deltaTime;
            if (picAnimCounter >= 0.4f)
            {
                spotlight.pointLightInnerAngle = 75;

                if (!photoTaken)
                {
                    if (HasEnemyInPicture())
                    {
                        Debug.Log("PICTURE SUCCESS");
                    }
                    else
                    {
                        GetComponent<AudioSource>().clip = pictureFailure;
                        GetComponent<AudioSource>().Play();
                        Debug.Log("PICTURE FAILURE");
                    }
                    photoTaken = true;
                }
            }
            if (picAnimCounter >= 1f)
            {
                TurnOn(false);
                picAnimCounter = 0;
                spotlight.pointLightInnerAngle = 0;
                isTakingPicture = false;
                photoTaken = false;
            }
        }
    }

    IEnumerator BeingChased()
    {
        beingChasedText.enabled = true;

        yield return new WaitForSeconds(3);

        beingChasedText.enabled = false;
    }

    Coroutine beingChasedRoutine;

    private bool HasEnemyInPicture()
    {
        int sightAngle = 120;
        float sightDistance = 3;

        int halfSightAngle = sightAngle / 2;
        int angleStep = sightAngle / 15;

        Vector2 targetDirection = ((Vector2)targetPoint.transform.position - (Vector2)spotlight.transform.position).normalized;

        for (int i = -halfSightAngle; i <= halfSightAngle; i += angleStep)
        {
            Vector2 raycastDirection = Quaternion.Euler(0, 0, i) * targetDirection;
            Debug.DrawLine(spotlight.transform.position, spotlight.transform.position + (Vector3)raycastDirection.normalized * sightDistance, Color.yellow, 2f);
            //origin, direction, distance, layermask
            RaycastHit2D hit = Physics2D.Raycast(spotlight.transform.position, raycastDirection, sightDistance);

            if (hit.collider != null && LayerMask.LayerToName(hit.transform.gameObject.layer) == "Enemy")
            {
                EnemyController enemyController = hit.collider.GetComponent<EnemyController>();
                GameController gameController = GameObject.Find("GameController").
                    GetComponent<GameController>();

                if (enemyController != null)
                {
                    string taskName = enemyController.Name;

                    if (gameController.IsTaskCompleted(taskName)) return false;

                    if (enemyController.isChasing)
                    {
                        if (beingChasedRoutine != null) StopCoroutine(beingChasedRoutine);
                        beingChasedRoutine = StartCoroutine(BeingChased());
                        return false;
                    }

                    enemyController.targetPosition = transform.position;

                    Debug.Log("Captured on cam: " + hit.transform.gameObject.name);
                    gameController.CompleteTask(taskName);
                    return true;
                }

            }
        }

        return false;
    }


    private void TurnOn(bool value)
    {
        spotlight.enabled = value;
    }
}
