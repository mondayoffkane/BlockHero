using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Building : MonoBehaviour
{

    //public int _maxCount = 10;
    //public int _currentCount = 0;


    //public int[] _blockArray = new int[4];

    public Block.BlockType _blockType;
    public int _maxCount = 50;
    public int _currentCount = 0;

    public bool isBuildComplete = false;
    public double _rewardPrice = 100d;
    public Transform _buildingDeco;
    public Canvas _buildingCanvas;

    public GameObject _floating_Text_Pref;
    //public Transform _floating_Group;

    public Sprite[] _blockSprites;

    VillageManager _villageManager;
    // =================


    private void Awake()
    {
        if (_villageManager == null) _villageManager = Managers._stageManager._villageManager;

        _buildingDeco = transform.Find("BuildingDeco");
        _buildingCanvas = transform.Find("Building_Canvas").GetComponent<Canvas>();


        _buildingDeco.localScale = Vector3.zero;
        _buildingCanvas.gameObject.SetActive(false);

        _buildingCanvas.transform.Find("Build_Button").GetComponent<Button>().AddButtonEvent(() => Build_Button());

        _currentCount = 0; // _maxCount;
        _buildingCanvas.transform.Find("BlockCountImg").GetChild(0).GetComponent<Text>().text = $"{_currentCount}/{_maxCount}";
        _buildingCanvas.transform.Find("BlockCountImg").GetComponent<Image>().sprite = _blockSprites[(int)_blockType];



        LoadData();

        CheckBuild();

        if (isBuildComplete)
        {

            _buildingDeco.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            _buildingCanvas.gameObject.SetActive(false);
            Transform _spark = Managers.Pool.Pop(Resources.Load<GameObject>("Building-Sparks"), transform).transform;
            _spark.localPosition = Vector3.zero;

        }


        if (_floating_Text_Pref == null) _floating_Text_Pref = Resources.Load<GameObject>("Floating_Text_Pref");



    }

    public void LoadData()
    {
        _currentCount = ES3.Load<int>($"{GetInstanceID()}_currentCount", 0);
        isBuildComplete = ES3.Load<bool>($"{GetInstanceID()}_isBuildComplete", false);
    }

    public void SaveData()
    {
        ES3.Save<int>($"{GetInstanceID()}_currentCount", _currentCount);
        ES3.Save<bool>($"{GetInstanceID()}_isBuildComplete", isBuildComplete);
    }


    public void PushBlock()
    {


        _currentCount++;
        _buildingCanvas.transform.Find("BlockCountImg").GetChild(0).GetComponent<Text>().text
            = $"{_currentCount}/{_maxCount}";
        CheckBuild();

        SaveData();




    }

    public void SetCanvas()
    {
        _buildingDeco.localScale = Vector3.zero;
        _buildingCanvas.gameObject.SetActive(true);
        _buildingCanvas.transform.Find("Build_Button").Find("Reward_Text").GetComponent<Text>().text = $"{_rewardPrice}";
    }


    public void CheckBuild()
    {
        if (isBuildComplete == false)
        {
            if (_currentCount >= _maxCount)
            {
                isBuildComplete = true;

                _buildingCanvas.transform.Find("BlockCountImg").gameObject.SetActive(false);
                _buildingCanvas.transform.Find("Build_Button").gameObject.SetActive(true);

                // build button on


                _villageManager.CompleteBuild();

                if (TutorialManager._instance._tutorial_Level == 3)
                {
                    TutorialManager._instance.Tutorial_Img();



                }

            }
        }



    }

    public void Build_Button()
    {
        if (TutorialManager._instance._tutorial_Level == 3)
        {
            TutorialManager._instance.Tutorial_Complete();
            Managers._stageManager._cams[0].transform.position = TutorialManager._instance._cams[3].transform.position;

            DOTween.Sequence().AppendInterval(2f).AppendCallback(() =>
            {
                if (TutorialManager._instance._tutorial_Level == 4)
                {

                    TutorialManager._instance.Tutorial_Img();
                }
            });

        }


        GameObject _makeingParticle = Managers.Pool.Pop(Resources.Load<GameObject>("Building-Making"), transform).gameObject;
        DOTween.Sequence()
            .AppendCallback(() =>
            {
                _makeingParticle.transform.DOScale(Vector3.one, 0.5f);
                _makeingParticle.transform.localPosition = Vector3.up * 2f;
                _buildingCanvas.gameObject.SetActive(false);
            })
            .AppendInterval(1.5f)
            .Append(_buildingDeco.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce))
            .Join(_makeingParticle.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                Managers.Pool.Push(_makeingParticle.GetComponent<Poolable>());

                Managers._stageManager.CalcMoney(_rewardPrice);

                Floating_Text(_rewardPrice);

                Transform _spark = Managers.Pool.Pop(Resources.Load<GameObject>("Building-Sparks"), transform).transform;
                _spark.localPosition = Vector3.zero;

            });



        //_buildingDeco.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        //_buildingCanvas.gameObject.SetActive(false);
        //Managers._stageManager.CalcMoney(_rewardPrice);

        //Floating_Text(_rewardPrice);



    }

    public void Floating_Text(double _num)
    {
        Transform _floatingTrans = Managers.Pool.Pop(_floating_Text_Pref, Managers._stageManager.transform.Find("4.Floating_Group")).transform;
        _floatingTrans.localScale = Vector3.one * 0.015f;
        _floatingTrans.SetAsLastSibling();
        _floatingTrans.GetChild(0).GetComponent<Text>().text = $"${_num}";
        _floatingTrans.rotation = Quaternion.Euler(new Vector3(80f, 0f, 0f));



        //_floatingTrans.localPosition = new Vector3(0f, 3f, 0f);
        _floatingTrans.position = transform.position + new Vector3(0f, 1.5f, 0);

        _floatingTrans.DOMoveZ(_floatingTrans.position.z + 2f, 1f).SetEase(Ease.Linear)
            .OnComplete(() => Managers.Pool.Push(_floatingTrans.GetComponent<Poolable>()));

    }




}
