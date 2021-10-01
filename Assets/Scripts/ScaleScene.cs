using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ScaleScene : MonoBehaviour
{
    [SerializeField] private GameObject contentToScale;
    [SerializeField] private float scaleMultiplier = .01f;
    private ARSessionOrigin arSessionOrigin;
    
    private void Start()
    {
        arSessionOrigin = FindObjectOfType<ARSessionOrigin>();
        
        arSessionOrigin.MakeContentAppearAt(contentToScale.transform, quaternion.identity);
        contentToScale.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, scaleMultiplier);
    }

    private void Update()
    {
        //arSessionOrigin.MakeContentAppearAt(transform, transform.position, transform.rotation);
    }
}
