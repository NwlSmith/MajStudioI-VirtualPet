using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBowl : MonoBehaviour
{

    public GameObject water;
    private AudioSource aS;
    private Vector3 waterInitPos;
    private Vector3 waterEmptyPos;
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        waterInitPos = water.transform.position;
        waterEmptyPos = waterInitPos - Vector3.up * 5f * .01f;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("something entered water");
        if (other.tag.Equals("Player"))
        {
            Debug.Log("Player entered water, does it want to drink?");
            if (PetManager.instance.curBehavior.Equals("drink") && !PetManager.instance.drinking)
            {
                Debug.Log("Player drinking");

                PetManager.instance.Drink();
                StartCoroutine(DrainWater());
            }
        }
    }

    public void FillWater()
    {
        GameManager.instance.amtWater = 100;
        water.transform.position = waterInitPos;
    }

    private IEnumerator DrainWater()
    {
        aS.Play();
        while (PetManager.instance.drinking)
        {
            if (GameManager.instance.amtWater <= 0)
            {
                PetManager.instance.StopDrink();
                break;
            }
            GameManager.instance.amtWater -= 1;
            float fract = GameManager.instance.amtWater / 100f;
            water.transform.position = Vector3.Lerp(waterEmptyPos, waterInitPos, fract);
            yield return new WaitForSeconds(1f);
        }
        aS.Stop();
    }
}
