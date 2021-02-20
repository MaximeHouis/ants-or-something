using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class AntPlayer : MonoBehaviour, IAntRacer
{
    public static AntPlayer Instance;
    
    [Header("Player statistics")]
    [Min(0)] public float m_speed = 10f;

    [Min(0)] public float m_rotationSpeed = 15f;


    private Rigidbody m_rigidbody;
    private Vector3 m_targetAngle = Vector3.zero;
    private bool m_canMove;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
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

    public IEnumerator BeginRace()
    {
        m_canMove = true;

        yield return new WaitForSeconds(2);
        GameUI.Instance.CountdownText.text = "";
    }

    public IEnumerator Countdown()
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

        GameUI.Instance.CountdownText.text = "GO!";
    }

    public IEnumerator NewCheckpoint(uint index)
    {
        yield break;
    }

    public IEnumerator NewLap(int index, uint count)
    {
        GameUI.Instance.CountdownText.text = $"Lap {index}/{count}!";
        yield return new WaitForSeconds(2);
        GameUI.Instance.CountdownText.text = "";
    }
    
    public IEnumerator Finished()
    {
        yield return new WaitForSeconds(1);
        m_canMove = false;
    }
}