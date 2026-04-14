using NodeCanvas.BehaviourTrees;
using NodeCanvas.Framework;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public BehaviourTreeOwner bTO;
    public Blackboard bB;
    private float timer = 1.7f;
    public Rigidbody rb, playerRb;
    public Transform cameraTra;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bTO.enabled = false;
        bB.enabled = false;
        rb.AddForce(new Vector3(0, 30, 45), ForceMode.Impulse);
        playerRb.AddForce(new Vector3(0, 30, 80), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            bTO.enabled = true;
            bB.enabled = true;
        }
    }
}
