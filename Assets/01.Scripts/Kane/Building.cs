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

        _currentCount = _maxCount;
        _buildingCanvas.transform.Find("BlockCountImg").GetChild(0).GetComponent<Text>().text = $"X{_currentCount}";
        CheckBuild();

        LoadData();

    }

    public void LoadData()
    {

    }


    public void PushBlock()
    {
        //int _blockTypeNum = (int)_block._blockType;
        //if (_blockArray[_blockTypeNum] > 0)
        //{
        //    _blockArray[_blockTypeNum]--;
        //}

        _currentCount--;
        _buildingCanvas.transform.Find("BlockCountImg").GetChild(0).GetComponent<Text>().text = $"X{_currentCount}";
        CheckBuild();



        //_block.transform.SetParent(transform);

        //DOTween.Sequence()
        //    .Append(_block.transform.DOLocalJump(Vector3.zero, 10f, 1, 0.3f).SetEase(Ease.Linear))
        //    .Join(_block.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear))
        //    .OnComplete(() =>
        //    {
        //        Managers.Pool.Push(_block.transform.GetComponent<Poolable>());
        //CheckBuild();
        //});


    }

    public void SetCanvas()
    {
        _buildingDeco.localScale = Vector3.zero;
        _buildingCanvas.gameObject.SetActive(true);
    }


    public void CheckBuild()
    {
        if (isBuildComplete == false)
        {
            if (_currentCount <= 0)
            {
                isBuildComplete = true;

                _buildingCanvas.transform.Find("BlockCountImg").gameObject.SetActive(false);
                _buildingCanvas.transform.Find("Build_Button").gameObject.SetActive(true);

                // build button on



                _villageManager.CompleteBuild();
            }
        }

        //for (int i = 0; i < _blockArray.Length; i++)
        //{
        //    if (_blockArray[i] > 0)
        //    {
        //        isBuildComplete = false;
        //        return;
        //    }
        //}
        //isBuildComplete = true;
        //_villageManager.CompleteBuild();



    }

    public void Build_Button()
    {

        _buildingDeco.DOScale(Vector3.one, 1f).SetEase(Ease.OutBounce);
        _buildingCanvas.gameObject.SetActive(false);
        Managers._stageManager.CalcMoney(_rewardPrice);
    }






}
