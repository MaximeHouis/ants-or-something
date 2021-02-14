using UnityEngine;

public class Edible : MonoBehaviour
{
    [Tooltip("Can be negative")]
    public int m_value;

    [Min(0)] [Tooltip("In seconds")]
    public float m_eatingTime = 1f;

    private bool m_free = true;

    public bool Eat(AntAgent antAgent)
    {
        if (!m_free)
            return false;

        m_free = false;
        // TODO: eating stuff

        return true;
    }
}