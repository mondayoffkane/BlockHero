using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPoolReturn : MonoBehaviour
{

    private void OnEnable()
    {


        this.TaskDelay(5f, () => Managers.Pool.Push(transform.GetComponent<Poolable>()));
    }




}
