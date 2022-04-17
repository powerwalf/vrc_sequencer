/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using VRC.SDKBase;
using UdonSharp;

public class StepGUI : UdonSharpBehaviour
{
	[SerializeField] private Toggle[] m_toggles;
	[SerializeField] Sequencer m_sequencer;

	private bool[] m_toggleStates = new bool[16];
	
    void Start()
    {
		for(int i = 0; i < m_toggles.Length; i++)
		{
			int localCopy = i;
			m_toggles[i].onValueChanged.AddListener(new UnityAction<bool>((bool _on) => m_sequencer.SetStep(localCopy, _on)));  // might be able to use normal lambda
		}
	}

	void Update()
	{
		for(int i = 0; i < m_toggles.Length; i++)
		{
			m_toggles[i].targetGraphic.color = (i == (m_sequencer.CurrentStep - 1) ? Color.red : Color.black);
			if(m_toggleStates[i] != m_toggles[i].isOn)
			{
				m_toggleStates[i] = m_toggles[i].isOn;
				m_sequencer.SetStep(i, m_toggleStates[i]);
			}
		}
	}
}
*/
