using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using MondayOFF;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager _instance;

    public Vector3[] _imgPoss;
    public Vector2[] _imgSizes;
    public GameObject[] _cams;


    public bool isFirst = true;
    public GameObject _maskImg;
    public int _tutorial_Level = 0;


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {

        isFirst = ES3.Load<bool>("isFirst", true);

        _maskImg = Managers._gameUi.MaskImg;

        if (isFirst)
        {
            //MondayOFF.EventTracker.TryStage(0);
            EventTracker.LogCustomEvent("Village"
                  , new Dictionary<string, string> { { "Village ", $"VillageTry -0" } });

            Tutorial_Img();
        }

        //Managers._gameUi.NextStage_Button.gameObject.SetActive(false);

    }

    [Button]
    public void Tutorial_Img()
    {
        Debug.Log("Tutorial Img :" + _tutorial_Level);

        _maskImg.SetActive(true);
        _maskImg.transform.GetComponent<RectTransform>().anchoredPosition = _imgPoss[_tutorial_Level];
        //_maskImg.transform.localScale = _imgSizes[_tutorial_Level];
        _maskImg.transform.GetComponent<RectTransform>().sizeDelta = _imgSizes[_tutorial_Level];

        _cams[_tutorial_Level].SetActive(true);

    }

    [Button]
    public void Tutorial_Complete()
    {
        Debug.Log("Tutorial Complete : +" + _tutorial_Level);
        ES3.Save<bool>("isFirst", false);
        _maskImg.SetActive(false);
        _cams[_tutorial_Level].SetActive(false);
        _tutorial_Level++;
    }






}
