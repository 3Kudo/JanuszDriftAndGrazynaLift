using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class NewMapGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public GameObject previousTile, nextTile;
    public float curvature;

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
        tileVector.y += 10.5f;
        Quaternion quaternion = new Quaternion();
        //todo: tutaj bedzie trzeba zmienic na zasadzie posiadania zakretow a wiec bedzie trzeba pogadac z klaudia;
        GameObject newTile = Instantiate(tiles[0], tileVector, quaternion);
        newTile.GetComponent<NewMapGenerator>().previousTile = firstTile;
        firstTile.GetComponent<NewMapGenerator>().nextTile = newTile;
    }
}
