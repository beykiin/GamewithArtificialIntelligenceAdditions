using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Random = UnityEngine.Random;

public class Ajan3 : Agent
{
    public GameObject kure;
    public Rigidbody kureRbody;

    public override void Initialize()
    {
        kureRbody = kureRbody.GetComponent<Rigidbody>();


    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.rotation.z);
        sensor.AddObservation(transform.rotation.x);
        sensor.AddObservation(kure.transform.position - transform.transform.position);
        sensor.AddObservation(kureRbody.velocity);



    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionZ = Mathf.Clamp(actions.ContinuousActions[0], -1f, 1f);
        var actionX = Mathf.Clamp(actions.ContinuousActions[1], -1f, 1f);

        if((transform.rotation.z < 0.25f && actionZ > 0f) || (transform.rotation.z > -0.25f && actionZ < 0f))
        {
            transform.Rotate(new Vector3(0f, 0f, 1f), actionZ);

        }

        if ((transform.rotation.x < 0.25f && actionX > 0f) || (transform.rotation.x > -0.25f && actionX < 0f))
        {
            transform.Rotate(new Vector3(1f, 0f, 0f), actionX);

        }

        if((kure.transform.position.y - transform.position.y)<-2f || Mathf.Abs(kure.transform.position.x - transform.position.x)>3f || Mathf.Abs(kure.transform.position.z - transform.position.z) > 3f)
        {
            SetReward(-1f);
            EndEpisode();
        }
        else
        {
            SetReward(0.1f);
        }


    }

    public override void OnEpisodeBegin()
    {
        transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        transform.Rotate(new Vector3(1f, 0f, 0f), Random.Range(-10f, 10f));
        transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(-10f, 10f));
        kureRbody.velocity = new Vector3(0f, 0f, 0f);
        kure.transform.position = new Vector3(Random.Range(-1.5f, 1.5f), 4f, Random.Range(-1.5f, 1.5f)) + transform.position; 
    }


}
