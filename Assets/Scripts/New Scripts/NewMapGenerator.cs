using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.VersionControl;
using UnityEngine;

public class NewMapGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject previousTile, nextTile;
    public float curvature, tilt;
    public enum roadType
    {
        straight,
        turn,
        smallIntersection,
        bigIntersection
    };

    public roadType road;

    private void Start()
    {
        
    }

    public void GenerateNewTile()
    {
        GameObject lastTile = previousTile;
        while (lastTile.GetComponent<NewMapGenerator>().previousTile != null)
        {
            lastTile = lastTile.GetComponent<NewMapGenerator>().previousTile;
        }
        lastTile.GetComponent<NewMapGenerator>().nextTile.GetComponent<NewMapGenerator>().previousTile = null;
        Destroy(lastTile);
        //todo: dolozyc skrypt do brak przejscia

        GameObject firstTile = nextTile;

        /*while (firstTile.GetComponent<NewMapGenerator>().nextTile != null)
        {
            firstTile = firstTile.GetComponent<NewMapGenerator>().nextTile;
        }*/

        Vector2 tileVector = firstTile.transform.position;
        tileVector.y += 10f;
        Quaternion quaternion = new Quaternion();
        int tileToGenerate = Random.Range(0, 2);
        



        GameObject newTile = Instantiate(tiles[tileToGenerate], tileVector, quaternion);
        newTile.GetComponent<NewMapGenerator>().previousTile = firstTile;
        firstTile.GetComponent<NewMapGenerator>().nextTile = newTile;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
