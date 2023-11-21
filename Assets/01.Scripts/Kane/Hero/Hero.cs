using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;



public class Hero : MonoBehaviour
{
    [FoldoutGroup("Army")] public float _maxHP;
    [FoldoutGroup("Army")] public float _currentHP;
    [FoldoutGroup("Army")] public float _damage;
    //[FoldoutGroup("Army")] public float _targetRange;
    [FoldoutGroup("Army")] public float _attackRange;
    [FoldoutGroup("Army")] public float _attackInterval;
    [FoldoutGroup("Army")] public float _speed;
    [FoldoutGroup("Army")] public Enemy _target;
    [FoldoutGroup("Army")] public bool isPlay = true;
    //bool isFirst = true;




    public enum ArmyState
    {
        Wait,
        Move,
        Attack,
        Dead,
        Victory
    }
    public ArmyState _armyState;

    bool isFirst = true;
    Rigidbody _rig;
    BoxCollider[] _colls;

    [SerializeField] PuzzleManager _puzzleManager;


    // ===============================

    private void OnEnable()
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
    }

    public void Fight()
    {
        StartCoroutine(Cor_Fight());
    }


    IEnumerator Cor_Fight()
    {
        yield return null;

        _rig.isKinematic = false;
        isPlay = true;
        _colls[1].enabled = true;

        _colls[1].size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        _colls[1].center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

        while (isPlay)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else _armyState = ArmyState.Move;

                    yield return null;
                    break;

                case ArmyState.Move:
                    if (_target != null)
                    {

                        transform.LookAt(_target.transform);
                        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }

                    }
                    else
                    {
                        _armyState = ArmyState.Wait;
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:
                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case ArmyState.Dead:

                    isPlay = false;
                    yield return null;
                    break;
            }



            yield return null;
        }

        // add Victory Animation

    }


    protected virtual void Attack()
    {
        if (_target == null || _target._currentHP <= 0)
        {
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _target.OnDamage(_damage);

        }


    }

    protected virtual void OnDamage(float _enemyDamage)
    {
        _currentHP -= _enemyDamage;
        if (_currentHP <= 0) _armyState = ArmyState.Dead;
    }



    protected virtual void FindTarget()
    {
        if (_puzzleManager._enemyList.Count < 1)
        {
            isPlay = false;
            _armyState = ArmyState.Victory;

        }
        else
        {
            _target = _puzzleManager._enemyList[0];



            if (_puzzleManager._enemyList.Count > 2)
            {

                for (int i = 1; i < _puzzleManager._enemyList.Count; i++)
                {
                    _target = Vector3.Distance(transform.position, _puzzleManager._enemyList[i].transform.position)
                    < Vector3.Distance(transform.position, _target.transform.position)
                    ? _puzzleManager._enemyList[i] : _target;
                }
            }
        }

        if (_target == null)
        {
            _armyState = ArmyState.Victory;
            Debug.Log("Test2");

        }
        else if (_target._currentHP <= 0)
        {
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _armyState = ArmyState.Move;
        }



    }








}
