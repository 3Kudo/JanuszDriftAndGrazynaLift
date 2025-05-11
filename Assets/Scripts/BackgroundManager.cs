using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private GameObject road;
    [SerializeField] private Sprite roadSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (road.transform.position.y < car.transform.position.y - roadSprite.bounds.size.y / 16)
        {
            road.transform.position += new Vector3(0,roadSprite.bounds.size.y/4, 0);
        }
    }
}
