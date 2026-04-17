using TMPro;
using UnityEngine;

public class TextFloat : MonoBehaviour
{
    private Vector3 startPos;
    private float timer, timer2;
    public int spacing;
    private int textStylePos = 0;
    private TextMeshProUGUI txt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer2 += Time.deltaTime;
        timer += Time.deltaTime;
        timer %= 6.2f;
        transform.position = startPos + (Vector3.up * spacing * (Mathf.Cos(timer)-0.5f));

        if (timer2 > 0.3f)
        {
            switch (textStylePos)
            {
                case 0:
                    txt.fontStyle = FontStyles.Bold | FontStyles.LowerCase;
                    break;
                case 1:
                    txt.fontStyle = FontStyles.LowerCase;
                    break;
                case 2:
                    txt.fontStyle = FontStyles.UpperCase;
                    break;
                case 3:
                    txt.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
                    break;
                default:
                    break;
            }

            textStylePos++;
            textStylePos %= 4;
            timer2 %= 0.3f;
        }
    }
}
