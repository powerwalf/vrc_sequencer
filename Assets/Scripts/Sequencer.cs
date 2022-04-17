using UnityEngine;
using UdonSharp;
using VRC.SDKBase;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Sequencer : MonoBehaviour
{
	[SerializeField] private bool[] m_sequence = new bool[8];

	[SerializeField] private AudioSource m_audioSource;
	[SerializeField] private AudioClip m_audioClip;
	[SerializeField, Min(20)] private float m_bpm = 130.0f;

	private int[] m_playheads = new int[10];
	private float[] m_audioClipData;  // all audioclips will be summed to mono
	private double m_nextStepTime;
	private int m_stepIndex;
	private bool m_isPlaying = false;
	private int m_sampleRate;  // cache AudioSettings.outputSampleRate since it cant be called in the audio thread

	public int SequenceLength { get { return m_sequence.Length; } }

	private void Start()
	{
		m_sampleRate = AudioSettings.outputSampleRate;

		for(int i = 0; i < m_playheads.Length; i++)
		{
			m_playheads[i] = -1;
		}

		// ADD MONO SUMMING LOGIC HERE
		m_audioClipData = new float[m_audioClip.samples * m_audioClip.channels];
		m_audioClip.GetData(m_audioClipData, 0);

		Play();
	}

	public void Play()
	{
		m_stepIndex = -1;
		m_nextStepTime = AudioSettings.dspTime * AudioSettings.outputSampleRate;
		m_isPlaying = true;
	}

	public void Stop()
	{
		m_isPlaying = false;
	}

	private void OnAudioFilterRead(float[] _data, int _channels)
	{
		if(!m_isPlaying)
		{
			return;
		}

		double stepLength = (m_sampleRate * 60.0f / m_bpm) * 0.25f; // 16th notes for now
		double currentTime = AudioSettings.dspTime * m_sampleRate;
		int dataLength = _data.Length / _channels;

		for(int dataIndex = 0; dataIndex < dataLength; dataIndex++)
		{
			if(currentTime + dataIndex >= m_nextStepTime)  // its time to trigger the new step 
			{
				m_nextStepTime += stepLength;
				m_stepIndex = (m_stepIndex + 1) % SequenceLength;

				// if this step is on, initialize a playhead by setting it to the first sample index
				if(m_sequence[m_stepIndex] == true)
				{
					for(int i = 0; i < m_playheads.Length; i++)
					{
						if(m_playheads[i] == -1)
						{
							m_playheads[i] = 0;
							break;
						}
					}
				}
			}

			// play from the playheads
			for(int i = 0; i < m_playheads.Length; i++)
			{
				if(m_playheads[i] > -1) 
				{
					for(int channelIndex = 0; channelIndex < _channels; channelIndex++)
					{
						_data[dataIndex * _channels + channelIndex] += m_audioClipData[m_playheads[i]];
					}

					m_playheads[i]++;

					if(m_playheads[i] >= m_audioClipData.Length)
					{
						m_playheads[i] = -1;
					}
				}
			}
		}
	}

}

