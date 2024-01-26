using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using MondayOFF;



public class GameManager : MonoBehaviour
{

    public StageManager[] stageManagers;
    public StageManager currentStageManager;

    public int stageLevel;
    public int currentStageLevel;
    public double money = 0d;
    public int playTime = 0;

    // =====================
    //public BlockMachine _selectBlockMachine;

    [ShowInInspector] RaycastHit _tempHit;
    public GameObject[] _tempObj = new GameObject[2];
    // ===================================

    public void Init() // 처음 초기화
    {
        LoadData();

        currentStageLevel = stageLevel;
        ChangeStage();

        CalcMoney(0);



    }


    public void Clear() // 씬 전환할
    {



    }

    // ====================================
    [Button]
    public void ChangeStage(int _num = 0)
    {
        for (int i = 0; i < stageManagers.Length; i++)
        {
            if (stageManagers[i] != null)
                stageManagers[i].gameObject.SetActive(false);
        }

        currentStageLevel += _num;
        if (currentStageLevel > 1) currentStageLevel = 1;
        else if (currentStageLevel < 0) currentStageLevel = 0;

        currentStageManager = stageManagers[currentStageLevel];
        currentStageManager.gameObject.SetActive(true);
        currentStageManager.SetStage();

        GetComponent<NavMeshSurface>().BuildNavMesh();


        //CheckMoney();
        CheckScrollUpgradePrice();


        Managers._gameUi.InitRvPanel();

        money = 0;
        CalcMoney(0);

    }


    private void Start()
    {
        StartCoroutine(Cor_Update());
    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            WaitForSeconds term = new WaitForSeconds(1f);
            yield return term;

            playTime++;
            if ((playTime % 10) == 0)
            {
                ES3.Save<int>("playTime", playTime);

                EventTracker.LogCustomEvent("Village"
                    , new Dictionary<string, string> { { "Village", $"PlayTime -{playTime}" } });
            }


        }
    }

    private void Update()
    {
        //#if UNITY_EDITOR
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    CalcMoney(10000);
        //}


        //// ================= Mouse ====================
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject())// 
        //    {
        //        Managers._gameUi.ChangePanel(0);
        //    }
        //}


        //else if (Input.GetMouseButtonUp(0))
        //{
        //    if (!EventSystem.current.IsPointerOverGameObject())// 
        //    {
        //        //Debug.Log("None Ui");
        //        Ray ray;
        //        RaycastHit hit;
        //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);
        //            switch (hit.collider.tag)
        //            {
        //                case "BlockMachine":
        //                    currentStageManager._selectBlockMachine = hit.transform.GetComponent<BlockMachine>();
        //                    Managers._gameUi.ChangePanel(1);
        //                    Managers._gameUi.BlockMachine_SetColor((int)currentStageManager._selectBlockMachine._spawnBlockType);
        //                    break;

        //            }
        //        }
        //    }
        //    //else
        //    //{
        //    //    //Debug.Log("On Ui");
        //    //}
        //}

        //#elif !UNITY_EDITOR

        if (Input.touchCount > 0)
        {
            Touch _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {

                if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                {
                    Managers._gameUi.ChangePanel(0);

                    Ray ray;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Physics.Raycast(ray, out _tempHit);

                    if (_tempHit.collider == null) { }
                    else
                    {
                        _tempObj[0] = _tempHit.collider.gameObject;
                    }

                }
            }

            if (_touch.phase == TouchPhase.Ended)
            {
                //if (!EventSystem.current.IsPointerOverGameObject(_touch.fingerId))
                if (!Managers._gameUi.BlockMachine_Panel.activeSelf)
                {
                    //Debug.Log("None Ui");
                    Ray ray;
                    RaycastHit hit;
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit))
                    {
                        Debug.DrawLine(ray.origin, hit.point, Color.red, 1.5f);

                        if (hit.collider == null) { }
                        else
                        {
                            _tempObj[1] = hit.collider.gameObject;
                        }

                        //if (_tempHit.Equals(hit))
                        if (_tempObj[0].Equals(_tempObj[1]))
                        {

                            switch (hit.collider.tag)
                            {
                                case "BlockMachine":
                                    currentStageManager._selectBlockMachine = hit.transform.GetComponent<BlockMachine>();
                                    Managers._gameUi.ChangePanel(1);
                                    Managers._gameUi.BlockMachine_SetColor((int)currentStageManager._selectBlockMachine._spawnBlockType);
                                    break;

                            }
                        }
                    }
                }

            }
        }
        //#endif
    }





    public void CalcMoney(double _value)
    {
        money += _value;
        if (money < 0) money = 0;

        Managers._gameUi.Money_Text.text = $"{money:F0}";

        ES3.Save<double>("Money", money);

        CheckMoney();
        CheckScrollUpgradePrice();


    }

    void CheckMoney()
    {
        currentStageManager.CheckMoney();
    }

    void CheckScrollUpgradePrice()
    {
        //currentStageManager.CheckScrollUpgradePrice();
        currentStageManager.ChangeUI();
    }


    public void LoadData()
    {
        stageLevel = ES3.Load<int>("stageLevel", 0);
        money = ES3.Load<double>("money", 0);
        //money = ES3.Load<double>("money", 100000);
    }

    public void SaveData()
    {
        ES3.Save<int>("stageLevel", stageLevel);
        ES3.Save<double>("money", money);

    }

    //public void TryStage()
    //{

    //}

    public void ClearStage()
    {
        stageLevel++;
        ES3.Save<int>("stageLevel", stageLevel);

        EventTracker.LogCustomEvent("Village"
                       , new Dictionary<string, string> { { "Village", $"VillageClear -{stageLevel}" } });

        DOTween.Sequence()
                  .AppendCallback(() =>
                  {
                      Managers._gameUi.ChangePanel(3);

                  }).AppendInterval(2f)
                  .AppendCallback(() => Managers._gameUi.PanelOnOff(Managers._gameUi.Unlock_Panel, false))
                  .OnComplete(() => Managers._gameUi.NextStage_Button.gameObject.SetActive(true));


    }





}
