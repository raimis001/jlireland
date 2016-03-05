using UnityEngine;
using System.Collections;

public enum LightFunc
{
	SIN, TRI, SQR, SAW, INV, NOISE
}

public class ViralLight : MonoBehaviour
{
	// Properties
	public LightFunc waveFunction = LightFunc.SIN; // possible values: sin, tri(angle), sqr(square), saw(tooth), inv(verted sawtooth), noise (random)
	[Range(0f,1f)]
	public float baseLight = 0.0f; // start
	[Range(0f, 1f)]
	public float amplitude = 1.0f; // amplitude of the wave
	[Range(0f, 1f)]
	public float phase = 0.0f; // start point inside on wave cycle
	[Range(0f, 1f)]
	public float frequency = 0.5f; // cycle frequency per second
 
// Keep a copy of the original color
private Color originalColor;
 
// Store the original color
void Start()
	{
		originalColor = GetComponent<Light>().color;
	}

	void Update()
	{
		Light light = GetComponent<Light>();
		light.color = originalColor * (EvalWave());
	}

	float EvalWave()
	{
		float x  = (Time.time + phase) * frequency;
		float y;

		x = x - Mathf.Floor(x); // normalized value (0..1)

		if (waveFunction == LightFunc.SIN)
		{
			y = Mathf.Sin(x * 2 * Mathf.PI);
		}
		else if (waveFunction == LightFunc.TRI)
		{
			if (x < 0.5)
				y = 4.0f * x - 1.0f;
			else
				y = -4.0f * x + 3.0f;
		}
		else if (waveFunction == LightFunc.SQR)
		{
			if (x < 0.5)
				y = 1.0f;
			else
				y = -1.0f;
		}
		else if (waveFunction == LightFunc.SAW)
		{
			y = x;
		}
		else if (waveFunction == LightFunc.INV)
		{
			y = 1.0f - x;
		}
		else if (waveFunction == LightFunc.NOISE)
		{
			y = 1 - (Random.value * frequency);
		}
		else {
			y = 1.0f;
		}
		return (y * amplitude) + baseLight;
	}
}