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

    private void Start()
    {
        _completeCount = ES3.Load<int>("CompleteCount", 0);
        _villageComplete = ES3.Load<bool>("VillageComplete", false);

        for (int i = 0; i < _buildingList.Count; i++)
        {
            if (_buildingList[i].isBuildComplete == false)
            {
                _buildingList[i].SetCanvas();
                break;
            }
        }
    }


    public void CompleteBuild()
    {
        _completeCount++;
        ES3.Save<int>("CompleteCount", _completeCount);
        if (_completeCount < _buildingList.Count)
            _buildingList[_completeCount].SetCanvas();

        if (_completeCount >= _buildingCount)
        {
            _villageComplete = true;
            ES3.Save<bool>("VillageComplete", _villageComplete);
        }

    }


    public Transform FindBuilding()
    {

        for (int i = 0; i < _buildingList.Count; i++)
        {
            //if (_buildingList[i].isBuildComplete == false)
            if (_buildingList[i]._currentCount < _buildingList[i]._maxCount)
            {
                return _buildingList[i].transform;
            }
        }

        return _buildingList[Random.Range(0, _buildingList.Count)].transform;

    }

    public Transform ReFindBuilding()
    {
        for (int i=0; i<_buildingList.Count; i++)
        {
            if (Managers._stageManager._blockStorage._blockCountArray[(int)_buildingList[i]._blockType] > 0)
            {
                return _buildingList[i].transform;
            }
        }

            return null;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < _buildingList.Count; i++)
            {
                //if (_buildingList[i].isBuildComplete == false)
                if (_buildingList[i]._currentCount < _buildingList[i]._maxCount)
                {
                    _buildingList[i]._currentCount = _buildingList[i]._maxCount;
                    _buildingList[i].isBuildComplete = true;
                    CompleteBuild();
                    _buildingList[i].Build_Button();
                    break;
                }
            }
        }
    }












}
