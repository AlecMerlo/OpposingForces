using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private float timer = 0;
    private int textStylePos = 0;
    private TextMeshProUGUI txt;
    private bool hovering;

    private void Start()
    {
        txt = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Update()
    {
        if (hovering)
        {
            timer += Time.deltaTime;

            if (timer > 0.3f)
            {
                switch (textStylePos)
                {
                    case 0:
                        txt.fontStyle = FontStyles.Bold | FontStyles.LowerCase;
                        break;
                    case 1:
                        txt.fontStyle = FontStyles.Bold | FontStyles.UpperCase;
                        break;
                    case 2:
                        txt.fontStyle = FontStyles.UpperCase;
                        break;
                    case 3:
                        txt.fontStyle = FontStyles.LowerCase;
                        break;
                    default:
                        break;
                }

                textStylePos++;
                textStylePos %= 4;
                timer %= 0.3f;
            }
        }
        else
        {
            txt.fontStyle = FontStyles.LowerCase;
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hovering = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textStylePos = 0;
        timer = 0.4f;
        hovering = true;
    }
}
