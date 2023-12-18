using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public float _mouseSense = 0.05f;


    //public Vector3 _camOffset;
    public float _startPos, _endPos;

    public Vector2 _limitZ = new Vector2();




    void Start()
    {
        //_camOffset = transform.position;
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _startPos = Input.mousePosition.y;
            _endPos = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            _endPos = Input.mousePosition.y;

            transform.position = new Vector3(0f, transform.position.y, (_endPos - _startPos) * _mouseSense);


        }
        else if (Input.GetMouseButtonUp(0))
        {

        }


    }
}
