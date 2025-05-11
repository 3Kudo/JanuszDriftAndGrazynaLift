using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMapGenerator : MonoBehaviour
{
    [SerializeField] GameObject startPoint, nextTile;
    public void GenerateMap()
    {
        GameObject tile, previousTile;
        previousTile = new GameObject();
        float y = 11;
        for (int i = 0; i < 4; i++) //to mi siê trochê nie podoba dlatego bêdê musia³ to zmieiæ
        {
            Vector2 spawnPosition = new Vector2(0f, y);
            Quaternion quaternion = new Quaternion();

            tile = Instantiate(nextTile, spawnPosition, quaternion);
            if (i == 0)
            {
                tile.GetComponent<NewMapGenerator>().previousTile = startPoint;
                startPoint.GetComponent<NewMapGenerator>().nextTile = tile;
                tile.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(previousTile);
                previousTile = tile;
            }
            else
            {
                tile.GetComponent<NewMapGenerator>().previousTile = previousTile;
                previousTile.GetComponent<NewMapGenerator>().nextTile = tile;
                previousTile = tile;
            }

            if (i == 1)
            {
                tile.GetComponent<BoxCollider2D>().enabled = false;
            }
            y += 10.5f;
        }
    }
}
