using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class StageManager : MonoBehaviour
{
    public List<BlockFactory> _blockfactoryList = new List<BlockFactory>();
    public HeroFactory _heroFactory;



    // =================================================
    private void Awake()
    {
        Managers._stageManager = this;


    }



    private void Update()
    {




    }



}
