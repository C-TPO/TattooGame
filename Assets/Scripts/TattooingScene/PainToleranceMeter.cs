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

    private const float DefaultMaxValue = 100f;

    private bool isPassedOut = false;
    private int currentFaceIndex = 0;

    public Action OnPassedOut;

    public float CurrentPain => slider.value;
    public float NormalizedPain => slider.normalizedValue;
    public bool IsPassedOut => isPassedOut;

    #region Unity Messages

    private void Awake()
    {
        slider.interactable = false;
    }

    #endregion

    #region Public API

    public void AddPain(float amount)
    {
        if (isPassedOut)
        {
            return;
        }

        slider.value = Mathf.Min(
            slider.maxValue,
            slider.value + amount
        );

        UpdateImage();

        if (slider.value >= slider.maxValue)
        {
            isPassedOut = true;
            OnPassedOut?.Invoke();
        }
    }

    public void Recover(float amount)
    {
        if (isPassedOut)
        {
            return;
        }

        slider.value = Mathf.Max(
            slider.minValue,
            slider.value - amount
        );

        UpdateImage();
    }

    public void ResetMeter(float maxValue = DefaultMaxValue)
    {
        slider.minValue = 0f;
        slider.maxValue = maxValue;
        slider.value = 0f;

        isPassedOut = false;
        currentFaceIndex = -1;

        UpdateImage();
    }

    #endregion

    #region Implementation

    private void UpdateImage()
    {
        float value = slider.value;
        float maxValue = slider.maxValue;
        int oldIndex = currentFaceIndex;

        if (value >= maxValue)
        {
            currentFaceIndex = 3;
        }
        else if (value > maxValue * 0.7f)
        {
            currentFaceIndex = 2;
        }
        else if (value > maxValue * 0.4f)
        {
            currentFaceIndex = 1;
        }
        else
        {
            currentFaceIndex = 0;
        }

        if (oldIndex == currentFaceIndex)
        {
            return;
        }

        for (int i = 0; i < faces.Length; i++)
        {
            faces[i].SetActive(i == currentFaceIndex);
        }

        background.color = backgroundColors[currentFaceIndex];
    }

    #endregion
}
