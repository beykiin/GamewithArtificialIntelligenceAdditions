using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class Agent4 : Agent
{
    public GameObject ground;
    public GameObject area;

    public Bounds areaLimit;

    private BlockSkills _blockSkills;

    public GameObject goal;

    public GameObject block;

    public GoalScore _goalScore;

    private Rigidbody blockRbody;
    private Rigidbody agentRbody;
    private Material groundMaterial;
    private Renderer groundRenderer;

    private EnvironmentParameters resetParam;

    protected override void Awake()
    {
        base.Awake();
        _blockSkills = FindObjectOfType<BlockSkills>();
    }

    public override void Initialize()
    {
        _goalScore = block.GetComponent<GoalScore>();
        _goalScore.agent = this;


        agentRbody = GetComponent<Rigidbody>();
        blockRbody = block.GetComponent<Rigidbody>();

        groundRenderer = ground.GetComponent<Renderer>();

        areaLimit = ground.GetComponent<Collider>().bounds;

        groundMaterial = groundRenderer.material;


    }

    public Vector3 RandomSpawnPosition()
    {
        var foundNewSpawnPosition = false;
        var randomSpawnPosition = Vector3.zero;
        while (foundNewSpawnPosition == false)
        {
            var randomPosX = Random.Range(-areaLimit.extents.x * _blockSkills.spawnAreaMultiplier, areaLimit.extents.x * _blockSkills.spawnAreaMultiplier);
            var randomPosZ = Random.Range(-areaLimit.extents.z * _blockSkills.spawnAreaMultiplier, areaLimit.extents.z * _blockSkills.spawnAreaMultiplier);
            randomSpawnPosition = ground.transform.position + new Vector3(randomPosX, 1f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPosition, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnPosition = true;
            }
        }
        return randomSpawnPosition;
    }

    public void GoalScore()
    {
        AddReward(10f);
        EndEpisode();
        Start.Coroutine(GoalScoreChangeMaterial(_blockSkills.rightMaterial, 0.5f));
    }
    IEnumerator GoalScoreChangeMaterial(Material mat, float time)
    {
        groundRenderer.material = mat;
        yield return new WaitForSeconds(time);
        groundRenderer.material = groundMaterial;
    }

    public void agentMovement(ActionSegment<int> actionSegment)
    {
        var direction = Vector3.zero;
        var distance = Vector3.zero;

        var action = actionSegment[0];

        switch (action)
        {
            case 1:
                distance = transform.forward * 1f;
                break;
            case 2:
                distance = transform.forward * -1;
                break;
            case 3:
                direction = transform.up * 1f;
                break;
            case 4:
                direction = transform.up * -1d;
                break;
            case 5:
                distance = transform.right * -0.75f;
                break;
            case 6:
                distance = transform.right * 0.75f;
                break;
        }
        transform.Rotate(direction, Time.fixedDelteTime * 100f);
        agentRbody.AddForce(distance * _blockSkills.agentRunningSpeed, ForceMode.VelocityChange)
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        agentMovement(actions.DiscreteActions);
        AddReward(-1 / MaxStep);
    }

    void BlockReset()
    {
        block.transform.position = Vetor3.zero;
        blockRbody.velocity = Vector3.zero;
        blockRbody.angularVelocity = Vector3.zero;
    }

    public override void OnEpisodeBegin()
    {
        var rotatiton = Random.Range(0, 4);
        var rotationAngle = rotation * 90f;

        area.transform.Rotate(new Vector3(0f, rotationAngle, 0f));
        BlockReset();
        transform.position = RandomSpawnPosition();
        agentRbody.velocity = Vector3.zero;
        agentRbody.angularVelocity = Vector3.zero;
        ParameterReset();
    }

    public void GroundFrictionMaterial()
    {
        var groundCollider = ground.GetComponent<Collider>();
        groundCollider.material.dynamicFriction = resetParam.GetWithDefault("dynamic_friction", 0);
        groundCollider.material.staticFriction = resetParam.GetWithDefault("static_friction", 0);
    }

    public void BlockSettings()
    {
        var scale = resetParam.GetWithDefault("block_scale", 2);
        blockRbody.transform.localScale = new Vector3(scale, 0.75f, scale);
        blockRbody.drag = resetParam.GetWithDefault("block_drag", 0.5f);
    }

    void ParameterReset()
    {
        GroundFrictionMaterial();
        BlockSettings();
    }

}
