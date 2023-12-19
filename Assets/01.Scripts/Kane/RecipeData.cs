using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Recipe_Data", menuName = "Scriptable Object/Recipe Data", order = int.MaxValue - 1)]
public class RecipeData : ScriptableObject
{
    public string _recipeName;
    public Hero.HeroType _heroType;
    public Sprite _thumbnail_Sprite;
    public Sprite[] _bluePrint_Sprites;
    public Sprite[] _bluePrintFillSprites;
    public float _makingTime = 5f;


    [FoldoutGroup("Parts Img&Mesh")] public int _partsCount = 1;
    [FoldoutGroup("Parts Img&Mesh")] public Sprite[] _partsSprites = new Sprite[1];
    [FoldoutGroup("Parts Img&Mesh")] public Mesh[] _partsMeshes = new Mesh[1];

    [FoldoutGroup("Status_Base")] public float _baseDamage = 10f;
    [FoldoutGroup("Status_Base")] public float _baseAttackInterval = 1f;
    [FoldoutGroup("Status_Base")] public float _baseSpeed = 5f;
    [FoldoutGroup("Status_Base")] public float _baseHP = 10f;
    [FoldoutGroup("Status_Base")] public float _baseDefense = 5f;

    [FoldoutGroup("Status_addValue")] public float _damageValue = 5f;
    [FoldoutGroup("Status_addValue")] public float _attackIntervalValue = 0.1f;
    [FoldoutGroup("Status_addValue")] public float _speedValue = 2f;
    [FoldoutGroup("Status_addValue")] public float _HPValue = 10f;
    [FoldoutGroup("Status_addValue")] public float _defenseValue = 1f;





}
