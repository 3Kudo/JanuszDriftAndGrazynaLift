using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarLocalize : MonoBehaviour
{
    public Image arrow;
    public Wife wife;

    void Update()
    {

        //int x = Mathf.RoundToInt((position.x + 200) / 20);
        //int y = Mathf.RoundToInt(position.y / 20);

        //ebug.Log($"Tile position: ({x}, {y})");
        RotateArrowToNextTile(transform.position);
        //CheckForVictory(transform.position);
    }

    void RotateArrowToNextTile(Vector2 currentPosition)
    {
        if (MapGenerator.trail.Count < 3) return; // Ensure there are at least 3 positions to analyze

        int currentX = Mathf.RoundToInt((currentPosition.x + 200) / 20);
        int currentY = Mathf.RoundToInt(currentPosition.y / 20);

        int currentIndex = MapGenerator.trail.FindIndex(t => t.Item2 == currentX && t.Item1 == currentY);

        if (currentIndex == MapGenerator.trail.Count - 1)
        {
            wife.EndGame();
        }


        if (currentIndex == -1 || currentIndex >= MapGenerator.trail.Count - 2) return; // Ensure valid indices for next and next-next

        (int nextX, int nextY) = MapGenerator.trail[currentIndex + 1];
        (int nextNextX, int nextNextY) = MapGenerator.trail[currentIndex + 2];

        int deltaX1 = nextX - currentX;
        int deltaY1 = nextY - currentY;
        int deltaX2 = nextNextX - nextX;
        int deltaY2 = nextNextY - nextY;

        float angle = 0f;

        arrow.gameObject.SetActive(false);

        if (deltaX1 != 0) // Moving horizontally
        {
            if (deltaY2 > 0) { angle = -90f; arrow.gameObject.SetActive(true); }   // Turn East
            else if (deltaY2 < 0) angle = 90f; // Turn West
        }
        else if (deltaY1 != 0) // Moving vertically
        {
            
            if (deltaX2 > 0) angle = 0f;     // Turn North
            else if (deltaX2 < 0) angle = 180f; // Turn South
        }

        arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
