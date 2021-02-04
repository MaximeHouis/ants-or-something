using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var antController = other.gameObject.GetComponent<AntController>();

        if (!antController)
            return;

        antController.m_agent.ResetPath();
    }
}