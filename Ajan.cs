using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class Ajan : Agent
{
    private Rigidbody rbody;
    public Transform Hedef;
    public float carpan = 5f; 

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (transform.localPosition.y < 0)
        {
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0f, 0.5f, 0f);

        }

        Hedef.localPosition = new Vector3(Random.value * 8.5f - 4, 0.5f, Random.value * 8.5f - 4);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Hedef.localPosition);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 kontrolSinyali = Vector3.zero;
        kontrolSinyali.x = actions.ContinuousActions[0];
        kontrolSinyali.z = actions.ContinuousActions[1];
        rbody.AddForce(kontrolSinyali * carpan);


        float hedefeOlanFark = Vector3.Distance(transform.localPosition,Hedef.localPosition);

        if (hedefeOlanFark < 1.5f)
        {
            SetReward(1.0f);
            EndEpisode();

        }

        if (transform.localPosition.y < 0f)
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");

    }

}
