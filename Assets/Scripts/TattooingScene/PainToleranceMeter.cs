using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class PainToleranceMeter : MonoBehaviour
{
    [SerializeField, NotNull] private Slider slider = null;
    [SerializeField, NotNull] private Image background = null;
    [SerializeField, NotNull] private GameObject[] faces = null;
    [SerializeField, NotNull] private Color[] backgroundColors = null;

    private bool isPassedOut = false;
    private int currentFaceIndex = 0;

    private const float DefaultMaxValue = 100f;
    private const float defaultPainIncrement = .01f;

    private void Awake()
    {
        slider.interactable = false;
    }

    public Action OnPassedOut;

    public void UpdateMeter(float painModifier)
    {
        if(isPassedOut)
            return;
        
        slider.value += defaultPainIncrement * painModifier;

        if(slider.value >= slider.maxValue)
        {
            isPassedOut = true;
            OnPassedOut?.Invoke();
        }

        UpdateImage();
    }

    public void ResetMeter(float maxValue = DefaultMaxValue)
    {
        slider.maxValue = maxValue;
        isPassedOut = false;
    }

    private void UpdateImage()
    {
        float val = slider.value;
        float max = slider.maxValue;
        int oldIndex = currentFaceIndex;

        if(val >= max)
        {
            currentFaceIndex = 3;
        }
        else if(val > (max * .7f))
        {
            currentFaceIndex = 2;
        }
        else if(val > (max * .4f))
        {
            currentFaceIndex = 1;
        }
        else
        {
            currentFaceIndex = 0;
        }

        if(oldIndex != currentFaceIndex)
        {
            for(int i=0; i < faces.Length; i++)
                faces[i].SetActive(i == currentFaceIndex);
            
            background.color = backgroundColors[currentFaceIndex];
        }
    }
}
