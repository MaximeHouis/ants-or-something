using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColonyConfiguration : MonoBehaviour
{
    [Header("Components")]
    public GameObject m_inputsRoot;

    public GameObject m_antCountModel;
    public Text m_antCountText;
    
    [Header("Ant Class Inputs")]
    public Vector2 m_offset = new Vector2(0, 0);

    [Header("Component Toggles")] [Tooltip("Components to enable once 'OK' is pressed")]
    public List<MonoBehaviour> m_components;

    private readonly Dictionary<string, GameObject> m_inputs = new Dictionary<string, GameObject>();

    private void Start()
    {
        if (!m_inputsRoot || !m_antCountModel || !m_antCountText)
        {
            Debug.LogError("Invalid component configuration");
            return;
        }

        var slider = m_antCountModel.GetComponentInChildren<Slider>();
        var classNameText = m_antCountModel.GetComponentInChildren<Text>();
        var rectTransform = m_antCountModel.GetComponentInChildren<RectTransform>();

        if (!slider || !classNameText || !rectTransform)
        {
            Debug.LogError("Missing required components in children: Slider and/or Text");
            return;
        }

        var offset = Vector2.zero;
        foreach (var antClass in Enum.GetValues(typeof(AntClass)).Cast<AntClass>())
        {
            var obj = Instantiate(m_antCountModel, m_inputsRoot.transform);
            obj.name = "Input Ant Class: " + antClass;

            slider = obj.GetComponentInChildren<Slider>();
            slider.value = Random.value;
            slider.onValueChanged.AddListener(OnValueChanged);
            SetBackground(slider, antClass);

            classNameText = obj.GetComponentInChildren<Text>();
            classNameText.text = antClass.ToString().ToLower();

            rectTransform = obj.GetComponentInChildren<RectTransform>();
            rectTransform.offsetMin += offset;
            rectTransform.offsetMax += offset;

            m_inputs.Add(antClass.ToString(), obj);

            offset += m_offset;
        }

        Destroy(m_antCountModel);
        OnValueChanged();
    }

    public void ButtonOK()
    {
        foreach (var component in m_components)
        {
            component.enabled = true;
        }
        
        gameObject.SetActive(false);
    }

    private void OnValueChanged(float _ = 0)
    {
        foreach (var input in m_inputs)
        {
            var obj = input.Value;
            var text = obj.GetComponentInChildren<Text>();
            var slider = obj.GetComponentInChildren<Slider>();

            text.text = $"{Mathf.RoundToInt(slider.value * 100.0f),3:d}% - {input.Key}";
        }
    }

    private static void SetBackground(Slider slider, AntClass antClass)
    {
        var bg = slider.fillRect.GetComponent<Image>();

        bg.color = antClass.Color();
    }
}