using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject objectToFollow;
    public float followSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float camZ = transform.position.z;
        Vector3 lerrpVec = Vector3.Lerp(transform.position, objectToFollow.transform.position, followSpeed);
        transform.position = new Vector3(lerrpVec.x, lerrpVec.y, camZ);
    }
}
