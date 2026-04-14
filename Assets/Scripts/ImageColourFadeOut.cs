using UnityEngine;
using UnityEngine.UI;

public class ImageColourFadeOut : MonoBehaviour
{
    private Image img;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = transform.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(img.color.a > 0f)
        {
            img.color = new Color(1,1,1,img.color.a - (Time.deltaTime * 0.2f));
        }
    }
}
