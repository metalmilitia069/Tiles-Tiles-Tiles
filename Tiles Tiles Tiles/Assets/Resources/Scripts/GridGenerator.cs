using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject[] gridStartingPoint;

    public int[] rows;
    public int[] columns;

    private int _gridIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        foreach (var startingPoint in gridStartingPoint)
        {
            float nextCoordY = startingPoint.transform.position.z + (tilePrefab.transform.localScale.z / 2);
            for (int i = 0; i < rows[_gridIndex]; i++)
            {
                float nextCoordX = startingPoint.transform.position.x + (tilePrefab.transform.localScale.x / 2);

                for (int j = 0; j < columns[_gridIndex]; j++)
                {
                    Instantiate(tilePrefab, new Vector3(nextCoordX, startingPoint.transform.position.y, nextCoordY), Quaternion.identity).transform.parent = this.transform;
                    nextCoordX += tilePrefab.transform.localScale.x;
                }
                nextCoordY += tilePrefab.transform.localScale.z;
            }
            _gridIndex++;
        }
    }
    
}
