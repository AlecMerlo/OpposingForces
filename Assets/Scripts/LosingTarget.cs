using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LosingTarget : MonoBehaviour
{
    public Transform player, runner;
    public Image img;
    public GameObject desertingText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.position, runner.position) > 180)
        {
            img.color = new Color(1, 1, 1, img.color.a + (Time.deltaTime * 0.35f));
            desertingText.SetActive(true);
            desertingText.transform.localPosition = new Vector3(-202 + Random.Range(-4,4), 175 + Random.Range(-4, 4), 0);
            if (img.color.a > 0.8f)
            {
                SceneManager.LoadScene("Dead Scene", LoadSceneMode.Single);
            }
        }
        else
        {
            desertingText.SetActive(false);
        }
    }
}
