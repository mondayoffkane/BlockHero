using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Recipe_Model : MonoBehaviour
{

    public Transform _partsGroup;
    public Renderer[] _renderers;
    public int _partsCount = 0;

    public int _currentParts_Num = 0;

    public Texture _rendTexture;

    // ======================================================

    private void Start()
    {

        _partsCount = _partsGroup.childCount;

        _renderers = new Renderer[_partsCount];
        for (int i = 0; i < _partsCount; i++)
        {
            _renderers[i] = _partsGroup.GetChild(i).GetComponent<Renderer>();
            _renderers[i].material = Instantiate(_renderers[i].material);
        }

    }



    //public void



}
