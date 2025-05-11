using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarChildScript : MonoBehaviour
{
    [SerializeField] private CarScript carParent;
    [SerializeField] private ParticleSystem explosion;

    public static event Action treeHit;

    private void Start()
    {
        treeHit += carParent.Crash;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Tree"))
        {
            Vector3 middlePosition = (collision.transform.position + transform.position) / 2;
            Instantiate(explosion, middlePosition, Quaternion.Euler(Vector3.zero));
            GameObject.Find("Car").GetComponentInChildren<Audio>().playTreeCrash();
            treeHit?.Invoke();
        }
        if (collision.gameObject.CompareTag("Road"))
        {
            collision.gameObject.GetComponent<NewMapGenerator>().GenerateNewTile();
        }
    }
}
