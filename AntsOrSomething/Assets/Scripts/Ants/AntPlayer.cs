using System;
using System.Collections;
using UnityEngine;

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
        if (!m_canMove)
            return;

        MoveAnt();
        RotateAnt();
    }

    private void MoveAnt()
    {
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
        GameUI.Instance.m_textCountdown.text = "";
    }

    public IEnumerator Countdown()
    {
        yield return new WaitForFixedUpdate();

        var audioSource = GameUI.Instance.GetComponent<AudioSource>();
        var deltaPitch = 1.1f;

        if (!audioSource)
            throw new NullReferenceException();

        GameUI.Instance.m_textCountdown.text = "Ready";
        yield return new WaitForSeconds(1);

        audioSource.Play();
        GameUI.Instance.m_textCountdown.text = "3";
        yield return new WaitForSeconds(1);

        audioSource.pitch *= deltaPitch;
        audioSource.Play();
        GameUI.Instance.m_textCountdown.text = "2";
        yield return new WaitForSeconds(1);

        audioSource.pitch *= deltaPitch;
        audioSource.Play();
        GameUI.Instance.m_textCountdown.text = "1";
        yield return new WaitForSeconds(1);

        audioSource.pitch *= deltaPitch;
        audioSource.Play();
        GameUI.Instance.m_textCountdown.text = "GO!";
    }

    public IEnumerator NewCheckpoint(uint index)
    {
        yield break;
    }

    public IEnumerator NewLap(int index, uint count)
    {
        GameUI.Instance.m_textCountdown.text = $"Lap {index}/{count}!";
        yield return new WaitForSeconds(2);
        GameUI.Instance.m_textCountdown.text = "";
    }

    public IEnumerator Finished()
    {
        GameUI.Instance.m_textCountdown.text = $"Finished!\nResults incoming\nPhoto finish...";

        yield return new WaitForSeconds(0.5f);
        m_canMove = false;

        var position = CheckpointTracker.Instances.IndexOf(GetComponent<CheckpointTracker>()) + 1;
        var total = CheckpointSystem.Instance.EntityCount;
        var result = position == 1 ? "You won! Congratulations!" : "You lost, better luck next time!";

        GameUI.Instance.m_textCountdown.text = $"Finished!\nYou arrived {position} out of {total}!\n{result}";
    }
}