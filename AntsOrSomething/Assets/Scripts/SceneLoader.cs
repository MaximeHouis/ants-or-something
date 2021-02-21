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

#if UNITY_EDITOR
    private void Update()
    {
        m_animator.gameObject.SetActive(Application.isPlaying);
    }
#endif

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
        yield return new WaitForSeconds(.2f);
        SceneManager.LoadScene(index);
    }
}