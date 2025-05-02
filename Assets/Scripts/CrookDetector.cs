using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CrookDetector : MonoBehaviour
{
    public event Action Detected;
    public event Action Released;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Detected?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Crook _) == false)
            return;

        Released?.Invoke();
    }
}