using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColonyConfiguration : MonoBehaviour
{
    [Header("Components")]
    public GameObject m_inputsRoot;

    public GameObject m_antCountModel;
    public Vector2 m_offset = new Vector2(0, 0);

    private Dictionary<string, GameObject> m_inputs = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (!m_inputsRoot || !m_antCountModel)
        {
            Debug.LogError("Invalid component configuration");
            return;
        }

        var input = m_antCountModel.GetComponentInChildren<Slider>();
        var classNameText = m_antCountModel.GetComponentInChildren<Text>();
        var rectTransform = m_antCountModel.GetComponentInChildren<RectTransform>();

        if (!input || !classNameText || !rectTransform)
        {
            Debug.LogError("Missing required components in children: Slider and/or Text");
            return;
        }

        var offset = Vector2.zero;
        foreach (var antClass in Enum.GetValues(typeof(AntController.AntClass)))
        {
            var obj = Instantiate(m_antCountModel, m_inputsRoot.transform);
            obj.name = "Input Ant Class: " + antClass;

            classNameText = obj.GetComponentInChildren<Text>();
            classNameText.text = antClass.ToString().ToLower();

            rectTransform = obj.GetComponentInChildren<RectTransform>();
            rectTransform.offsetMin += offset;
            rectTransform.offsetMax += offset;

            m_inputs.Add(antClass.ToString(), obj);

            offset += m_offset;
        }

        Destroy(m_antCountModel);
    }
}