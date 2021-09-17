using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceSound : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 2f);
        }
    }
}
