using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System;



public class Hero : MonoBehaviour
{
    public enum HeroType
    {
        Viking,
        Archer,
        Priest,
        Wizard


    }
    public HeroType _heroType;




    [FoldoutGroup("Army")] public int _level;
    [FoldoutGroup("Army")] public float _maxHP;
    [FoldoutGroup("Army")] public float _currentHP;
    [FoldoutGroup("Army")] public float _recoveryMP = 20f;
    [FoldoutGroup("Army")] public float _currentMP = 0f;
    [FoldoutGroup("Army")] public float _maxMP = 100f;
    [FoldoutGroup("Army")] public float _damage;
    //[FoldoutGroup("Army")] public float _targetRange;
    [FoldoutGroup("Army")] public float _attackRange;
    [FoldoutGroup("Army")] public float _attackInterval;
    [FoldoutGroup("Army")] public float _speed;

    [FoldoutGroup("Army")] public Enemy _target;
    [FoldoutGroup("Army")] public RuntimeAnimatorController[] _controllers;
    [FoldoutGroup("Army")] public GameObject[] _deadEffects;

    //[FoldoutGroup("Army")] public bool isPlay = true;
    //bool isFirst = true;

    [FoldoutGroup("UI")] public Image _hpGuage;

    SkinnedMeshRenderer _skinnedMesh;
    protected Animator _animator;

    public enum ArmyState
    {
        Wait,
        Move,
        Attack,
        Dead,
        Victory


    }
    public ArmyState _armyState;

    protected bool isFirst = true;
    //protected Rigidbody _rig;
    //protected BoxCollider[] _colls;
    protected BoxCollider _boxColl;

    protected PuzzleManager _puzzleManager;
    public HeroStatus _heroStatus;
    protected bool isReadySkill = false;


    // =====================================================


    public virtual void InitStatus(HeroStatus HeroStatus, int Level)
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_skinnedMesh == null) _skinnedMesh = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        if (_hpGuage == null) _hpGuage = transform.Find("HP_Canvas").Find("HP_Guage").GetComponent<Image>();
        //if(_deadEffects==null) _deadEffects[0]

        foreach (GameObject _obj in _deadEffects)
        {
            _obj.SetActive(false);
        }
        _heroStatus = HeroStatus;

        _target = null;
        _armyState = ArmyState.Wait;

        //_heroStatus = HeroStatus;
        _level = Level;

        _maxHP = _heroStatus._maxHP[Level - 1];
        _currentHP = _maxHP;
        _damage = _heroStatus._damages[Level - 1];
        _attackRange = _heroStatus._attackRange[Level - 1];
        _attackInterval = _heroStatus._attackInterval[Level - 1];
        _speed = _heroStatus._speeds[Level - 1];
        _hpGuage.fillAmount = (_currentHP / _maxHP);
        _recoveryMP = HeroStatus._recoveryMP;
        _currentMP = 0f;

        if (_boxColl == null) _boxColl = GetComponent<BoxCollider>();
        _boxColl.isTrigger = false;
        _skinnedMesh.sharedMesh = _heroStatus._heroMeshes[_level];
        _controllers = _heroStatus._controllers;
        _animator.runtimeAnimatorController = _controllers[_level - 1];



        _animator.SetBool("Dead", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Attack", false);


        _hpGuage.transform.parent.gameObject.SetActive(false);

        DOTween.Sequence()
            .Append(transform.DOScale(0f, 0f))
                        .AppendCallback(() =>
            {
                var _effect = Managers.Pool.Pop(Resources.Load<GameObject>("Effect/SpawnHero"));
                _effect.transform.position = transform.position + Vector3.up * 0.5f;
            })
            .AppendInterval(0.5f)
            .Append(transform.DOScale(2f, 0.5f).SetEase(Ease.Linear))
           .Append(transform.DOScale(1f, 0.5f).SetEase(Ease.Linear));


        //.AppendCallback(() =>
        //{
        //    transform.DOScale(1f, 1f).SetEase(Ease.OutBounce);
        //    transform.DOMoveY(1f, 1f).SetEase(Ease.Linear);
        //})
        //.AppendInterval(1f)
        //.Append(transform.DOMoveY(-0.5f, 0.5f).SetEase(Ease.Linear));



    }

    public void PushHeroList()
    {
        if (_puzzleManager == null) _puzzleManager = Managers._puzzleManager;
        _puzzleManager._heroList.Add(this);
    }


    public virtual void Fight()
    {


    }



    protected virtual void Attack()
    {

        _target.OnDamage(_damage);

        if (_target == null || _target._enemyState == Enemy.EnemyState.Dead || Vector3.Distance(transform.position, _target.transform.position) > _attackRange)
        {
            _target = null;
            _armyState = ArmyState.Wait;
        }

        _currentMP += _recoveryMP;
        if (_currentMP >= _maxMP) isReadySkill = true;


    }

    public virtual void OnDamage(float _enemyDamage)
    {
        _currentHP -= _enemyDamage;
        _hpGuage.fillAmount = (_currentHP / _maxHP);

        if (_hpGuage.fillAmount >= 1f)
        {
            _hpGuage.transform.parent.gameObject.SetActive(false);
        }
        else if (_hpGuage.transform.parent.gameObject.activeSelf == false)
        {
            _hpGuage.transform.parent.gameObject.SetActive(true);
        }

        if (_currentHP <= 0)
        {
            Dead();
        }
        else if (_currentHP >= _maxHP)
        {
            _currentHP = _maxHP;
        }
    }

    public void Dead()
    {

        _boxColl.isTrigger = true;
        _armyState = ArmyState.Dead;
        _animator.SetBool("Dead", true);
        _animator.SetBool("Attack", false);
        _animator.SetBool("Run", false);
        _puzzleManager._heroList.Remove(this);

        foreach (GameObject _effect in _deadEffects)
        {
            DOTween.Sequence().AppendCallback(() => _effect.SetActive(true))
                           .AppendInterval(3f).
           AppendCallback(() => _effect.SetActive(false));
        }

        this.TaskDelay(3.5f, () =>
        {
            Managers._puzzleManager.DeadArnmyNEnemy(true);
            transform.gameObject.SetActive(false);
        });

    }

    protected virtual void FindTarget()
    {

        if (_puzzleManager._enemyList.Count < 1)
        {
            //isPlay = false;
            _armyState = ArmyState.Victory;
            _animator.SetBool("Run", false);
            _animator.SetBool("Attack", false);

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
            _animator.SetBool("Run", false);
            _animator.SetBool("Attack", false);

        }
        else if (_target._currentHP <= 0 || _target._enemyState == Enemy.EnemyState.Dead)
        {
            Debug.Log("enemy is dead");
            _target = null;
            _armyState = ArmyState.Wait;

        }
        else
        {
            _armyState = ArmyState.Move;
            _animator.SetBool("Run", true);

            Type _enemyType = Type.GetType(_target._enemyType.ToString());
            _target = (Enemy)_target.GetComponent(_enemyType);

        }
    }


    protected virtual void Skill()
    {

        if (_target == null || _target._enemyState == Enemy.EnemyState.Dead || Vector3.Distance(transform.position, _target.transform.position) > _attackRange)
        {
            _target = null;
            _armyState = ArmyState.Wait;
        }

        _currentMP = 0f;
        isReadySkill = false;
    }


    public virtual void TestFunc()
    {
        Debug.Log("Hero Parent");

    }




}
