using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[Button]
public class EnemyCastle : Enemy
{
    public GameObject _onDamageEffect;

    public bool isEffectReady = true;

    private void OnEnable()
    {
        isEffectReady = true;
    }


    public override void InitStatus(EnemyStatus EnemyStatus, int Level)
    {
        base.InitStatus(EnemyStatus, Level);

        _maxHP = 200f + Managers._puzzleManager._stageLevel * 50f;
        _currentHP = _maxHP;
    }


    public override void OnDamage(float _tempDamage)
    {
        if (isEffectReady)
        {
            isEffectReady = false;
            var _effect = Managers.Pool.Pop(_onDamageEffect);
            _effect.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 3f, Random.Range(-2f, -1f));
            this.TaskDelay(0.5f, () => isEffectReady = true);
        }
        //this.TaskDelay(1f, () => Managers.Pool.Push(_effect));


        //DOTween.Kill(this.gameObject);
        if (!DOTween.IsTweening(this.gameObject))
        {
            transform.DOScale(0.9f, 0.1f).SetEase(Ease.Linear)
                .OnComplete(() => transform.DOScale(1f, 0.1f));
        }



        base.OnDamage(_tempDamage);
    }


}
