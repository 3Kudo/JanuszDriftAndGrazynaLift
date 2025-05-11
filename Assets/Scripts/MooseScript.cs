using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MooseScript : MonoBehaviour
{
    private GameObject car, wife;
    [SerializeField] private float speed;
    [SerializeField] private float activationDistance;
    [SerializeField] private float ogPosition;
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem explosion;

    public Action mooseHit;

    private enum State 
    { 
        Idle,
        Activated,
        Hit
    }

    [SerializeField] private State state;

    void Start()
    {
        car = GameObject.Find("Car");
        wife = GameObject.Find("Wife");

        mooseHit = wife.GetComponent<Wife>().healthDecrese;

        state = State.Idle;
        ogPosition = transform.position.y;

        CameraScript camera = GameObject.Find("Car").transform.Find("Main Camera").gameObject.GetComponent<CameraScript>();
        mooseHit += camera.Shake;
    }

    // Update is called once per frame
    void Update()
    {
        //Activate if player is near
        if (car != null)
        {
            if (transform.position.y - car.transform.position.y < activationDistance && state == State.Idle)
            {
                state = State.Activated;
            }
        }
        //Go left if activated
        if (state == State.Activated)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        //Jumping
        if (transform.position.y <= ogPosition && state != State.Hit)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Car") && state != State.Hit)
        {
            mooseHit?.Invoke();
            Vector3 middlePosition = (collision.transform.position + transform.position)/2;
            Instantiate(explosion, middlePosition, Quaternion.Euler(Vector3.zero));
            state = State.Hit;
            rb.velocity = Vector2.zero;
            rb.AddForce(new Vector2(-20, 28), ForceMode2D.Impulse);
            rb.AddTorque(UnityEngine.Random.Range(-1000,1000));
            GameObject.Find("Car").GetComponentInChildren<Audio>().mooseCrash();
        }
    }
}
