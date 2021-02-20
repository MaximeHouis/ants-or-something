using UnityEngine;

public class FleeZone : MonoBehaviour
{
    public void OnTriggerStay(Collider other)
    {
        var agent = other.GetComponent<AntAgent>();

        if (!agent)
            return;
        if (agent.m_agent.hasPath)
            return;

        var direction = (other.transform.position - transform.position) * 2f;
        StartCoroutine(agent.SetDestination(agent.transform.position + direction));
    }
}