using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStandard : MonoBehaviour
{
	public int id;
	public Text field;

	public void Init(int _id, string _text)
	{
		this.id = _id;
		field.text = _text;
	}
	public void Clicked()
	{
		Events.OnButtonClicked (this);
	}

}
