using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public float _mouseSense = 0.05f;


    //public Vector3 _camOffset;
    public float _startY, _endY;
    public Vector3 _startPos;

    public Vector2 _limitZ = new Vector2();




    void Start()
    {
        //_camOffset = transform.position;
    }


    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            _startY = Input.mousePosition.y;
            _endY = Input.mousePosition.y;
            _startPos = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            _endY = Input.mousePosition.y;

            //var _posZ = (_startPos.z + (_startY - _endY) * _mouseSense);
            //if (_posZ >= _limitZ.y && _posZ <= _limitZ.x)
            //{
            //    transform.position = _startPos + new Vector3(0f, 0f, (_startY - _endY) * _mouseSense);

            //}

            transform.position = _startPos + new Vector3(0f, 0f, (_startY - _endY) * _mouseSense);

            if (transform.position.z > _limitZ.x)
            {
                transform.position = new Vector3(0f, transform.position.y, _limitZ.x);
            }
            else if (transform.position.z < _limitZ.y)
            {
                transform.position = new Vector3(0f, transform.position.y, _limitZ.y);
            }


        }
        else if (Input.GetMouseButtonUp(0))
        {

        }


    }
}
