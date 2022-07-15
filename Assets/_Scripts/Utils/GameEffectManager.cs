using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System;

public class GameEffectManager : Singleton<GameEffectManager>
{
    [SerializeField] private Volume globalVolume = null;
    private VolumeComponent currentPulseComponent = null;

    private LensDistortion ld = null;
    private Vignette vg = null;

    private bool pulse = false;
    private float pulseForce = 0.0f;
    private float pulseSpeed = 0.0f;
    private float currentPulseValue = 0.0f;

    private void Start() {
        globalVolume.profile.TryGet<LensDistortion>(out ld);
        globalVolume.profile.TryGet<Vignette>(out vg);
        DOTween.Init();
    }

    private void Update() {
        if (!currentPulseComponent) {
            return;
        }

        if (pulse) {
            currentPulseValue += Time.deltaTime * pulseSpeed;
            
            ld.intensity.value = Mathf.Sin(currentPulseValue) * Mathf.Clamp(pulseForce, 0f, 1f);

            if (currentPulseValue >= 180.0f * Mathf.Deg2Rad) {
                pulse = false;
                ld.intensity.value = 0.0f;
                currentPulseValue = 0.0f;
            }
        }
    }

    public void DistortionPulse(float pulseForce, float pulseSpeed) {
        currentPulseComponent = ld;
        pulse = true;

        this.pulseForce = pulseForce;
        this.pulseSpeed = pulseSpeed;
    }

    public void VignetteIntensityLerp(float target, float duration, Action OnComplete) {

        DOTween.To(() => vg.intensity.value, (x) => vg.intensity.value = x, target, duration).OnComplete(() => OnComplete?.Invoke());
    }

    public void VignetteSmoothnessLerp(float target, float duration, Action OnComplete) {
        DOTween.To(() => vg.smoothness.value, (x) => vg.smoothness.value = x, target, duration).OnComplete(() => OnComplete?.Invoke());
    }
}
