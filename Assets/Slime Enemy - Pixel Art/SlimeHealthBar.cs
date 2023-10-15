using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlimeHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    // Add a reference to the health bar's RectTransform.
    [SerializeField] private RectTransform healthBarRectTransform;
    private Vector3 initialHealthBarScale;

    private void Start()
    {
        // Cache the initial local scale of the health bar RectTransform.
        if (healthBarRectTransform != null)
        {
            initialHealthBarScale = healthBarRectTransform.localScale;
        }
    }

    public void UpdateHPBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;

        // Ensure that the health bar's local scale is not affected by the slime's flip.
        if (healthBarRectTransform != null)
        {
            // Set the local scale of the health bar RectTransform based on the initial scale.
            healthBarRectTransform.localScale = new Vector3(
                Mathf.Abs(initialHealthBarScale.x) * Mathf.Sign(target.localScale.x),
                initialHealthBarScale.y,
                initialHealthBarScale.z
            );
        }
    }
}