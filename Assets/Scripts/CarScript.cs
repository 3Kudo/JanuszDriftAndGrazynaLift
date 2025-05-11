using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class CarScript : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float topSpeed;
    [SerializeField] private float maxTopSpeed;
    [SerializeField] private float topSpeedSpeed;
    public float speed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float startupSpeed;
    [SerializeField] private float defaultSpeed;
    [Header("Other")]
    public State state;
    [SerializeField] private GameObject carSprite;
    [SerializeField] private float targetAngle;
    [SerializeField] private TrailRenderer trail1;
    [SerializeField] private TrailRenderer trail2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSound;
    [SerializeField] private GameObject camera;
    [SerializeField] private ParticleSystem gas;
    private bool isPlayingTurningSfx = false;

    public Wife wife;

    private Vector2 startTouchPosition, endTouchPosition;

    public enum State
    {
        Running,
        Breaking,
        GameOver,
        Startup,
        Crash
    }

    private void Start()
    {
        state = State.Startup;
        targetAngle = carSprite.transform.localEulerAngles.z;
    }
    // Update is called once per frame
    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //Startup stuff
        if (UnityEngine.Input.GetKeyDown(KeyCode.Y) && state == State.Startup)
        {
            Startup();
        }
        if (state == State.Startup && speed > 0)
        {
            topSpeed = Mathf.Lerp(topSpeed, defaultSpeed, 0.01f);

            if (topSpeed < defaultSpeed + 1)
            {
                state = State.Running;
            }
        }
        //Get input
        if (state != State.GameOver && state != State.Startup)
        {
            if (UnityEngine.Input.GetKey(KeyCode.Space))
            {
                if (state == State.Running)
                {
                    gas.Stop();
                   // audioSource.PlayOneShot(breakSound);
                }
                if (speed < 1)
                {
                    audioSource.Stop(); 
                }
                state = State.Breaking;
            }
            else
            {
                if (!gas.isPlaying)
                {
                    gas.Play();
                }
                //audioSource.Stop();
                state = State.Running;
            }
        }

        //Go if not braking
        if (state == State.Running || state == State.Startup)
        {
            speed = Mathf.Clamp(speed + Time.deltaTime * acceleration, 0, topSpeed);
        }
        else
        {
            speed = Mathf.Clamp(speed - Time.deltaTime * acceleration, 0, topSpeed);
        }
        //Turning
        if (state == State.Running)
        {
            if(UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startTouchPosition = UnityEngine.Input.GetTouch(0).position;
            }

            if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endTouchPosition = UnityEngine.Input.GetTouch(0).position;

                if (endTouchPosition.x < startTouchPosition.x)
                {
                    targetAngle += 90;
                }
                else if (endTouchPosition.x > startTouchPosition.x)
                {
                    targetAngle += -90;
                }
            }
            
            /*if (Input.GetKeyDown(KeyCode.D) && Wife.PlayerMovementAllowed)
            {
                targetAngle = -90;
            }
            if (Input.GetKeyDown(KeyCode.A) && Wife.PlayerMovementAllowed)
            {
                targetAngle = 90;
            }
            //Increase or decrease top speed
            if (Input.GetKey(KeyCode.S) && Wife.PlayerMovementAllowed)
            {
                //topSpeed = Mathf.Clamp(topSpeed - Time.deltaTime * topSpeedSpeed, defaultSpeed, maxTopSpeed);
                targetAngle = 180;
            }
            else if (Input.GetKey(KeyCode.W) && Wife.PlayerMovementAllowed)
            {
                // topSpeed = Mathf.Clamp(topSpeed + Time.deltaTime * topSpeedSpeed, defaultSpeed, maxTopSpeed);
                targetAngle = 0;
            }*/
        }
        targetAngle = AngleFormat(targetAngle);
        float lerpedAngle = Mathf.LerpAngle(carSprite.transform.localEulerAngles.z, targetAngle, 0.05f);
        if (state != State.GameOver && Time.deltaTime != 0)
        {
            carSprite.transform.localEulerAngles = new Vector3(0, 0, AngleFormat(lerpedAngle));
        }
        float angleDifference = targetAngle - AngleFormat(lerpedAngle);
        //Use speed
        float displacement = speed * Time.deltaTime;
        transform.position += carSprite.transform.up * displacement;
        //Trail
        if (state == State.Breaking || Mathf.Abs(angleDifference) > 1)
        {
            trail1.emitting = true;
            trail2.emitting = true;
            if (!isPlayingTurningSfx)
            {
                audioSource.loop = true;
                audioSource.Play();
                isPlayingTurningSfx = true;
            }
        }
        else
        {
            trail1.emitting = false;
            trail2.emitting = false;
            audioSource.Stop();
            isPlayingTurningSfx = false;
        }
    }

    public void GameOver()
    {
        if (gas != null)
        {
            gas.Stop();
        }
        state = State.GameOver;
    }

    public void Crash()
    {
        Wife.PlayerMovementAllowed = false;
        GameOver();
        speed = 0;
        wife.healthDecrese();
        wife.healthDecrese();
        wife.healthDecrese();
        //Destroy(gameObject);
    }

    public void Startup()
    {
        gas.Play();
        topSpeed = startupSpeed;
        speed = startupSpeed;
        GetComponentInChildren<Audio>().startEngine();
    }

    private float AngleFormat(float angle)
    {
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        if (angle > 360)
        {
            angle = angle - 360;
        }

        return angle;
    }
}
