using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSpectrum : MonoBehaviour
{
	public AudioSource audioSource;
	bool isOn;
	public float result;
	//public float result2;
	//public float result3;
	//public float result4;
	//public float result5;

	public void SetOn()
	{
		isOn = true;
	}
	public void SetOff()
	{
		isOn = false;
		//result = 0;
	}

	void Update()
	{
        //if (!isOn)
        //	return;

        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        float total = 0;
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            total += spectrum[i];
        }
        total /= spectrum.Length;
        print(total);
    }
}