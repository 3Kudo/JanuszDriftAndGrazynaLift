using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeTimer;
    [SerializeField] private Vector3 ogPosition;
    [SerializeField] private bool shaking;

    // Start is called before the first frame update
    void Start()
    {
        //MooseScript.mooseHit += Shake;
        //ObstacleScript.obstacleHit += Shake;
        CarChildScript.treeHit += Shake;
        ogPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shaking)
        {
            shakeTimer += Time.deltaTime;
            Vector3 shakeVector = new Vector3(UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude)*Time.deltaTime, UnityEngine.Random.Range(-shakeMagnitude, shakeMagnitude) * Time.deltaTime, 0);
            transform.localPosition = ogPosition + shakeVector;
            if (shakeTimer >= shakeDuration)
            {
                shaking = false;
                shakeTimer = 0;
                transform.localPosition = ogPosition;
            }
        }
    }

    public void Shake()
    {
        shaking = true;
    }
}
