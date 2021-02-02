using UnityEngine;

public class RandomScale : MonoBehaviour
{
    [Header("Scale")]
    [Min(0)] public float m_Minimum = 0;

    [Min(0)] public float m_Maximum = 1;

    private void Start()
    {
        transform.localScale = Random.Range(m_Minimum, m_Maximum) * Vector3.one;
    }
}