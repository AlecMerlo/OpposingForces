using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public float swingTime;
    public Image swingBar;
    public AudioSource auSo;
    public AudioClip hitSound, missSound;

    private float swingTimer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingTimer <= 0)
        {
            swingTimer = swingTime;

            RaycastHit hit = new RaycastHit();

            LayerMask mask = LayerMask.GetMask("RunAway");

            if (Physics.SphereCast(transform.position, 4, transform.forward, out hit, 3, mask))
            {
                auSo.clip = hitSound;
                auSo.volume = 0.4f;
                auSo.Play();
                Debug.Log("Hit");
            }
            else
            {
                auSo.clip = missSound;
                auSo.volume = 1;
                auSo.Play();
                Debug.Log("Miss");
            }
        }

        if (swingTimer > 0)
        {
            swingTimer -= Time.deltaTime;
        }

        if (swingBar != null) { swingBar.fillAmount = (swingTime - swingTimer) / swingTime; }
    }
}
