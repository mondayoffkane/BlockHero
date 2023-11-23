using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{

    public Gradient[] _colors;

    TrailRenderer _trail;


    public void SetInit(int _num, Vector3 _pos)
    {
        transform.position = _pos;

        if (_trail == null) _trail = GetComponent<TrailRenderer>();

        _trail.colorGradient = _colors[_num];

        for (int i = 0; i < _trail.positionCount; i++)
        {
            _trail.SetPosition(i, _pos);
        }

    }

}
