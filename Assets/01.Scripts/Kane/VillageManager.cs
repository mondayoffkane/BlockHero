using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageManager : MonoBehaviour
{

    public int _villageLevel = 0;
    public GameObject _vehicl_Pref;


    public List<Building> _buildingList = new List<Building>();

    public int _buildingCount = 0;
    public int _completeCount = 0;


    public bool _villageComplete = false;


    public Transform[] AreaGroups;



    // ==================================

    private void OnEnable()
    {
        int _count = transform.Find("MapAreaGroup").childCount;
        AreaGroups = new Transform[_count];

        for (int i = 0; i < _count; i++)
        {
            AreaGroups[i] = transform.Find("MapAreaGroup").GetChild(i);
        }
    }


    public void CompleteBuild()
    {
        _completeCount++;
        if (_completeCount < _buildingList.Count)
            _buildingList[_completeCount].SetCanvas();

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
