using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UiEffectManager : MonoBehaviour
{


    public GameObject[] _clearEffects;

    public GameObject[] _failEffects;



    public void ClearEffect()
    {
        foreach (GameObject _effect in _clearEffects)
        {
            PlayEffect(_effect);
        }
    }

    public void FailEffect()
    {
        foreach (GameObject _effect in _failEffects)
        {
            PlayEffect(_effect);
        }
    }



    public void PlayEffect(GameObject _obj)
    {
        DOTween.Sequence().AppendCallback(() => _obj.SetActive(true))
            .AppendInterval(3f).
            AppendCallback(() => _obj.SetActive(false));

    }


}
