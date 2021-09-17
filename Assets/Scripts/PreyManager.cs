using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PreyManager : MonoBehaviour
{

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.instance.numPrey++;

        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.SetDestination(GameManager.instance.RandomLoc());
        PetManager.instance.GoToEat(transform);
    }
    public void PlaySound()
    {
        GetComponent<AudioSource>().Play();
    }
    
}
