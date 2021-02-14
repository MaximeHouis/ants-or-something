using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var antController = other.gameObject.GetComponent<AntAgent>();

        if (!antController)
            return;

        antController.m_agent.ResetPath();
    }
}