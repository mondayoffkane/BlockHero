using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Building : MonoBehaviour
{

    //public int _maxCount = 10;
    //public int _currentCount = 0;


    public int[] _blockArray = new int[4];

    public bool isBuildComplete = false;

    VillageManager _villageManager;
    // =================


    private void Awake()
    {
        if (_villageManager == null) _villageManager = Managers._stageManager._villageManager;
    }

    public void PushBlock(Block _block)
    {
        int _blockTypeNum = (int)_block._blockType;
        if (_blockArray[_blockTypeNum] > 0)
        {
            _blockArray[_blockTypeNum]--;
        }





        _block.transform.SetParent(transform);

        DOTween.Sequence()
            .Append(_block.transform.DOLocalJump(Vector3.zero, 10f, 1, 0.3f).SetEase(Ease.Linear))
            .Join(_block.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                Managers.Pool.Push(_block.transform.GetComponent<Poolable>());
                CheckBuild();
            });


    }


    public void CheckBuild()
    {
        for (int i = 0; i < _blockArray.Length; i++)
        {
            if (_blockArray[i] > 0)
            {
                isBuildComplete = false;
                return;
            }
        }
        isBuildComplete = true;
        _villageManager.CompleteBuild();
    }






}
