using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ColonyConfiguration : MonoBehaviour
{
    [Header("Components")]
    public GameObject m_inputsRoot;

    public GameObject m_antCountModel;
    public Text m_antCountText;
    public Button m_okButton;

    [Header("Entity Spawn")]
    public Spawner m_spawner;

    [Header("Ant Class Inputs")]
    public Vector2 m_offset = new Vector2(0, 0);

    [Header("OK button pressed")]
    public UnityEvent m_callbacks;

    private readonly Dictionary<string, GameObject> m_inputs = new Dictionary<string, GameObject>();
    private string m_originalCountFormat;

    public Dictionary<AntClass, float> Ratios { get; } = new Dictionary<AntClass, float>();

    public void ButtonOK()
    {
        m_callbacks.Invoke();
        gameObject.SetActive(false);
    }

    public void ButtonRandomizeInputs()
    {
        foreach (var slider in m_inputs.Select(input => input.Value.GetComponentInChildren<Slider>()))
        {
            slider.value = Random.value;
        }
    }

    private void Start()
    {
        if (!m_inputsRoot || !m_antCountModel || !m_antCountText)
        {
            Debug.LogError("Invalid component configuration");
            return;
        }

        m_originalCountFormat = m_antCountText.text;

        InitInputs();
        UpdateAntCount();
    }

    private void InitInputs()
    {
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

    private void UpdateAntCount()
    {
        m_antCountText.text = string.Format(m_originalCountFormat, m_spawner.m_count);
    }

    private void OnValueChanged(float _ = 0)
    {
        float weight = 0;
        float offset = 0;

        foreach (var input in m_inputs)
        {
            var obj = input.Value;
            var slider = obj.GetComponentInChildren<Slider>();

            weight += slider.value;
        }

        m_okButton.interactable = weight != 0;
        Ratios.Clear();

        foreach (var input in m_inputs)
        {
            var obj = input.Value;
            var text = obj.GetComponentInChildren<Text>();
            var slider = obj.GetComponentInChildren<Slider>();
            var count = weight != 0 ? Mathf.RoundToInt(slider.value / weight * m_spawner.m_count) : 0;

            if (weight != 0)
            {
                if (!Enum.TryParse(input.Key, out AntClass antClass))
                    throw new InvalidEnumArgumentException("Ant class error");

                var ratio = slider.value / weight;

                Ratios.Add(antClass, offset + ratio);
                offset += ratio;
            }

            var plural = count != 1 ? "s" : "";
            text.text = $"Weight: {Mathf.RoundToInt(slider.value * 100.0f),3} - " +
                        $"{input.Key,-7} {Mathf.RoundToInt(weight != 0 ? slider.value / weight * 100f : 0),3}%  " +
                        $"~{count,3} unit{plural}";
        }
    }

    private static void SetBackground(Slider slider, AntClass antClass)
    {
        var bg = slider.fillRect.GetComponent<Image>();

        bg.color = antClass.Color();
    }
}