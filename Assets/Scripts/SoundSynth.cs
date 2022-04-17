using UnityEngine;
using UdonSharp;
public class SoundSynth : UdonSharpBehaviour
{
	/*
	public float sampleRate = 44100;
	public float frequency = 440;
	public float amplitude = 1;
	private Vector2 phase = new Vector2(1, 0);
	*/
	float m_frequency = 844.0f;
	int m_accumulator = 0;
	float m_sampleRate = 44100;

	void OnAudioFilterRead(float[] data, int channels)
	{
		int dataLength = data.Length / channels;

		for(int dataIndex = 0; dataIndex < dataLength; dataIndex++)
		{
			for(int channelIndex = 0; channelIndex < channels; channelIndex++)
			{
				data[dataIndex * channels + channelIndex] = Mathf.Sin(Mathf.PI * 2f * m_accumulator * m_frequency / m_sampleRate);
			}
			m_accumulator++;
		}

		/*
		var delta = frequency / sampleRate * Mathf.PI * 2;
		var deltaX = Mathf.Cos(delta);
		var deltaY = Mathf.Sin(delta);
		var dotX = new Vector2(deltaX, -deltaY);
		var dotY = new Vector2(deltaY, +deltaX);

		var dataLen = data.Length;
		for(int i = 0; i < dataLen;)
		{
			data[i] = amplitude * phase.y;
			if((++i) % channels == 0)
				phase = new Vector2(Vector2.Dot(phase, dotX), Vector2.Dot(phase, dotY));
		}
		phase = phase.normalized;
		*/
	}
	// don't implement Update() here! it'll crash the Udon stack

	// UdonSharp doesn't support OnAudioFilterRead yet, so we expose it manually
	private float[] onAudioFilterReadData;
	private int onAudioFilterReadChannels;
	public void _onAudioFilterRead()
	{
		OnAudioFilterRead(onAudioFilterReadData, onAudioFilterReadChannels);
	}
}
