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

		audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

		//AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
		float a = 0;
		int frag = (int)(spectrum.Length / 4);
		float result1 = spectrum [(frag*0)]+ spectrum [(frag*0)+1]+ spectrum [(frag*0)+2];
  //      float result2 = result1 * (Random.Range (0, 50) - 100) / 60;
  //      float result3 = result1 * (Random.Range (0, 50) - 100) / 60;
  //      float result4 = result1 * (Random.Range (0, 50) - 100) / 60;
  //      float result5 = result1 * (Random.Range (0, 50) - 100) / 60;
        float result2 = spectrum [(frag*1)]+ spectrum [(frag*1)+1]+ spectrum [(frag*1)+2];
        float result3 = spectrum [(frag*2)]+ spectrum [(frag*2)+1]+ spectrum [(frag*2)+2];
        float result4 = spectrum [(frag*3)]+ spectrum [(frag*3)+1]+ spectrum [(frag*3)+2];
        float result5 = spectrum [(frag*4)-3]+ spectrum [(frag*4)-2]+ spectrum [(frag*4)-1];
		a /= spectrum.Length;
		//print(spectrum.Length);
		result = (int)Mathf.Lerp (1, 100, (a / spectrum.Length) * 1500);

	}
}