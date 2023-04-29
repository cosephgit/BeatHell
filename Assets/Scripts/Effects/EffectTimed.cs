using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for short-lived particle systems
// self-destroys after the duration timer
// Trigger should be called immediately when instantiated
// automatically plays any attached sound and particle systems when instantiated

public class EffectTimed : MonoBehaviour
{
    [SerializeField]private AudioSource effectSound;
    [SerializeField]private ParticleSystem effectParticles;
    [SerializeField]private float effectDuration = 1f;

    public void Trigger(Color color, Vector3 scale)
    {
        if (effectSound) effectSound.Play();
        if (effectParticles)
        {
            ParticleSystem.MainModule module = effectParticles.main;
            transform.localScale = scale;
            module.startColor = color;
            effectParticles.Play();
        }
    }

    private void Update()
    {
        effectDuration -= Time.deltaTime;
        if (effectDuration < 0) Destroy(gameObject);
    }
}
