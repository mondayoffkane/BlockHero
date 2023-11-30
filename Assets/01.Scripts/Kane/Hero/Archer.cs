using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class Archer : Hero
{

    public override void InitStatus(HeroStatus HeroStatus, int Level)
    {
        //_heroStatus = Resources.Load<HeroStatus>($"HeroStatus/{_heroType.ToString()}");



        base.InitStatus(HeroStatus, Level);

        Fight();
    }



    public override void Fight()
    {


        StartCoroutine(Cor_Fight());


    }



    IEnumerator Cor_Fight()
    {
        yield return new WaitForSeconds(2f);

        while (_armyState != ArmyState.Dead && _armyState != ArmyState.Victory)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else
                    {
                        _armyState = ArmyState.Move;
                        _animator.SetBool("Run", true);
                    }
                    _animator.SetBool("Attack", false);
                    yield return null;
                    break;

                case ArmyState.Move:
                    if (_target != null && _target._currentHP > 0)
                    {
                        Vector3 _targetpos = _target.transform.position;
                        _targetpos.y = 0.5f;
                        transform.LookAt(_target.transform);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                            Vector3 _pos = transform.position;
                            transform.position = new Vector3(_pos.x, 0f, _pos.z);
                        }

                    }
                    else
                    {
                        _target = null;
                        _armyState = ArmyState.Wait;
                        _animator.SetBool("Attack", false);
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:

                    if (_target != null)
                    {
                        Vector3 _targetpos2 = _target.transform.position;
                        _targetpos2.y = 0.5f;
                        transform.LookAt(_target.transform);
                    }

                    _animator.SetBool("Run", false);
                    _animator.SetBool("Attack", true);

                    Attack();
                    yield return new WaitForSeconds(_attackInterval);
                    break;

                case ArmyState.Dead:


                    //isPlay = false;
                    //yield return null;
                    break;
            }



            //yield return null;
        }

        // add Victory Animation

    }


    protected override void Attack()
    {

        if (!isReadySkill)
        {
            ThrowWeapon _arrow;

            switch (_level)
            {
                case 1:
                    _arrow = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Archer-Lv1")).GetComponent<Arrow>();

                    break;

                case 2:
                    _arrow = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Archer-Lv2")).GetComponent<Arrow>();
                    break;

                default:
                    _arrow = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Archer-Lv1")).GetComponent<Arrow>();
                    break;
            }

            _arrow.SetInit(0, transform.position + Vector3.up * 0.5f);
            _arrow.transform.LookAt(_target.transform);
            DOTween.Sequence()
                .Append(_arrow.transform.DOMove(_target.transform.position, 0.5f))
                .OnComplete(() =>
                {
                    base.Attack();
                    //this.TaskDelay(0.5f, base.Attack);
                    Managers.Pool.Push(_arrow.GetComponent<Poolable>());
                });

        }
        else
        {
            Skill();
        }




    }

    public override void TestFunc()
    {
        base.TestFunc();

    }


    protected override void Skill()
    {
        //Debug.Log("Child Skill");
        base.Skill();






    }


}
