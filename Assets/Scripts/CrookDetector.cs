using System;
using UnityEngine;

public class CrookDetector : MonoBehaviour
{
    public event Action Enable;
    public event Action Disable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Enable?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Disable?.Invoke();
    }
}