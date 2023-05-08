using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDelay : MonoBehaviour
{
	public Slider slider;

	public void SetDelay(float delay)
	{
		slider.value = delay;
	}
}
