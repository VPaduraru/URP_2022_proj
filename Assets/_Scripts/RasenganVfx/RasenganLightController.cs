using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class RasenganLightController : MonoBehaviour
{
    [SerializeField]
    private float _defaultIntensity = 15f;
    [SerializeField]
    private float _lightChangingRate = 1f;
    [SerializeField]
    private float range = 3f;
    private Light _pointLight;
    // Start is called before the first frame update
    void Start()
    {
        _pointLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        float perlinNoiseVal = Mathf.PerlinNoise(Time.time * _lightChangingRate, Time.time * _lightChangingRate);
        float pNoiseRemapped = Mathf.Lerp(-range, range, perlinNoiseVal);

        _pointLight.intensity = _defaultIntensity + pNoiseRemapped;
    }
}
