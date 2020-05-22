using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class alphaHitThreshold : MonoBehaviour
{
    public void OnEnable()
	{
		Image image = GetComponent<Image>();
		image.alphaHitTestMinimumThreshold = 0.1f;
	}
}
