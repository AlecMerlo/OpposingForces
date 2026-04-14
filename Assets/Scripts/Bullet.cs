using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public AudioClip hurtAC, parryAC;
    public AudioSource auSo;
    public Image hurtImg;
    private float timer;
    
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 130;
        timer += Time.deltaTime;
        if (timer > 10)
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            if (Input.GetMouseButton(1))
            {// parry
                auSo.clip = parryAC;
            }
            else
            {// hurt
                auSo.clip = hurtAC;
                if (hurtImg.color.a > 0.15f)
                {
                    SceneManager.LoadScene("Dead Scene", LoadSceneMode.Single);
                }
                hurtImg.color = new Color(1, 1, 1, 0.75f);
            }
            auSo.Play();
        }
    }
}
