using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class Ajan2 : Agent
{
    private Rigidbody rbody;

    public Transform BuyukHedef;
    public Transform KucukHedef;

    public float carpan=20f;

    private void Start()
    {
        rbody = GetComponent<Rigidbody>();

    }

    public override void OnEpisodeBegin()
    {
        if(transform.localPosition.y < 0)
        {
            rbody.angularVelocity = Vector3.zero;
            rbody.velocity = Vector3.zero;
            transform.localPosition = new Vector3(0f, 0f, 0f);
        }
        BuyukHedef.localPosition = new Vector3(Random.value * 13.5f - 8, 0f, Random.value * 13.5f - 8);
        KucukHedef.localPosition = new Vector3(Random.value * 13.5f - 8, 0f, Random.value * 13.5f - 8);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BuyukHedef.localPosition);
        sensor.AddObservation(KucukHedef.localPosition);
        sensor.AddObservation(transform.localPosition);

        sensor.AddObservation(rbody.velocity.x);
        sensor.AddObservation(rbody.velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Vector3 kontrolSinyali = Vector3.zero;
        kontrolSinyali.x = actions.ContinuousActions[0];
        kontrolSinyali.z = actions.ContinuousActions[1];
        rbody.AddForce(kontrolSinyali * carpan * 10);

        float buyukHedefeOlanFark = Vector3.Distance(transform.localPosition, BuyukHedef.localPosition);
        float kucukHedefeOlanFark = Vector3.Distance(transform.localPosition, KucukHedef.localPosition);

        if (buyukHedefeOlanFark < 1f)
        {
            SetReward(2f);
            EndEpisode();
        }
        if(kucukHedefeOlanFark < 1f)
        {
            SetReward(0.1f);
            EndEpisode();
        }
        if(transform.localPosition.y < -0.1f)
        {
            SetReward(-1f);
            EndEpisode();
        }

    }

}
