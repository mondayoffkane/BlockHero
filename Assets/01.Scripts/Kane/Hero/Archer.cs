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
        yield return new WaitForSeconds(1.5f);

        //isPlay = true;
        //_rig.isKinematic = false;
        //_colls[1].enabled = true;

        _boxColl.size = GetComponent<MeshFilter>().sharedMesh.bounds.size;
        _boxColl.center = GetComponent<MeshFilter>().sharedMesh.bounds.center;

        while (_armyState != ArmyState.Dead && _armyState != ArmyState.Victory)
        {

            switch (_armyState)
            {
                case ArmyState.Wait:
                    if (_target == null) FindTarget();
                    else _armyState = ArmyState.Move;

                    yield return null;

                    break;

                case ArmyState.Move:
                    if (_target != null && _target._currentHP > 0)
                    {

                        transform.LookAt(_target.transform);


                        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
                        {
                            _armyState = ArmyState.Attack;
                        }
                        else
                        {
                            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                        }

                    }
                    else
                    {
                        _target = null;
                        _armyState = ArmyState.Wait;
                    }
                    yield return null;
                    break;

                case ArmyState.Attack:
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
            ThrowWeapon _arrow = Managers.Pool.Pop(Resources.Load<GameObject>("AttackObjects/Arrow"), transform).GetComponent<Arrow>();

            _arrow.SetInit(0, transform.position + Vector3.up * 0.5f);

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
        Debug.Log("Child Skill");
        base.Skill();






    }


}
