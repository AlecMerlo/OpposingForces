using NodeCanvas.Framework;
using NodeCanvas.Tasks.Actions;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public float swingTime;
    public Image swingBar;
    public AudioSource auSo, auSo2;
    public AudioClip hitSound, missSound;
    public AudioClip dun1, dun2;
    public Transform sword;
    private float sliceTime = 1;
    public TrailRenderer tR;
    public ParticleSystem pS;
    public int hits = 0;

    public GameObject hitImg, bloodParticles;

    private float swingTimer;

    public GameObject tTheyre, tGoing, tFaster;

    public Blackboard bbRun, bbChase;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && swingTimer <= 0)
        {
            swingTimer = swingTime;

            RaycastHit hit = new RaycastHit();

            LayerMask mask = LayerMask.GetMask("RunAway");

            if (Physics.SphereCast(transform.position, 4, transform.forward, out hit, 3, mask))
            {
                hits++;
                auSo.clip = hitSound;
                auSo.volume = 0.4f;
                auSo.ignoreListenerPause = true;
                auSo2.ignoreListenerPause = true;
                auSo.Play();
                sliceTime = 0;
                AudioListener.pause = true;
                hitImg.SetActive(true);
                switch (hits)
                {
                    case 1:
                        bbRun.GetVariable("farSpeed").value = 87;
                        bbRun.GetVariable("closeSpeed").value = 70;

                        bbRun.GetVariable("firingSpeedClose").value = 0.4f;
                        bbRun.GetVariable("firingSpeedMedium").value = 0.8f;
                        bbRun.GetVariable("firingSpeedFar").value = 1.4f;

                        bbChase.GetVariable("farSpeed").value = 500;
                        bbChase.GetVariable("closeSpeed").value = 650;
                        Time.timeScale = 0.1f;
                        StartCoroutine(TheyreGettingFaster());
                        break;
                    case 2:
                        bbRun.GetVariable("farSpeed").value = 95;
                        bbRun.GetVariable("closeSpeed").value = 80;

                        bbRun.GetVariable("firingSpeedClose").value = 0.15f;
                        bbRun.GetVariable("firingSpeedMedium").value = 0.3f;
                        bbRun.GetVariable("firingSpeedFar").value = 0.4f;

                        bbChase.GetVariable("farSpeed").value = 550;
                        bbChase.GetVariable("closeSpeed").value = 750;
                        Time.timeScale = 0;
                        StartCoroutine(Pause());
                        break;
                    case 3:
                        Time.timeScale = 0.1f;
                        GameObject gO2 = Instantiate(bloodParticles);
                        gO2.transform.parent = bbChase.transform.parent;
                        gO2.transform.position = bbChase.transform.position;
                        gO2.SetActive(true);
                        bbRun.gameObject.SetActive(false);
                        bbChase.gameObject.SetActive(false);
                        StartCoroutine(Pause());
                        break;
                    default:
                        Time.timeScale = 0;
                        StartCoroutine(Pause());
                        break;
                }
                GameObject gO = Instantiate(pS.gameObject, pS.transform.parent);
                gO.SetActive(true);
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

    public IEnumerator TheyreGettingFaster()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        tTheyre.SetActive(true);
        auSo2.clip = dun1;
        auSo2.Play();
        yield return new WaitForSecondsRealtime(0.6f);
        tTheyre.SetActive(false);
        tGoing.SetActive(true);
        auSo2.Play();
        yield return new WaitForSecondsRealtime(0.6f);
        tGoing.SetActive(false);
        tFaster.SetActive(true);
        auSo2.clip = dun2;
        auSo2.Play();

        for (float i = 0; i < 0.6f; i += 0.04f)
        {
            yield return new WaitForSecondsRealtime(0.03f);
            tFaster.transform.localPosition = new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), 0);
        }

        tFaster.SetActive(false);

        Time.timeScale = 1;
        AudioListener.pause = false;
        hitImg.SetActive(false);
        auSo.ignoreListenerPause = false;
    }

    public IEnumerator Pause()
    {
        while (Time.timeScale < 1)
        {
            yield return new WaitForSecondsRealtime(0.03f);
            Time.timeScale += 0.05f;
        }
        Time.timeScale = 1;
        AudioListener.pause = false;
        hitImg.SetActive(false);
        auSo.ignoreListenerPause = false;
    }
}
