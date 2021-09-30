using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyAnimationController : MonoBehaviour
{

    public bool blinking = true;
    [SerializeField] private PhysicsMovementManager physicsMovementManager;
    [SerializeField] private Transform headHolder;
    [SerializeField] private Transform body2Holder;
    [SerializeField] private Transform eyeHolder = null;
    [SerializeField] private Transform mandible1Holder = null;
    [SerializeField] private Transform mandible2Holder = null;
    [SerializeField] private Transform strawHolder = null;
    
    public float footHeight = 10f;
    public float maxTargetDistance = 4f;
    public float targetMoveSpeed = 30f;

    public LegManager[] legManagers;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BlinkEnum());
        legManagers = GetComponentsInChildren<LegManager>();
        for (int i = 0; i < legManagers.Length; i++)
        {
            legManagers[i].footHeight = footHeight;
            legManagers[i].maxTargetDistance = maxTargetDistance;
            legManagers[i].targetMoveSpeed = targetMoveSpeed;
            legManagers[i].oppositeLeg = i % 2 == 0 ? legManagers[i + 1] : legManagers[i - 1];
        }
    }

    private IEnumerator BlinkEnum()
    {
        Vector3 openScale = eyeHolder.transform.localScale;
        Vector3 closedScale = Vector3.zero;
        while (blinking)
        {
            // close eyes for a short time
            eyeHolder.transform.localScale = closedScale;
            yield return new WaitForSeconds(.25f);

            // then open
            eyeHolder.transform.localScale = openScale;

            // wait a random time, repeat
            yield return new WaitForSeconds(Random.Range(2f, 10f));
        }
        // then open
        eyeHolder.transform.localScale = openScale;
    }

    public IEnumerator DrinkEnum()
    {

        // Extend straw
        strawHolder.transform.localScale = Vector3.one;
        transform.LookAt(FindObjectOfType<WaterBowl>().transform);
        yield return new WaitWhile(() => PetManager.instance.drinking);
        strawHolder.transform.localScale = Vector3.zero;
    }

    public void Die()
    {
        StopAllCoroutines();
        body2Holder.localRotation = Quaternion.Euler(-20, 0, 0);
        eyeHolder.transform.localScale = Vector3.zero;
    }
    
}