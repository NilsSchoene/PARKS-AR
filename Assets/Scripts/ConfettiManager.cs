using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiManager : MonoBehaviour
{
    /// <summary>
    /// Gets the ParticleSystem and plays it once.
    /// </summary>
    public void ThrowConfetti()
    {
        ParticleSystem confetti = GetComponent<ParticleSystem>();
        confetti.Play();
    }
}
