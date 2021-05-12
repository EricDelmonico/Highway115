using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    /// <summary>
    /// Amount of energy as a float with 0 meaning no energy and 1 being the maximum
    /// </summary>
    public float Energy
    {
        get => slider.value;
        set => slider.value = value;
    }

    [SerializeField]
    private Slider slider;

	private Vector3 baseScale;

	private const float maxScaleIncrease = 2f;

	//private float baseWidth;
	//private float baseHeight;

    private void Start()
    {
        slider.value = slider.maxValue;

		//baseWidth = GetComponent<RectTransform>().rect.width;
		//baseHeight = GetComponent<RectTransform>().rect.height;
		baseScale = GetComponent<RectTransform>().localScale;
		transform.GetChild(0).GetComponent<RectTransform>().localScale = new Vector2(baseScale.x, baseScale.y + maxScaleIncrease);
	}
	private void Update()
	{
		float secPerBeat = Conductor.Instance.secondsPerBeat;
		float secondsSinceLastBeat = Conductor.Instance.songPosition - Conductor.Instance.lastBeatSeconds;
		float trigPercent = /*1 - */Mathf.Sqrt(Mathf.Sin(secondsSinceLastBeat / secPerBeat * Mathf.PI));

		if(!double.IsNaN(trigPercent))
			transform.GetChild(1).GetComponent<RectTransform>().localScale = new Vector2(baseScale.x/*baseScale.x + trigPercent * maxScaleIncrease*/, baseScale.y + trigPercent * maxScaleIncrease);
	}
}
