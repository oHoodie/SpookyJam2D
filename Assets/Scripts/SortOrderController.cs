using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortOrderController : MonoBehaviour
{
    public GameObject referencePoint;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (referencePoint != null)
        {
            sr.sortingOrder = Mathf.RoundToInt(referencePoint.transform.position.y * 100f) * -1;
        }
        else
        {
            sr.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
        }
    }
}
