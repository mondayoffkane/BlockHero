using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILookCam : MonoBehaviour
{

    Quaternion _quaternion;

    private void OnEnable()
    {
        _quaternion = Camera.main.transform.rotation;
    }

    private void Update()
    {
        transform.rotation = _quaternion;
    }
}
