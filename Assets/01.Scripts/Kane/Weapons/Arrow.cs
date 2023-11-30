using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : ThrowWeapon
{


    //private void OnEnable()
    //{

    //    for (int i = 0; i < transform.childCount; i++)
    //    {
    //        //if(transform.GetChild(i).GetComponent<ParticleSystem>() != null)
    //        //{
    //        //    transform.GetChild(i).GetComponent<ParticleSystem>().Clear(true);
    //        //}
    //        if (transform.GetChild(i).GetComponent<TrailRenderer>() != null)
    //        {
    //            transform.GetChild(i).GetComponent<TrailRenderer>().Clear();
    //            //transform.GetChild(i).GetComponent<TrailRenderer>().po
    //        }
    //    }
    //}


    public override void SetInit(int _num, Vector3 _pos)
    {
        base.SetInit(_num, _pos);

    }



}
