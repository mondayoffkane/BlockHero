using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;


using Sirenix.OdinInspector;

public class Vehicle : MonoBehaviour
{
    public GameObject _blockPref;

    public enum State
    {
        Wait,
        Move,
        PickUp,
        PickDown,

    }
    public State _state;
    public Block.BlockType _blockType;

    public int _maxCount = 10;
    public int _currentCount;

    public float _minDistance = 1f;
    public float _currentDis;
    //


    NavMeshAgent _agent;
    public Transform _target;

    public BlockStorage _blockStorage;
    public VillageManager _villagemanager;

    // =====================================


    private void Start()
    {
        if (_villagemanager == null) _villagemanager = Managers._stageManager._villageManager;

        if (_blockStorage == null) _blockStorage = Managers._stageManager._blockStorage;
        StartCoroutine(Cor_Update());

        if (_agent == null) _agent = transform.GetComponent<NavMeshAgent>();
    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;
            switch (_state)
            {
                case State.Wait:
                    _target = _blockStorage.transform;
                    SetDest(_target);
                    break;

                case State.Move:
                    _currentDis = _agent.remainingDistance;
                    if (_agent.remainingDistance <= _minDistance)
                    {
                        if (_currentCount == 0)
                        {
                            _target = _villagemanager.FindBuilding();

                            PullBlock();
                            if (_currentCount > 0)
                                SetDest(_target);

                            yield return new WaitForSeconds(1f);
                        }
                        else
                        {

                            while (_currentCount > 0)
                            {

                                PushBlock();

                                yield return new WaitForSeconds(0.2f);

                            }


                            _target = _blockStorage.transform;
                            SetDest(_target);

                        }
                    }
                    break;
                ////case State.PickUp:                    
                //    break;
                case State.PickDown:
                    break;

            }
        }
    }

    public void SetDest(Transform _trans)
    {
        _agent.SetDestination(_trans.position);
        _state = State.Move;
    }

    public void PullBlock()
    {
        Debug.Log("Pull Block");
        Building _building = _target.GetComponent<Building>();
        int _blockTypeNum = -1;
        for (int i = 0; i < _building._blockArray.Length; i++)
        {
            if (_building._blockArray[i] > 0)
            {
                _blockTypeNum = i;
                break;
            }
        }

        if (_blockTypeNum == -1) _blockTypeNum = Random.Range(0, 4);

        _blockType = (Block.BlockType)_blockTypeNum;

        if (_blockStorage._blockCountArray[_blockTypeNum] >= _maxCount)
        {
            _blockStorage._blockCountArray[_blockTypeNum] -= _maxCount;
            _currentCount = _maxCount;
        }
        else
        {
            int _tempCount = _blockStorage._blockCountArray[_blockTypeNum];
            _blockStorage._blockCountArray[_blockTypeNum] -= _tempCount;
            _currentCount = _tempCount;
        }
        _blockStorage.UpdateBlockCount();

    }

    public void PushBlock()
    {
        Transform _block = Managers.Pool.Pop(_blockPref, transform).transform;
        _block.transform.localPosition = Vector3.zero;

        _target.GetComponent<Building>()._blockArray[(int)_blockType]--;
        _currentCount--;

        _block.DOJump(_target.transform.position, 5f, 1, 0.3f).SetEase(Ease.Linear)
            .Join(_block.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Linear));
        _target.GetComponent<Building>().CheckBuild();
    }















}
