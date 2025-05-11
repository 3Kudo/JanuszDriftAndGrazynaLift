using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] tiles;
    public int X;
    public int Y;
    public float curvature;

    public static List<(int,int)> trail;

    public GameObject moosePrefab;

    void Start()
    {
        int[,] map = GenerateRoadPath(X,Y, curvature);
        for (int x = 0; x < map.GetLength(0); x++) 
        {
            for (int y = 0; y < map.GetLength(1); y++) 
            {
                SpawnTile(map[x, y], y, x);
            }
        }

        /*Debug.Log(" size " + trail.Count);
        foreach((int i, int j) in trail)
        {
            Debug.Log(i + ":" + j);
        }*/
    }

    int lastType;

    public void SpawnTile(int type, int x, int y)
    {
        if(type == 0)
        {
            return;
        }
        Vector2 tilePos = new Vector2(x*20 - 200, y*20);

        

        Quaternion quaternion = new Quaternion();

        if(type == 11)
        {
            switch(lastType)
            {
                case 2:
                    type = 12;
                    break;
                case 3:
                    type = 13;
                    break;
            }
        }

        Instantiate(tiles[type], tilePos, quaternion, transform);

        GenerateObjects(tilePos);

        lastType = type;
    }

    public void GenerateObjects(Vector2 center)
    {
        int chance = Random.Range(0, 6);

        if(chance != 0)
        {
            return;
        }

        Instantiate(moosePrefab, center + new Vector2(20,0), new Quaternion(), transform);
    }

    int[,] GenerateRoadPath(int width, int height, float curveProbability)
    {
        trail = new List<(int,int)> ();
        int[,] map = new int[height, width];
        int currentX = width / 2;
        int currentY = 0;
        int nextType = 0;
        bool curveBefore = false;
        bool spawnedStart = false;

        while (currentY < height - 1)
        {
            map[currentY, currentX] = nextType == 0 ? 1 : nextType;
            trail.Add((currentY, currentX));

            if (Random.Range(0f, 1f) < curveProbability && !curveBefore && spawnedStart)
            {
                int direction = Random.Range(0, 2) == 0 ? -1 : 1;
                int newX = currentX + direction;

                if (newX >= 0 && newX < width)
                {
                    map[currentY + 1, currentX] = direction == -1 ? 3 : 2;
                    nextType = direction == -1 ? 5 : 4;
                    trail.Add((currentY + 1, currentX));

                    if (Random.Range(0, 2) == 0)
                    {
                        map[currentY + 1, currentX] = 6;
                        if (!(currentY + 2 >= map.GetLength(0) || currentX + 1 >= map.GetLength(1) || currentY + 2 < 0 || currentX + 1 < 0))
                        {
                            map[currentY + 2, currentX] = 9;
                        } 
                        
                        map[currentY + 1, currentX - direction] = 10;

                    }

                    currentX = newX;
                    curveBefore = true;
                }
            }
            else
            {
                curveBefore = false;
                nextType = 0;
                if (!spawnedStart)
                {
                    map[currentY, currentX] = 8;
                    spawnedStart = true;
                }
            }

            currentY++;
        }

        (int, int) win = trail.Last();

        map[win.Item1,  win.Item2] = 11;

        return map;

    }

}
