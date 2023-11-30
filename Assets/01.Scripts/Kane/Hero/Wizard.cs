using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Wizard : Hero
{


    public override void InitStatus(HeroStatus HeroStatus, int Level)
    {
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
                        _targetpos.y = transform.position.y;
                        transform.LookAt(_target.transform);

                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                            Vector3 _pos = transform.position;
                            transform.position = new Vector3(_pos.x, -0.5f, _pos.z);
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

            switch (_level)
            {
                case 1:
                    ThrowWeapon _magicBall = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Wizard-Lv1")).GetComponent<MagicBall>();

                    _magicBall.SetInit(3, transform.position + Vector3.up * 0.5f);

                    DOTween.Sequence()
                        .Append(_magicBall.transform.DOJump(_target.transform.position, 2, 1, 1f))
                        .OnComplete(() =>
                        {
                            base.Attack();
                            Managers.Pool.Push(_magicBall.GetComponent<Poolable>());
                        });
                    break;

                case 2:

                    ThrowWeapon _magicBall2 = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Wizard-Lv2")).GetComponent<MagicBall>();

                    _magicBall2.SetInit(3, _target.transform.position);

                    DOTween.Sequence()
                        .AppendInterval(1f)
                        .AppendCallback(() => base.Attack())
                        .AppendInterval(2f)
                        //.Append(_magicBall2.transform.DOJump(_target.transform.position, 2, 1, 1f))
                        .OnComplete(() =>
                        {
                            //base.Attack();
                            Managers.Pool.Push(_magicBall2.GetComponent<Poolable>());
                        });


                    break;
            }


        }
        else
        {
            Skill();
        }



        //base.Attack();


    }

    public override void TestFunc()
    {
        base.TestFunc();

        Debug.Log("Hero Child");
    }

}
