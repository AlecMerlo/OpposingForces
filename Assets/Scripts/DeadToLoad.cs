using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadToLoad : MonoBehaviour
{
    private float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1.4f)
        {
            SceneManager.LoadScene("LoadInScene");
        }
    }
}
