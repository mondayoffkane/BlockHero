using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MagicBall : ThrowWeapon
{
    public override void SetInit(int _num, Vector3 _pos)
    {
        transform.position = _pos;
        GetComponent<ParticleSystem>().Clear(true);

        //base.SetInit(_num, _pos);

    }


}
