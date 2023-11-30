using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{

    //public Gradient[] _colors;

    //TrailRenderer _trail;


    public virtual  void SetInit(int _num, Vector3 _pos)
    {
        transform.position = _pos;

        GetComponent<ParticleSystem>().Clear(true);

        for (int i = 0; i < transform.childCount; i++)
        {
            //if(transform.GetChild(i).GetComponent<ParticleSystem>() != null)
            //{
            //    transform.GetChild(i).GetComponent<ParticleSystem>().Clear(true);
            //}
            if (transform.GetChild(i).GetComponent<TrailRenderer>() != null)
            {
                transform.GetChild(i).GetComponent<TrailRenderer>().Clear();
                //transform.GetChild(i).GetComponent<TrailRenderer>().po
            }
        }




        //if (_trail == null) _trail = GetComponent<TrailRenderer>();

        //_trail.colorGradient = _colors[_num];

        //for (int i = 0; i < _trail.positionCount; i++)
        //{
        //    _trail.SetPosition(i, _pos);
        //}

    }

}
