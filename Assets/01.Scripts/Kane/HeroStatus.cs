
using UnityEngine;

[CreateAssetMenu(fileName = "HeroStatus", menuName = "ScriptableObj/HeroStatus", order = int.MaxValue - 2)]
public class HeroStatus : ScriptableObject
{

    //public enum HeroType
    //{
    //    Viking,
    //    Archer,
    //    Priest,
    //    Wizard


    //}
    public Hero.HeroType _heroType;


    public float[] _maxHP = new float[3];
    public float[] _damages = new float[3];
    public float[] _attackRange = new float[3];
    public float[] _attackInterval = new float[3];
    public float[] _speeds = new float[3];

    public Mesh[] _blockMeshes = new Mesh[3];
    public Mesh[] _heroMeshes = new Mesh[3];







}
