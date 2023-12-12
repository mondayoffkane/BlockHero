using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Rail : MonoBehaviour
{
    public Transform _nextNode;

    //public float _moveInterval = 1f;

    public Transform _currentBlock;

    public StageManager _stageManager;

    private void Start()
    {
        _stageManager = Managers._stageManager;
    }


    public void MoveNextNode(Transform _Block)
    {
        _currentBlock = _Block;

        if (_nextNode.GetComponent<Rail>())
        {
            _Block.DOMove(_nextNode.transform.position, _stageManager._railSpeed).SetEase(Ease.Linear)
                .OnComplete(() => _nextNode.GetComponent<Rail>().MoveNextNode(_Block));

        }
        else
        {
            _Block.DOMove(_nextNode.transform.position, _stageManager._railSpeed).SetEase(Ease.Linear)
                .OnComplete(() => _stageManager._heroFactory.PushBlock(_Block.GetComponent<Block>()));
        }

    }

}
