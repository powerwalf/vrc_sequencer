using UnityEngine;
using UdonSharp;
public class SinOsc : UdonSharpBehaviour
{
	float m_frequency = 844.0f;
	int m_accumulator = 0;
	float m_sampleRate = 0; //= 48000;
	bool m_hasInitialized = false;

	private void Start()
	{
		Debug.Log("Start");
		m_sampleRate = AudioSettings.outputSampleRate;
	}

	private void OnAudioFilterRead(float[] _data, int _channels)
	{
		int dataLength = _data.Length / _channels;

		double time = AudioSettings.dspTime;

		for(int dataIndex = 0; dataIndex < dataLength; dataIndex++)
		{
			for(int channelIndex = 0; channelIndex < _channels; channelIndex++)
			{
				_data[dataIndex * _channels + channelIndex] = Mathf.Sin(Mathf.PI * 2f * m_accumulator * m_frequency / m_sampleRate);
			}
			m_accumulator++;
		}
	}

	// UdonSharp doesn't support OnAudioFilterRead yet, so we expose it manually
	private float[] onAudioFilterReadData;
	private int onAudioFilterReadChannels;
	public void _onAudioFilterRead()
	{
		OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
	}
}
