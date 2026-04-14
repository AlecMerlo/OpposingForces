using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public float swingTime;
    public Image swingBar;
    public AudioSource auSo;
    public AudioClip hitSound, missSound;
    public Transform sword;
    private float sliceTime = 1;
    public TrailRenderer tR;

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
                sliceTime = 0;
                Debug.Log("Hit");
            }
            else
            {
                auSo.clip = missSound;
                auSo.volume = 1;
                auSo.Play();
                sliceTime = 0;
                Debug.Log("Miss");
            }
        }

        if (swingTimer > 0)
        {
            swingTimer -= Time.deltaTime;
        }

        if (swingBar != null) { swingBar.fillAmount = (swingTime - swingTimer) / swingTime; }

        if (sliceTime < 1)
        {
            sliceTime += Time.deltaTime * 3.5f;
            sword.localEulerAngles = Vector3.Lerp(new Vector3(20, 0, 0), new Vector3(76, 0, 134), sliceTime);
            tR.enabled = true;
        }
        else
        {
            tR.enabled = false;
        }
    }
}
