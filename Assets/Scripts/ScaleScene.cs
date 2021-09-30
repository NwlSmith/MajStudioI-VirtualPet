using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScaleScene : MonoBehaviour
{
    private ARSessionOrigin arSessionOrigin;

    private void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
    }

    private void Update()
    {
        arSessionOrigin.MakeContentAppearAt(transform, transform.position, transform.rotation);
    }
}
