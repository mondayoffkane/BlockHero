using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TutorialManager : MonoBehaviour
{

    public Vector3[] _imgPoss;
    public Vector2[] _imgSizes;

    public bool isFirst = true;
    public GameObject _maskImg;
    public int _tutorial_Level = 0;


    private void Start()
    {

        isFirst = ES3.Load<bool>("isFirst", true);

        _maskImg = Managers._gameUi.MaskImg;

        if (isFirst)
        {
            Tutorial_Img();
        }


    }

    [Button]
    public void Tutorial_Img()
    {
        _maskImg.SetActive(true);
        _maskImg.transform.GetComponent<RectTransform>().anchoredPosition = _imgPoss[_tutorial_Level];
        //_maskImg.transform.localScale = _imgSizes[_tutorial_Level];
        _maskImg.transform.GetComponent<RectTransform>().sizeDelta = _imgSizes[_tutorial_Level];


    }

    [Button]
    public void Tutorial_Complete()
    {
        _maskImg.SetActive(false);
        _tutorial_Level++;
    }






}
