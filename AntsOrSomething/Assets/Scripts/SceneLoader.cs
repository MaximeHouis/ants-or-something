using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteAlways]
public class SceneLoader : MonoBehaviour
{
    private static readonly int Fade = Animator.StringToHash("Fade");

    public static SceneLoader Instance;

    public Animator m_animator;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        // Hides the transition in edit mode
        m_animator.gameObject.SetActive(Application.isPlaying);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void LoadScene(int index)
    {
        StartCoroutine(Load(index));
    }

    private IEnumerator Load(int index)
    {
        m_animator.SetTrigger(Fade);
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(index);
    }
}