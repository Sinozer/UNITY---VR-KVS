using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SwitchLights : MonoBehaviour
{
	[Header("Lightmaps")]
	[SerializeField] private Texture2D[] _darkLightmapDir, _darkLightmapColor;
	[SerializeField] private Texture2D[] _brightLightmapDir, _brightLightmapColor;
	
	[Header("Skyboxes")]
	[SerializeField] private Material _daySkybox;
	[SerializeField, Range(0,3)] private float _daySkyboxIntensity = 1;
	[SerializeField] private Material _nightSkybox;
	[SerializeField, Range(0,3)] private float _nightSkyboxIntensity = 0.1f;

	[Header("Neon Materials")]
	[SerializeField] private Material[] _neonMaterials;

	[Header("Lights")]
	[SerializeField] private Light _flashLight;

	[SerializeField] private Light _warningLight;
	[SerializeField, Range(0, 10)] private float _warningLightIntensity = 2;
	[SerializeField, Range(0, 10)] private float _warningLightSpeed = 2;
	
	[SerializeField] private List<SwapTexture> _swapTextures;

	[Button]
	public void TurnLightsOff()
	{
		if (_flashLight) _flashLight.enabled = true;
		if (_warningLight) _warningLight.enabled = true;
		if (_swapTextures != null)
		{
			foreach (var swapTexture in _swapTextures)
			{
				swapTexture.SetSwapMaterial();
			}
		}
		
		RenderSettings.skybox = _nightSkybox;
		RenderSettings.ambientIntensity = _nightSkyboxIntensity;
		
		LightmapSettings.lightmaps = _darkLightmap;
		foreach (var m in _neonMaterials)
		{
			m.SetColor(EmissionColor, Color.black);
		}

		_lightsOn = false;
	}

	[Button]
	public void TurnLightsOn()
	{
		if (_flashLight) _flashLight.enabled = false;
		if (_warningLight) _warningLight.enabled = false;
		if (_swapTextures != null)
		{
			foreach (var swapTexture in _swapTextures)
			{
				swapTexture.SetBaseMaterial();
			}
		}

		RenderSettings.skybox = _daySkybox;
		RenderSettings.ambientIntensity = _daySkyboxIntensity;
		
		LightmapSettings.lightmaps = _brightLightmap;
		foreach (var m in _neonMaterials)
		{
			m.SetColor(EmissionColor, Color.white);
		}

		_lightsOn = true;
	}

	private LightmapData[] _darkLightmap, _brightLightmap;
	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	private bool _lightsOn = true;

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

		TurnLightsOn();
	}

	private void Update()
	{
		if (_lightsOn) return;

		_warningLight.intensity = Mathf.PingPong(Time.time * _warningLightSpeed, _warningLightIntensity);
	}
}