using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Unity.MLAgents;

public class ParkingArea : MonoBehaviour
{
    //get a list of all the parked cars
    public List<GameObject> parkedCars;
    //get parking spot gameObject
    public GameObject parkingSpot;



    //Spawn the cars and parking spots
    public List<GameObject> parkedCarSpawnAreas;

    public List<GameObject> HumanSpawnAreas;

    public GameObject humanPrefab;

    /*private void Start()
    {

    }*/
    
    public void randomSpawnCarsAndParking()
    {
        foreach(GameObject g in parkedCarSpawnAreas)
        {
            float i = Random.Range(0f, 1f);
            if (i <= 0.85f)
            {
                placeObject(parkedCars[Mathf.FloorToInt(Random.Range(0,parkedCars.Count))],g);
            }
            else
            {
                placeObject(parkingSpot, g);
            }
        }
    }
    public void spawnHumans()
    {
        foreach(GameObject g in HumanSpawnAreas)
        {
            placeObject(humanPrefab, g);
        }
    }


    public void placeObject(GameObject parkedCarObject, GameObject gameObject)
    {
        Transform spawnLocation = gameObject.transform;
        Instantiate(parkedCarObject, spawnLocation);
    }

    public void clearParking()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>(true);
        foreach (Transform child in allChildren)
        {
            //Debug.Log(child.tag);
            if (child.CompareTag("obstacle") || child.CompareTag("parking") || child.CompareTag("human") || child.CompareTag("waypoint"))//child.CompareTag("parking")
            {
                Destroy(child.gameObject);
            }
        }
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            resetArea();
//            randomSpawnCarsAndParking();
        }
    }
    
    public void resetArea()
    {
        clearParking();
        randomSpawnCarsAndParking();
        spawnHumans();
    }

}
