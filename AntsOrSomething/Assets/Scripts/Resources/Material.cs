using UnityEngine;

public abstract class Material : MonoBehaviour
{
    [Min(0)] [Tooltip("In seconds")]
    public float m_collectTime = 1f;
}