using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    public event Action obstacleHit;
    [SerializeField] Rigidbody2D rb;
    private bool hit;
    [SerializeField] ParticleSystem explosion;
    private GameObject wife;

    void Start()
    {
        CameraScript camera = GameObject.Find("Car").transform.Find("Main Camera").gameObject.GetComponent<CameraScript>();
        obstacleHit += camera.Shake;
        wife = GameObject.Find("Wife");
        obstacleHit = wife.GetComponent<Wife>().healthDecrese;
    }
    private void GotHit()
    {
        rb.gravityScale = 5;
        hit = true;
        rb.AddForce(new Vector2(UnityEngine.Random.Range(-20, 20), 28), ForceMode2D.Impulse);
        rb.AddTorque(UnityEngine.Random.Range(-1000, 1000));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Car") && !hit)
        {
            GotHit();
            Vector3 middlePosition = (collision.transform.position + transform.position) / 2;
            Instantiate(explosion, middlePosition, Quaternion.Euler(Vector3.zero));
            obstacleHit?.Invoke();
            GameObject.Find("Car").GetComponentInChildren<Audio>().playObstacleCrash();
        }
    }
}
