using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;



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
    // ===============================




    public virtual void InitStatus(HeroStatus HeroStatus, int Level)
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        if (_skinnedMesh == null) _skinnedMesh = transform.Find("Mesh").GetComponent<SkinnedMeshRenderer>();
        if (_hpGuage == null) _hpGuage = transform.Find("HP_Canvas").Find("HP_Guage").GetComponent<Image>();

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

        _skinnedMesh.sharedMesh = _heroStatus._heroMeshes[_level];

        _animator.SetBool("Idle", true);
        _animator.SetBool("Smash", false);
        _animator.SetBool("Magic", false);
        _animator.SetBool("Arrow", false);
        _animator.SetBool("Heal", false);
        _animator.SetBool("Dead", false);
        _animator.SetBool("Run", false);

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
        _armyState = ArmyState.Dead;
        _animator.SetBool("Dead", true);
        _puzzleManager._heroList.Remove(this);

        this.TaskDelay(2f, () =>
        {
            Managers._puzzleManager.DeadArnmyNEnemy(true);
            transform.gameObject.SetActive(false);
        });

        //Managers._puzzleManager.DeadArnmyNEnemy(true);
        //transform.gameObject.SetActive(false);
        //Managers.Pool.Push(transform.GetComponent<Poolable>());
        //Destroy(this);

    }


    protected virtual void FindTarget()
    {

        if (_puzzleManager._enemyList.Count < 1)
        {

            //isPlay = false;
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
