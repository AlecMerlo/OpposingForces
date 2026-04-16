using NodeCanvas.Framework;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Slice : MonoBehaviour
{
    public float swingTime;
    public Image swingBar;
    public AudioSource bgMusic, auSo, auSo2, auSo3;
    public AudioClip hitSound, missSound;
    public AudioClip dun1, dun2, meow, gore;
    public Transform sword;
    private float sliceTime = 1;
    public TrailRenderer tR;
    public ParticleSystem pS;
    public int hits = 0;

    public GameObject catObj;

    public GameObject hitImg, bloodParticles;

    private float swingTimer;

    public GameObject tTheyre, tGoing, tFaster, tThey, tStole, tYour, tCat;

    public GameObject pP1, pP2;

    public Blackboard bbRun, bbChase;

    private bool timerOn = true;
    private float timerTime = 0;
    public TextMeshProUGUI timerText;

    void Update()
    {
        if (timerOn)
        {
            timerTime += Time.deltaTime;
            timerText.text = timerTime.ToString("#.00");
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene("LoadInScene", LoadSceneMode.Single);
        }

        if (Input.GetMouseButtonDown(0) && swingTimer <= 0)
        {
            swingTimer = swingTime;

            LayerMask mask = LayerMask.GetMask("RunAway");

            LayerMask maxwMask = LayerMask.GetMask("Cat");

            if (Physics.Raycast(transform.position, transform.forward, 7, maxwMask))
            {
                SceneManager.LoadScene("LoadInScene", LoadSceneMode.Single);
            }

            if (Physics.Raycast(transform.position, transform.forward, 8, mask))
            {
                hits++;
                auSo.clip = hitSound;
                auSo.volume = 0.4f;
                auSo.ignoreListenerPause = true;
                auSo2.ignoreListenerPause = true;
                auSo3.ignoreListenerPause = true;
                auSo.Play();
                sliceTime = 0;
                AudioListener.pause = true;
                hitImg.SetActive(true);

                bbRun.GetComponent<Rigidbody>().linearVelocity *= 200;

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

                        pP1.SetActive(true);

                        StartCoroutine(TheyreGettingFaster());
                        break;
                    case 2:
                        bbRun.GetVariable("farSpeed").value = 100;
                        bbRun.GetVariable("closeSpeed").value = 80;

                        bbRun.GetVariable("firingSpeedClose").value = 0.05f;
                        bbRun.GetVariable("firingSpeedMedium").value = 0.2f;
                        bbRun.GetVariable("firingSpeedFar").value = 0.3f;

                        bbChase.GetVariable("farSpeed").value = 550;
                        bbChase.GetVariable("closeSpeed").value = 750;

                        Time.timeScale = 0.1f;

                        StartCoroutine(TheyStoleYourCat());
                        break;
                    case 3:
                        timerOn = false;
                        Time.timeScale = 0.1f;
                        bloodParticles.transform.position = new Vector3(pS.gameObject.transform.position.x, 0, pS.gameObject.transform.position.z);
                        bloodParticles.SetActive(true);
                        bbRun.gameObject.SetActive(false);
                        bbChase.gameObject.SetActive(false);
                        pP1.SetActive(false);
                        pP2.SetActive(false);
                        bgMusic.Stop();
                        auSo2.clip = gore;
                        auSo2.volume = 0.3f;
                        auSo2.Play();
                        catObj.transform.position = new Vector3(pS.gameObject.transform.position.x, 0, pS.gameObject.transform.position.z);
                        catObj.SetActive(true);
                        catObj.GetComponent<AudioSource>().ignoreListenerPause = true;
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
        yield return new WaitForSecondsRealtime(0.2f);
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

    public IEnumerator TheyStoleYourCat()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        auSo2.clip = dun1;
        auSo2.Play();
        tThey.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        auSo2.Play();
        tThey.SetActive(false);
        tStole.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        auSo2.Play();
        tStole.SetActive(false);
        tYour.SetActive(true);
        yield return new WaitForSecondsRealtime(0.6f);
        auSo2.clip = dun2;
        auSo2.Play();
        auSo3.clip = meow;
        auSo3.Play();
        tYour.SetActive(false);
        tCat.SetActive(true);
        bgMusic.volume = 0.1f;
        bgMusic.pitch = 1.2f;

        for (float i = 0; i < 0.6f; i += 0.04f)
        {
            yield return new WaitForSecondsRealtime(0.03f);
            tCat.transform.localPosition = new Vector3(Random.Range(-40, 40), Random.Range(-40, 40), 0);
        }

        tCat.SetActive(false);

        pP2.SetActive(true);

        Time.timeScale = 1;
        AudioListener.pause = false;
        hitImg.SetActive(false);
        auSo.ignoreListenerPause = false;
    }

    public IEnumerator Pause()
    {
        while (Time.timeScale < 1)
        {
            yield return new WaitForSecondsRealtime(0.025f);
            Time.timeScale += 0.01f;
        }
        Time.timeScale = 1;
        AudioListener.pause = false;
        hitImg.SetActive(false);
        auSo.ignoreListenerPause = false;
    }
}
