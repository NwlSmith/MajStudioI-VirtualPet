using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PetManager : MonoBehaviour
{
    public static PetManager instance = null;

    public float curHunger = 100;
    private float maxHunger = 100;
    public float curThirst = 100;
    private float maxThirst = 100;
    public float curTired = 100;
    private float maxTired = 100;
    [SerializeField] private Slider hungerSlider = null;
    [SerializeField] private Slider thirstSlider = null;
    [SerializeField] private Slider energySlider = null;

    public Camera captureCam;

    public bool eating = false;
    public bool drinking = false;
    public bool sleeping = false;

    public Transform waterTrans;
    public Transform bedTrans;

    public bool busy = false;
    private NavMeshAgent navMeshAgent;
    public List<string> behaviorQ;
    public string curBehavior;
    public Transform preyTransform;
    public BodyAnimationController bodyAnimationController;

    private void Awake()
    {
        // Ensure that there is only one instance of the PetManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(NeedsEnum());
        NextBahavior();
        bodyAnimationController = FindObjectOfType<BodyAnimationController>();
    }

    public void NextBahavior()
    {
        if (behaviorQ.Count > 0)
        {
            curBehavior = behaviorQ[0];
            behaviorQ.RemoveAt(0);

            switch (curBehavior)
            {
                case "eat":
                    Debug.Log("Going to eat");
                    StartCoroutine(GoToEatEnum(preyTransform));
                    break;
                case "drink":
                    Debug.Log("Going to drink");
                    GoToDrink();
                    break;
                case "sleep":
                    Debug.Log("Going to sleep");
                    GoToSleep();
                    break;
                case "gotocamera":
                    Debug.Log("Going to camera");
                    GoToCameraBehavior();
                    break;
                case "wander":
                    Debug.Log("Going to wander");
                    Wander();
                    break;
                default:
                    Debug.Log("incorrect behavior q val");
                    break;
            }
        }
        else
        {
            behaviorQ.Add("wander");
            NextBahavior();
        }
    }

    public void Wander()
    {
        curBehavior = "wander";
        StartCoroutine(WanderEnum());
    }

    private IEnumerator WanderEnum()
    {
        Vector3 targetPos = GameManager.instance.RandomLoc();
        for (int i = 0; i < 20; i++)
        {
            if (Vector3.Distance(transform.position, targetPos) <= 10)
            {
                break;
            }
            navMeshAgent.SetDestination(targetPos);
            yield return new WaitForSeconds(2.0f);
        }
        behaviorQ.Remove("wander");
        NextBahavior();

    }



    public void GoToEat(Transform preyTrans)
    {
        preyTransform = preyTrans;
        PetManager.instance.behaviorQ.Insert(0, "eat");
    }

    private IEnumerator GoToEatEnum(Transform preyTrans)
    {
        eating = true;
        bool found = false;
        for (int i = 0; i < 20; i++) // FIGURE THIS OUT
        {
            if (Vector3.Distance(transform.position, preyTrans.position) <= 8)
            {
                found = true;
                break;
            }
            navMeshAgent.SetDestination(preyTrans.position);
            yield return new WaitForSeconds(2.0f);
        }
        preyTrans.GetComponent<NavMeshAgent>().isStopped = true;
        preyTrans.localScale = Vector3.zero;
        Destroy(preyTrans.gameObject, 2);

        if (found)
        {
            GameManager.instance.numPrey--;
            curHunger = Mathf.Clamp(curHunger + 50, 0, maxHunger);
            preyTrans.gameObject.GetComponent<PreyManager>().PlaySound();
            GetComponent<AudioSource>().Play();
        }
        eating = false;
        busy = false;

        behaviorQ.Remove("eat");
        NextBahavior();
    }



    public void GoToDrink()
    {
        busy = true;
        navMeshAgent.SetDestination(waterTrans.position);
    }

    public void Drink()
    {
        navMeshAgent.isStopped = true;
        // NEED DRINK ANIM
        drinking = true;
        StartCoroutine(bodyAnimationController.DrinkEnum());
    }

    public void StopDrink()
    {
        navMeshAgent.isStopped = false;
        curThirst = maxThirst;
        drinking = false;
        // CHOOSE ANOTHER ACTIVITY
        behaviorQ.Remove("drink");
        NextBahavior();
    }



    public void GoToSleep()
    {
        busy = true;
        navMeshAgent.SetDestination(bedTrans.position);
    }

    public void Sleep()
    {
        navMeshAgent.isStopped = true;
        // NEED SLEEP ANIM
        sleeping = true;
        GameManager.instance.StartSnore();
    }

    public void WakeUp()
    {
        navMeshAgent.isStopped = false;
        // NEED SLEEP ANIM
        sleeping = false;
        behaviorQ.Remove("sleep");
        NextBahavior();
        GameManager.instance.StopSnore();

        // CHOOSE NEW THING TO DO
    }

    public void GoToCamera()
    {
        behaviorQ.Add("gotocamera");
    }

    public void GoToCameraBehavior()
    {
        busy = true;
        navMeshAgent.SetDestination(GameManager.instance.closeToCamera.position);
        StartCoroutine(CheckIfAtDest(GameManager.instance.closeToCamera.position));
        Invoke("LeaveCamera", 15f);
    }

    public void LeaveCamera()
    {
        busy = false;
        NextBahavior();
    }

    public IEnumerator CheckIfAtDest(Vector3 dest)
    {
        for (int i = 0; i < 20f; i++)
        {
            if (Vector3.Distance(transform.position, dest) <= 8)
            {
                break;
            }
            yield return new WaitForSeconds(1f);
        }
        busy = false;
    }

    private void Die()
    {
        Debug.Log("Die");
        navMeshAgent.isStopped = true;
        transform.rotation = Quaternion.Euler(180, 0, 0);
        StopAllCoroutines();
        GetComponent<MovementManager>().enabled = false;
        GetComponent<PhysicsMovementManager>().enabled = false;
        bodyAnimationController.Die();
        GameManager.instance.Die();
    }

    private IEnumerator NeedsEnum()
    {
        bool looping = true;
        while (looping)
        {
            yield return new WaitForSeconds(1f); // CHANGE THIS VALUE

            curHunger -= .35f;
            hungerSlider.value = curHunger;
            // if < 0, DIE
            if (curHunger <= 0)
            {
                if (GameManager.instance.numPrey <= 0)
                {
                    looping = false;
                    Die();
                }
                else
                {
                    curHunger = 1f;

                }
            }

            // UPDATE UI SLIDER

            if (drinking)
            {
                curThirst += 5f;
                if (curThirst >= maxThirst)
                {
                    StopDrink();
                }
            }
            else
            {
                curThirst = Mathf.Clamp(curThirst - .75f, 0f, maxThirst);

                if (curThirst < 0 && !curBehavior.Equals("drink"))
                {
                    Die();
                    looping = false;
                }
                else if (curThirst < 50 && GameManager.instance.amtWater > 0 && !curBehavior.Equals("drink") && !behaviorQ.Contains("drink"))
                {
                    behaviorQ.Add("drink");
                }
            }

            thirstSlider.value = curThirst;

            if (sleeping)
            {
                curTired += 5f;
                if (curTired >= maxTired)
                {
                    curTired = maxTired;
                    WakeUp();
                    // CHOOSE ANOTHER ACTIVITY
                }
            }
            else
            {
                curTired = Mathf.Clamp(curTired - .25f, 0f, maxTired);

                if (curTired < 25 && !behaviorQ.Contains("sleep") && !behaviorQ.Contains("sleep"))
                {
                    behaviorQ.Insert(0, "sleep");
                }
            }

            energySlider.value = curTired;
        }
    }
}
