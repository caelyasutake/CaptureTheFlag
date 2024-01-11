using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveBlueAgent : Agent
{

    [SerializeField] private Transform safe;
    [SerializeField] private Transform redFlag;

    [SerializeField] private Transform obstacleRed1;
    [SerializeField] private Transform obstacleRed2;

    [SerializeField] private Transform obstacleBlue1;
    [SerializeField] private Transform obstacleBlue2;

    [SerializeField] private Transform opponent1;
    [SerializeField] private Transform opponent2;
    [SerializeField] private Transform opponent3;

    [SerializeField] private Transform teammate1;
    [SerializeField] private Transform teammate2;

    [SerializeField] private Transform blueTarget;
    [SerializeField] private Transform redTarget;

    [SerializeField] private GameObject Flag;
    [SerializeField] private Transform env;

    public bool target;
    public bool hasFlag;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 8.9f, Random.Range(-13.5f, -13f));
        teammate1.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 8.9f, Random.Range(-13.5f, -13f));
        teammate2.localPosition = new Vector3(Random.Range(-4.5f, 4.5f), 8.9f, Random.Range(-13.5f, -13f));

        obstacleBlue1.localPosition = new Vector3(Random.Range(0f, 9f), 8.41f, Random.Range(8f, 9f));
        obstacleBlue2.localPosition = new Vector3(Random.Range(0f, 9f), 8.41f, Random.Range(8f, 9f));

        Flag.SetActive(true);
        target = false;
        hasFlag = false;

        env.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        transform.rotation = Quaternion.identity;

    }

    public void Respawn()
    {
        transform.localPosition = new Vector3(Random.Range(-8.5f, 2.5f), 8.9f, Random.Range(-13.5f, -13f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (hasFlag == false)
        {
            sensor.AddObservation((Vector2)redFlag.localPosition);
        }

        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)safe.localPosition);

        sensor.AddObservation((Vector2)obstacleBlue1.localPosition);
        sensor.AddObservation((Vector2)obstacleBlue2.localPosition);
        sensor.AddObservation((Vector2)obstacleRed1.localPosition);
        sensor.AddObservation((Vector2)obstacleRed2.localPosition);

        sensor.AddObservation((Vector2)opponent1.localPosition);
        sensor.AddObservation((Vector2)opponent2.localPosition);
        sensor.AddObservation((Vector2)opponent3.localPosition);

        sensor.AddObservation((Vector2)teammate1.localPosition);
        sensor.AddObservation((Vector2)teammate2.localPosition);

        sensor.AddObservation((Vector2)redTarget.localPosition);
        sensor.AddObservation((Vector2)blueTarget.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = 0;
        float moveZ = actions.ContinuousActions[2];

        float movementSpeed = 5f;

        transform.localPosition += new Vector3(moveX, moveY, moveZ) * Time.deltaTime * movementSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[2] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if agent is target
        if (other.name == "TargetAreaBlue")
        {
            target = true;
        }
        else if (other.name == "SafeZoneRed")
        {
            //AddReward(25f);
            //target = false;
        }
        else if (other.name == "TargetAreaRed")
        {
            AddReward(35f);
            target = false;
        }

        // When agent contacts opponent given conditions
        if (target == true)
        {
            if (hasFlag == false)
            {
                if (other.TryGetComponent(out RedAgent redAgent))
                {
                    // Negative reward for getting tagged by opponent
                    AddReward(-50f);

                    // Move agent back to spawn point
                    Respawn();
                }
            }
            else
            {
                if (other.TryGetComponent(out RedAgent redAgent))
                {
                    // Respawn flag
                    Flag.SetActive(true);

                    hasFlag = false;

                    // Negative reward for getting tagged by opponent
                    AddReward(-50f);

                    // Move agent back to spawn point
                    Respawn();
                }
            }
        }
        else if (target == false)
        {
            if (other.TryGetComponent(out RedAgent redAgent))
            {
                // Positive reward for tagging opponnet
                AddReward(25f);
            }
        }

        // Check if agent gets flag
        if (other.name == "RedFlag")
        {
            hasFlag = true;
            Flag.SetActive(false);

            // Positive reward for getting flag
            AddReward(100f);
        }

        // Check if team wins round
        if (other.name == "TargetAreaRed")
        {
            if (hasFlag == true)
            {
                AddReward(200f);
                EndEpisode();
            }
        }

        if (other.name == "Right")
        {
            AddReward(-50f);
            //EndEpisode();
            Respawn();
        }
        else if (other.name == "Left")
        {
            AddReward(-50f);
            //EndEpisode();
            Respawn();
        }
        else if (other.name == "Top")
        {
            AddReward(-50f);
            //EndEpisode();
            Respawn();
        }
        else if (other.name == "Bottom")
        {
            AddReward(-50f);
            //EndEpisode();
            Respawn();
        }
    }
}