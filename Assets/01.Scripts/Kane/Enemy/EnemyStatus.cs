
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStatus", menuName = "ScriptableObj/EnemyStatus", order = int.MaxValue - 1)]
public class EnemyStatus : ScriptableObject
{

    public enum EnemyType
    {
        Mushroom


    }
    public EnemyType _enemyType;


    public float[] _maxHP = new float[3];
    public float[] _damages = new float[3];
    public float[] _attackRange = new float[3];
    public float[] _attackInterval = new float[3];
    public float[] _speeds = new float[3];

    public Mesh[] _meshes = new Mesh[3];







}
