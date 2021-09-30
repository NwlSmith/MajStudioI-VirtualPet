using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    public int numPrey = 0;

    private int curTimeMultiplier = 1;

    public Transform closeToCamera = null;
    public int amtWater = 100;
    public Text DieText;
    public GameObject RestartButton;
    public Image overlay;
    [SerializeField] private Transform[] RandomLocs = null;
    [SerializeField] private GameObject prey = null;

    private void Awake()
    {

        // Ensure that there is only one instance of the GameManager.
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        DieText.enabled = false;
        RestartButton.SetActive(false);

        StartCoroutine(FadeInEnum());
    }

    private IEnumerator FadeInEnum()
    {
        float duration = .5f;
        float elapsedTime = 0f;
        Color clearWhite = new Color(1, 1, 1, 0);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            overlay.color = Color.Lerp(Color.white, clearWhite, elapsedTime / duration);
            yield return null;
        }
        overlay.color = Color.clear;
    }

    private IEnumerator FadeOutEnum()
    {
        float duration = .5f;
        float elapsedTime = 0f;
        Color clearWhite = new Color(1, 1, 1, 0);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            overlay.color = Color.Lerp(clearWhite, Color.white, elapsedTime / duration);
            yield return null;
        }
        overlay.color = Color.white;

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Comma) && curTimeMultiplier > 1)
        {
            curTimeMultiplier -= 2;
            Debug.Log("Decreasing timescale to " + curTimeMultiplier);
            Time.timeScale = curTimeMultiplier;
        }
        else if (Input.GetKeyDown(KeyCode.Period) && curTimeMultiplier < 7)
        {
            curTimeMultiplier += 2;
            Debug.Log("Increasing timescale to " + curTimeMultiplier);
            Time.timeScale = curTimeMultiplier;
        }

        if (Input.GetKeyDown(KeyCode.P) && numPrey <=0)
        {
            Instantiate(prey, Vector3.zero, Quaternion.identity, transform.parent);
        }

        if (Input.GetKeyDown(KeyCode.F) )
        {
            PetManager.instance.GoToCamera();
        }*/
    }

    public void Die()
    {
        DieText.enabled = true;
        DieText.GetComponent<AudioSource>().Play();
        RestartButton.SetActive(true);

    }

    public void StartRestartEnum()
    {
        bool notpressed = true;
        if (notpressed)
        {
            notpressed = false;
            StartCoroutine(FadeOutEnum());
        }
    }

    public void TimeSlower()
    {
        if (curTimeMultiplier > 1)
        {
            curTimeMultiplier -= 2;
            Debug.Log("Decreasing timescale to " + curTimeMultiplier);
            Time.timeScale = curTimeMultiplier;
        }
    }

    public void TimeFaster()
    {
        if (curTimeMultiplier < 7)
        {
            curTimeMultiplier += 2;
            Debug.Log("Increasing timescale to " + curTimeMultiplier);
            Time.timeScale = curTimeMultiplier;
        }
    }

    public Vector3 RandomLoc()
    {
        return RandomLocs[Random.Range(0, RandomLocs.Length)].position;
    }

    public void SpawnPrey()
    {
        if (numPrey <= 0)
        {
            Instantiate(prey, Vector3.zero, Quaternion.identity, transform.parent);
        }
    }

    public void StartSnore()
    {
        GetComponent<AudioSource>().Play();
    }

    public void StopSnore()
    {
        GetComponent<AudioSource>().Play();
    }
}
