using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Rail : MonoBehaviour
{
    public Transform _nextNode;
    //public Rail _nextRail;

    //public float _moveInterval = 1f;

    public Transform _currentBlock;

    public StageManager _stageManager;


    public bool _isReady = true;

    // ==============================
    private void Start()
    {
        _stageManager = Managers._stageManager;

        //_nextRail = _nextNode.GetComponent<Rail>();

        StartCoroutine(Cor_Update());
    }


    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;

            if (_currentBlock != null)
            {
                if (_nextNode.GetComponent<Rail>()) // rail
                {
                    if (_isReady && _nextNode.GetComponent<Rail>()._currentBlock == null)
                    {
                        _nextNode.GetComponent<Rail>().PushBlock(_currentBlock);
                        _currentBlock = null;
                        //_isReady = true;
                    }
                }
                else // factory 
                {
                    _nextNode.GetComponent<HeroFactory>().PushBlock(_currentBlock.GetComponent<Block>());
                    _currentBlock = null;
                    _isReady = true;
                }
            }

        }
    }


    public void PushBlock(Transform _Block)
    {
        _currentBlock = _Block;
        _isReady = false;

        DOTween.Sequence()
            .Append(_currentBlock.DOMove(transform.position + Vector3.up, _stageManager._railSpeed)
            .OnComplete(() => { _isReady = true; }));

    }

}
