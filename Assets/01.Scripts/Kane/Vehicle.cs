using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.UI;


using Sirenix.OdinInspector;

public class Vehicle : MonoBehaviour
{
    public Vector3 _scale = new Vector3(0.9f, 1.1f, 0.9f);
    public float _scaleTime = 0.5f;

    public GameObject _blockPref;

    public enum State
    {
        Wait,
        Move,
        PickUp,
        PickDown,
        Return,
        Sleep


    }
    public State _state;
    public Block.BlockType _blockType;

    public int _speed_Level = 0;
    public int _capacity_Level = 0;

    public int _maxCount = 10;
    public int _currentCount;

    public float _minDistance = 1f;
    public float _currentDis;
    //


    public GameObject _floating_Text_Pref;
    //public Transform _floating_Group;


    NavMeshAgent _agent;
    public Transform _target;

    public Mesh[] _meshes;

    public float extraRotationSpeed = 5f;

    BlockStorage _blockStorage;
    //VillageManager _villagemanager;
    MeshFilter _boxMeshFilter;
    // =====================================


    private void Start()
    {
        //navMeshAgent.updateRotation = false;

        //if (_villagemanager == null) _villagemanager = Managers._stageManager._currentVillageManager;

        if (_blockStorage == null) _blockStorage = Managers._stageManager._blockStorage;
        StartCoroutine(Cor_Update());

        if (_agent == null) _agent = transform.GetComponent<NavMeshAgent>();
        _boxMeshFilter = transform.GetChild(0).GetComponent<MeshFilter>();


        if (_floating_Text_Pref == null) _floating_Text_Pref = Resources.Load<GameObject>("Floating_Text_Pref");

        //if (_floating_Group == null) _floating_Group = Instantiate(new GameObject("Floating_Text_Group")).transform;
        //_floating_Group.transform.SetParent(transform);
        //_floating_Group.transform.localPosition = Vector3.zero;
        //_floating_Group.rotation = Quaternion.Euler(Vector3.zero);
        //_floating_Group.SetAsLastSibling();


        DOTween.Sequence().Append(transform.DOScale(_scale, _scaleTime)).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;
            switch (_state)
            {
                case State.Wait:
                    //_target = _blockStorage.transform;
                    //_agent.SetDestination(_target.position);
                    //SetDest(_target);
                    break;

                case State.Move:
                    Vector3 lookrotation = _agent.steeringTarget - transform.position;

                    if (lookrotation != Vector3.zero)
                    {
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime);
                    }


                    _currentDis = _agent.remainingDistance;
                    if (_agent.remainingDistance <= _minDistance)
                    {
                        if (_currentCount == 0) // Arrive BlockStorage
                        {
                            //yield return new WaitForSeconds(1f);
                            //_target = _villagemanager.FindBuilding();
                            _target = Managers._stageManager._currentVillageManager.FindBuilding();
                            PullBlock();

                            if (_currentCount > 0)
                            {
                                _agent.Warp(_blockStorage.transform.Find("Out_Pos").position);
                                SetDest(_target);
                            }
                            else
                            {
                                //_target = _villagemanager.ReFindBuilding();
                                _target = Managers._stageManager._currentVillageManager.ReFindBuilding();
                                if (_target == null)
                                {
                                    //yield return new WaitForSeconds(1f);
                                    _state = State.Wait;
                                    break;
                                }
                                PullBlock();

                                if (_currentCount > 0)
                                {
                                    _agent.Warp(_blockStorage.transform.Find("Out_Pos").position);
                                    SetDest(_target);
                                }


                            }

                        }
                        else
                        {

                            while (_currentCount > 0) // Arrive Building
                            {

                                PushBlock();

                                yield return new WaitForSeconds(0.2f);

                            }
                            _boxMeshFilter.gameObject.SetActive(false);
                            yield return new WaitForSeconds(1f);

                            _target = _blockStorage.transform.Find("In_Pos");
                            SetDest(_target);

                            _state = State.Return;


                        }
                    }
                    break;

                case State.PickDown:
                    break;

                case State.Return:
                    _currentDis = _agent.remainingDistance;
                    if (_agent.remainingDistance <= _minDistance)
                    {
                        Managers._stageManager._vehicleQueue.Enqueue(this);
                        _state = State.Sleep;
                    }
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

        Building _building = _target.GetComponent<Building>();
        int _blockTypeNum = -1;

        _blockTypeNum = (int)_building._blockType;


        _blockType = (Block.BlockType)_blockTypeNum;
        _boxMeshFilter.sharedMesh = _meshes[(int)_blockType];

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

        _boxMeshFilter.gameObject.SetActive(true);


    }

    public void PushBlock()
    {
        Transform _block = Managers.Pool.Pop(_blockPref, transform).transform;
        _block.transform.localPosition = Vector3.zero;
        _block.GetComponent<MeshFilter>().sharedMesh = _boxMeshFilter.sharedMesh;

        //_target.GetComponent<Building>()._blockArray[(int)_blockType]--;
        //_target.GetComponent<Building>().PushBlock();
        _currentCount--;

        _block.DOJump(_target.transform.position, 5f, 1, 0.3f).SetEase(Ease.Linear)
            .Join(_block.DOScale(Vector3.one * 0.5f, 0.5f).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                Managers.Pool.Push(_block.GetComponent<Poolable>());

                Floating_Text(2);
                Managers._stageManager.CalcMoney(2);
                _target.GetComponent<Building>().PushBlock();
            });
        _target.GetComponent<Building>().CheckBuild();
    }




    public void Floating_Text(double _num)
    {
        Transform _floatingTrans = Managers.Pool.Pop(_floating_Text_Pref, Managers._stageManager.transform.Find("4.Floating_Group")).transform;
        _floatingTrans.localScale = Vector3.one * 0.015f;
        _floatingTrans.SetAsLastSibling();
        _floatingTrans.GetChild(0).GetComponent<Text>().text = $"${_num}";
        _floatingTrans.rotation = Quaternion.Euler(new Vector3(80f, 0f, 0f));



        //_floatingTrans.localPosition = new Vector3(0f, 1f, 0f);
        _floatingTrans.position = transform.position + Vector3.up;

        _floatingTrans.DOMoveZ(transform.position.z + 2f, 1f).SetEase(Ease.OutCirc)
            .OnComplete(() => Managers.Pool.Push(_floatingTrans.GetComponent<Poolable>()));

    }



    public void SetLevel(int _SpdLevel, int _CapLevel)
    {
        if (_agent == null) _agent = transform.GetComponent<NavMeshAgent>();

        _speed_Level = _SpdLevel;
        _capacity_Level = _CapLevel;


        _agent.speed = 2f + 0.5f * _speed_Level;
        _maxCount = 2 + 2 * _capacity_Level;


    }

    public void SetReturn()
    {
        if (_blockStorage == null) _blockStorage = Managers._stageManager._blockStorage;
        //_agent.Warp(_blockStorage.transform.Find("Out_Pos").position);
        //_agent.destination = _blockStorage.transform.Find("Out_Pos").position;

        _blockStorage._blockCountArray[(int)_blockType] += _currentCount;
        _currentCount = 0;
        //Managers._stageManager._vehicleQueue.Enqueue(this);
        //_state = State.Return;
        _state = State.Wait;
        //_target = _blockStorage.transform.Find("Out_Pos");

    }



}
