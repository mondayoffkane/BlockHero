using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;


public class Hero : MonoBehaviour
{
    public enum HeroState
    {
        Wait,
        Idle,
        Move,
        Attack,
        Dead
    }
    public HeroState _heroState;

    Transform _partsGroup;

    public Renderer[] _renderers;

    public float _damage =10f;
    public float _attackInterval = 2f;
    public float _maxHP = 11f;
    public float _currentHP = 11f;


    public virtual void SetInit(Recipe_Model _recipe)
    {
        if (_partsGroup == null) _partsGroup = transform.Find("PartsGroup");
        int _count = _partsGroup.childCount;
        _renderers = new Renderer[_count];
        for (int i = 0; i < _count; i++)
        {
            _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
        }

        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].material = Instantiate(_recipe._renderers[i].material);
        }

        for (int i = 0; i < _recipe._tempBlockList.Count; i++)
        {
            switch (_recipe._tempBlockList[i])
            {
                case Block.BlockType.Red:
                    _damage += 5f;
                    break;

                case Block.BlockType.Green:
                    _attackInterval += 2f;
                    break;

                case Block.BlockType.Blue:
                    _maxHP += 10f;
                    break;
                    
                default:

                    break;
            }
        }

        _currentHP = _maxHP;




    }





}
