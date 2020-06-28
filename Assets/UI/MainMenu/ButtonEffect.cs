using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(AudioSource))]
public class ButtonEffect : MonoBehaviour
{
    public Text text;
    public int fontFocusSize = 28;
    public int fontNormalSize = 18;
    public int fontClickSize = 22;
    public AudioSource audiosource;
    public AudioClip clickSound;
    public void OnEnable()
    {
        text = GetComponentInChildren<Text>();
        text.fontSize = fontNormalSize;
        audiosource = GetComponent<AudioSource>();
        //audiosource.clip = clickSound;
    }
    public void OnMouseEnter()
    {
        text.fontSize = fontFocusSize;
    }

    public void OnMouseExit()
    {
        text.fontSize = fontNormalSize;
    }

    public void OnMouseDown()
    {
        text.fontSize = fontClickSize;
    }

    public void OnMouseUp()
    {
        text.fontSize = fontFocusSize;
    }

    public void PlayOnEnter()
    {
        audiosource.Play(0);
    }
}
