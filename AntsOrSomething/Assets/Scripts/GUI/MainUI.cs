using System;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject m_selectionPanel;
    private RectTransform m_selectionTransform;

    private bool m_mouseDrag;
    private Vector3 m_rectBegin;
    private Vector3 m_rectEnd;
    private Vector3 Delta => m_rectEnd - m_rectBegin;
    private Rect m_selectionRect;
    private Camera m_camera;

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

        m_camera = Camera.main;
        m_selectionTransform = m_selectionPanel.GetComponent<RectTransform>();

        if (!m_camera)
            throw new NullReferenceException("No main camera");
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
                Select();
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

            m_selectionRect = new Rect(bottomLeft, delta);
            m_selectionTransform.sizeDelta = delta;
            m_selectionTransform.anchoredPosition = bottomLeft;
        }
    }

    private void Select()
    {
        foreach (var ant in AntAgent.s_instances)
        {
            ant.SetSelected(m_selectionRect.Contains(m_camera.WorldToScreenPoint(ant.transform.position)));
        }
    }
}