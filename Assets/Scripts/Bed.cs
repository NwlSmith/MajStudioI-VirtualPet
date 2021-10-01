using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something entered cave");
        if (other.tag.Equals("Player") && PetManager.instance.curTired < 80 && (PetManager.instance.behaviorQ.Contains("sleep") || PetManager.instance.curBehavior.Equals("sleep")))
        {
            Debug.Log("Player entered cave, sleeping");
            PetManager.instance.Sleep();
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, PetManager.instance.transform.position) > 12f * GlobalScale.GetScale())
            GameManager.instance.GetComponent<AudioSource>().Stop();
    }
}
