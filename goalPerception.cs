using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goalPerception : MonoBehaviour
{
    public Agent4 agent;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("goal"))
        {
            agent.GoalScore();
        }
    }
}
