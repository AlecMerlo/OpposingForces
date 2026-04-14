using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadTime : MonoBehaviour
{
    public GameObject make, them, bleed;
    private float timer;
    public AudioSource aS;
    public AudioClip oneTwo, three;
    public int beat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (beat == 0 && timer > 0.1f)
        {
            make.SetActive(true);
            aS.PlayOneShot(oneTwo);
            beat++;
            timer = 0;
        }
        if (beat == 1 && timer > 0.5f)
        {
            make.SetActive(false);
        }
        if (beat == 1 && timer > 0.7f)
        {
            them.SetActive(true);
            aS.PlayOneShot(oneTwo);
            beat++;
            timer = 0;
        }
        if (beat == 2 && timer > 0.5f)
        {
            them.SetActive(false);
        }
        if (beat == 2 && timer > 0.7f)
        {
            bleed.SetActive(true);
            aS.PlayOneShot(three);
            beat++;
            timer = 0;
        }
        if (beat == 3 && timer > 1.1f)
        {
            SceneManager.LoadScene("SampleScene");
        }
        bleed.transform.localPosition = new Vector3(1400 + Random.Range(-4, 4), -200 + Random.Range(-4, 4), 0);
    }
}
