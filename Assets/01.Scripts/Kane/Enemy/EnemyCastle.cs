using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

[Button]
public class EnemyCastle : Enemy
{
    public GameObject _onDamageEffect;


    public override void OnDamage(float _tempDamage)
    {
        var _effect = Managers.Pool.Pop(_onDamageEffect, transform);
        _effect.transform.position = transform.position + new Vector3(Random.Range(-2f, 2f), 1.5f, Random.Range(-1f, 0f));
        this.TaskDelay(1f, () => Managers.Pool.Push(_effect));


        //DOTween.Kill(this.gameObject);
        if (!DOTween.IsTweening(this.gameObject))
        {
            transform.DOScale(0.9f, 0.1f).SetEase(Ease.Linear)
                .OnComplete(() => transform.DOScale(1f, 0.1f));
        }



        base.OnDamage(_tempDamage);
    }


}
