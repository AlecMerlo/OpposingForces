using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadTime : MonoBehaviour
{
    public Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        img.color += (new Color(1,1,1) * 0.6f * Time.deltaTime);
        if(img.color.r >= 0.8f)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
