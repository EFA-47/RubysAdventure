using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
public class UIHandler : MonoBehaviour
{
    private VisualElement m_HealthBar;
    public static UIHandler instance {get; private set;}
    public float displayTime = 4.0f;
    private VisualElement m_NonPlayerDialogue;
    private Label label;
    private float m_TimerDisplay;
    // Start is called before the first frame update

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //currentHealth = 0.5f;
        UIDocument uiDocument = GetComponent<UIDocument>();
        m_HealthBar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialoge");
        m_NonPlayerDialogue.style.display = DisplayStyle.None;
        m_TimerDisplay = -1.0f;

        //VisualElement background = m_NonPlayerDialogue.Q<VisualElement>("background");
        label = m_NonPlayerDialogue.Q<Label>("DialogueLabel");


    }

    void Update()
    {
        if(m_TimerDisplay > 0)
        {
            m_TimerDisplay-= Time.deltaTime;
            if(m_TimerDisplay<=0)
            {
                m_NonPlayerDialogue.style.display = DisplayStyle.None;
            }
        }
    }

    public void DisplayDialogue(string text)
    {
        label.text = text;
        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;
        m_TimerDisplay = displayTime;
    }

    public void SetHealthValue(float health)
    {
        m_HealthBar.style.width = Length.Percent(health * 100.0f);
    }
}
