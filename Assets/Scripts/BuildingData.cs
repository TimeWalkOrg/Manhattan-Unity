using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingData : MonoBehaviour
{
    public string GUID;
    public int YearBuilt;
    public int YearReplaced;
    public float GPSLatitude;
    public float GPSLongitude;
    public string Address;
    public string Owner;
    public string Description;
    public bool randomizeYears = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (randomizeYears) {
            RandomizeYears();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeYears() {
        YearBuilt = Random.Range(1630, 1661);
        YearReplaced= Random.Range(1691, 1750);
    }
}
