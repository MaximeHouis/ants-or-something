using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class AntPlayer : MonoBehaviour
{
    [Header("Player statistics")]
    [Min(0)] public float m_speed = 10f;

    [Min(0)] public float m_rotationSpeed = 15f;

    [FormerlySerializedAs("m_particles")] [Header("Finish Line")]
    public GameObject m_flParticles;

    public Transform m_flTransform;

    private ParticleSystem m_particleSystem;

    private Rigidbody m_rigidbody;
    private Vector3 m_targetAngle = Vector3.zero;
    private bool m_canMove;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        if (m_flParticles)
            m_particleSystem = m_flParticles.GetComponent<ParticleSystem>();

        StartCoroutine(Countdown());
    }

    private void FixedUpdate()
    {
        MoveAnt();
        RotateAnt();
    }

    private void MoveAnt()
    {
        if (!m_canMove)
            return;
        
        // No need for fixed delta time because we want an instant move
        
        var x = Input.GetAxis("Horizontal") * m_speed;
        var z = Input.GetAxis("Vertical") * m_speed;
        var velocity = new Vector3(x, m_rigidbody.velocity.y, z);

        m_rigidbody.velocity = velocity;

        if (x == 0 && z == 0)
            return;
        
        var localAngle = transform.localEulerAngles;
        localAngle.y = Mathf.Atan2(x, z) * Mathf.Rad2Deg;
        m_targetAngle = localAngle;
    }

    private void RotateAnt()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(m_targetAngle),
            Time.fixedDeltaTime * m_rotationSpeed);
    }

    private void FireParticles(Vector3 position)
    {
        if (!m_flParticles)
            return;

        var system = Instantiate(m_flParticles, position, Quaternion.identity);

        Destroy(system, 5f);
    }

    private void BeginRace()
    {
        m_canMove = true;

        foreach (var antAgent in AntAgent.s_instances)
        {
            antAgent.BeginRace();
        }
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForFixedUpdate();
        GameUI.Instance.CountdownText.text = "Ready";

        yield return new WaitForSeconds(1);
        
        GameUI.Instance.CountdownText.text = "3";
        yield return new WaitForSeconds(1);
        
        GameUI.Instance.CountdownText.text = "2";
        yield return new WaitForSeconds(1);
        
        GameUI.Instance.CountdownText.text = "1";
        yield return new WaitForSeconds(1);

        BeginRace();
        FireParticles(m_flTransform.position);
        GameUI.Instance.CountdownText.text = "GO!";

        yield return new WaitForSeconds(2);
        
        GameUI.Instance.CountdownText.text = "";
    }

    public IEnumerator NewLap()
    {
        GameUI.Instance.CountdownText.text = "New Lap!";
        yield return new WaitForSeconds(2);
        GameUI.Instance.CountdownText.text = "";
    }
}