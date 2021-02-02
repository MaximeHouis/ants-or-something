using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var antController = other.gameObject.GetComponent<AntController>();

        if (!antController)
        {
            Debug.Log("GameObject " + gameObject.name + " ignored");
            return;
        }

        Destroy(other.gameObject);
    }
}