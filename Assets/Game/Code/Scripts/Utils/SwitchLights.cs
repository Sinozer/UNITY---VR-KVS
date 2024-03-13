using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchLights : MonoBehaviour
{
    [SerializeField] private Texture2D[] _darkLightmapDir, _darkLightmapColor;
    [SerializeField] private Texture2D[] _brightLightmapDir, _brightLightmapColor;

    [SerializeField] private Light _flashLight;
    [SerializeField] private Light _warningLight;
    
    [Button]
    public void TurnLightsOff()
    {
        if (_flashLight) _flashLight.enabled = true;
        if (_warningLight) _warningLight.enabled = true;
        LightmapSettings.lightmaps = _darkLightmap;
    }

    [Button]
    public void TurnLightsOn()
    {
        if (_flashLight) _flashLight.enabled = false;
        if (_warningLight) _warningLight.enabled = false;
        LightmapSettings.lightmaps = _brightLightmap;
    }

    private LightmapData[] _darkLightmap, _brightLightmap;

    private void Start()
    {
        List<LightmapData> dlightmap = new List<LightmapData>();

        for (int i = 0; i < _darkLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = _darkLightmapDir[i];
            lmdata.lightmapColor = _darkLightmapColor[i];

            dlightmap.Add(lmdata);
        }

        _darkLightmap = dlightmap.ToArray();

        List<LightmapData> blightmap = new List<LightmapData>();

        for (int i = 0; i < _brightLightmapDir.Length; i++)
        {
            LightmapData lmdata = new LightmapData();

            lmdata.lightmapDir = _brightLightmapDir[i];
            lmdata.lightmapColor = _brightLightmapColor[i];

            blightmap.Add(lmdata);
        }

        _brightLightmap = blightmap.ToArray();
    }
}