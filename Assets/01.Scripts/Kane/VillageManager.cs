using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{

    public int _villageLevel = 0;


    public List<Building> _buildingList = new List<Building>();

    public int _buildingCount = 0;
    public int _completeCount = 0;


    public bool _villageComplete = false;



    public void CompleteBuild()
    {
        _completeCount++;
        if (_completeCount >= _buildingCount)
        {
            _villageComplete = true;
        }

    }


    public Transform FindBuilding()
    {

        for (int i = 0; i < _buildingList.Count; i++)
        {
            if (_buildingList[i].isBuildComplete == false)
            {
                return _buildingList[i].transform;
            }
        }

        return _buildingList[Random.Range(0, _buildingList.Count)].transform;



    }













}
