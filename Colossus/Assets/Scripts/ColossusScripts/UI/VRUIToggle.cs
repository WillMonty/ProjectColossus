using UnityEngine;
using UnityEngine.UI;
//Based on this guide: https://unity3d.college/2017/06/17/steamvr-laser-pointer-menus/
/// <summary>
/// Basic VRUI Toggle. Uses Toggle Component solely for Toggle Groups and proper enable handling
/// </summary>
public class VRUIToggle : MonoBehaviour
{
	[Header("Toggle Colors")]
	public Color onColor;
	public Color offColor;
	public Color pressColor;

	[Header("Hover Effects")]
	public float hoverScaling;
	public float hoverZOut;
	protected Vector3 originalScale;
	protected Vector3 endScale;
	protected Vector3 originalPosition;
	protected Vector3 endPosition;
	protected float lerpTime = 0.2f;
	protected float currlerpTime;
	protected bool hovering;

	protected BoxCollider boxCollider;
	protected RectTransform rectTransform;
	protected Image image;
	protected Toggle toggle;

	private void OnEnable()
	{
		Validate();
	}

	private void OnValidate()
	{
		Validate();
	}

	protected virtual void Validate()
	{
		image = GetComponent<Image>();
		rectTransform = GetComponent<RectTransform>();

		originalScale = rectTransform.localScale;
		endScale = new Vector3(originalScale.x + hoverScaling, originalScale.y + hoverScaling, originalScale.z + hoverScaling);
		originalPosition = rectTransform.localPosition;
		endPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - hoverZOut);

		boxCollider = GetComponent<BoxCollider>();
		if (boxCollider == null)
		{
			boxCollider = gameObject.AddComponent<BoxCollider>();
		}


		toggle = GetComponent<Toggle>();
		if(toggle == null)
		{
			toggle = gameObject.AddComponent<Toggle>();
		}

		boxCollider.size = GetComponent<RectTransform>().sizeDelta;
	}

	protected virtual void Update()
	{
		HoverEffects();
	}

	protected virtual void HoverEffects()
	{
		float lerpFrac = currlerpTime/lerpTime;
		if(hovering && lerpFrac < 1.0f)
		{
			currlerpTime += Time.deltaTime;
		}
		if(!hovering && lerpFrac > 0.0f)
			currlerpTime -= Time.deltaTime;

		rectTransform.localScale = Vector3.Lerp(originalScale, endScale, lerpFrac);
		rectTransform.localPosition = Vector3.Lerp(originalPosition, endPosition, lerpFrac);
	}


	//Attach this to the Toggle On Value Changed event handler. Use Dynamic Bool
	public virtual void ToggleChanged(bool enabled)
	{
		if(!enabled)
		{
			image.color = offColor;
		}
	}


	public virtual void ToggleClicked()
	{
		toggle.isOn = !toggle.isOn;
		image.color = pressColor;
	}

	public virtual void ToggleRelease()
	{
		if(toggle.isOn)
			image.color = onColor;
		else
			image.color = offColor;
	}

	public virtual void ToggleHover(bool isIn)
	{
		if(isIn)
		{
			hovering = true;
		}
		else
		{
			hovering = false;
			ToggleRelease();
		}
	}
}
