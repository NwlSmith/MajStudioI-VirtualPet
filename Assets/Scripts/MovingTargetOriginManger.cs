using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTargetOriginManger : MonoBehaviour
{
    private float offset = 4f;
    private bool forward = false;
    private bool backward = false;
    private Rigidbody rb;
    private Vector3 startPos;
    private float leftoffset = 1f;

    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        startPos = transform.localPosition;
        if (transform.parent.localPosition.x > 0)
            leftoffset = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Dot(rb.velocity.normalized, transform.forward) <= 1 && rb.velocity.sqrMagnitude > 4f && !forward)
        {
            forward = true;
            backward = false;
            transform.localPosition = startPos + Vector3.left * -offset * leftoffset;
        }

        else if (Vector3.Dot(rb.velocity.normalized, transform.forward) <= 1 && rb.velocity.sqrMagnitude > 4f && !backward && false)
        {
            backward = true;
            forward = false;
            transform.localPosition = startPos + Vector3.left * offset * leftoffset;
        }
        else if (rb.velocity.sqrMagnitude <= 4f && (forward || backward))
        {
            forward = false;
            backward = false;
            transform.localPosition = startPos;
        }
    }
}
