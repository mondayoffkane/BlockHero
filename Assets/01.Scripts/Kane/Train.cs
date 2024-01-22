using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : Vehicle
{
    public bool isHead = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Cor_Update());
    }

    IEnumerator Cor_Update()
    {
        while (true)
        {
            yield return null;
            Vector3 lookrotation = _agent.steeringTarget - transform.position;

            if (lookrotation != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookrotation), extraRotationSpeed * Time.deltaTime * 10f);
            }


            switch (_state)
            {
                case State.Wait:

                    break;

                case State.Move:



                    _currentDis = Vector3.Distance(transform.position, _target.transform.position);
                    if (_currentDis <= _minDistance)
                    {
                        _target = Managers.Game.currentStageManager.FindBuilding();
                        if (isHead == false)
                        {
                            PullBlock();
                        }
                        _agent.Warp(_blockStorage.transform.Find("Out_Pos").position);
                        SetDest(_target, State.PickDown);

                    }
                    break;

                //case State.PickUp:
                //    _currentDis = Vector3.Distance(transform.position, _target.transform.position);
                //    if (_currentDis <= _minDistance)
                //    {
                //        setde
                //    }
                //    break;

                case State.PickDown:
                    _currentDis = Vector3.Distance(transform.position, _target.transform.position);
                    if (_currentDis <= _minDistance)
                    {
                        if (isHead == false && _currentCount > 0) PushBlock2(_currentCount);
                        _boxMeshFilter.gameObject.SetActive(false);
                        _agent.SetDestination(_blockStorage.transform.Find("In_Pos").transform.position);
                        yield return new WaitForSeconds(1f);
                        _target = _blockStorage.transform.Find("In_Pos");
                        SetDest(_target, State.Return);
                        //_state = State.Return;
                    }
                    break;

                case State.Return:

                    _currentDis = Vector3.Distance(transform.position, _target.transform.position);
                    if (_currentDis <= _minDistance)
                    {
                        //Managers.Game.currentStageManager._vehicleQueue.Enqueue(this);
                        Managers.Game.currentStageManager.VehicleEnqueue(this);

                        SetDest(_blockStorage.transform.Find("Out_Pos"), State.Sleep);
                        _agent.Warp(_blockStorage.transform.Find("Out_Pos").position);

                    }
                    break;

            }
        }
    }



}
