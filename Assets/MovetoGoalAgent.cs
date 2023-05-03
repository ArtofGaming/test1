using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MovetoGoalAgent : Agent
{
    [SerializeField] private Material winMat;
    [SerializeField] private Material loseMat;
    [SerializeField] private MeshRenderer floor;
    [SerializeField] private Transform targetTransform;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, .49f, -1.68f);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float moveSpeed = 3f;
        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Hoizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(2f);
            floor.material = winMat;
            EndEpisode();
        }
        else if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-3f);
            floor.material = loseMat;
            EndEpisode();
        }
    }
}
