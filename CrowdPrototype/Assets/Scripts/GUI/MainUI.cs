using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject m_selectionPanel;
    private RectTransform m_selectionTransform;

    private bool m_mouseDrag;
    private Vector3 m_rectBegin;
    private Vector3 m_rectEnd;
    private Vector3 Delta => m_rectEnd - m_rectBegin;

    private bool MouseDrag
    {
        get => m_mouseDrag;
        set
        {
            m_mouseDrag = value;
            m_selectionPanel.SetActive(value);
        }
    }

    private void Start()
    {
        MouseDrag = false;

        m_selectionTransform = m_selectionPanel.GetComponent<RectTransform>();
    }

    private void Update()
    {
        var mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            if (!MouseDrag)
            {
                MouseDrag = true;
                m_rectBegin = mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (MouseDrag)
            {
                MouseDrag = false;
                m_rectEnd = mousePosition;
            }
        }

        if (MouseDrag)
        {
            var bottomLeft = new Vector3(
                Mathf.Min(m_rectBegin.x, mousePosition.x),
                Mathf.Min(m_rectBegin.y, mousePosition.y));
            var topRight = new Vector3(
                Mathf.Max(m_rectBegin.x, mousePosition.x),
                Mathf.Max(m_rectBegin.y, mousePosition.y));
            var delta = topRight - bottomLeft;

            m_selectionTransform.anchoredPosition = bottomLeft;
            m_selectionTransform.sizeDelta = delta;
        }
    }
}