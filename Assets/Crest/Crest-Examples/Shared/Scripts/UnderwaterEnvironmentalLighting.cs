// Crest Ocean System

// This file is subject to the MIT License as seen in the root of this folder structure (LICENSE)

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Crest
{
    /// <summary>
    /// Provides out-scattering based on the camera's underwater depth. It scales down environmental lighting
    /// (directional light, reflections, ambient etc) with the underwater depth. This works with vanilla lighting, but
    /// uncommon or custom lighting will require a custom solution (use this for reference).
    /// </summary>
    [AddComponentMenu(Internal.Constants.MENU_PREFIX_EXAMPLE + "Underwater Environmental Lighting")]
    public class UnderwaterEnvironmentalLighting : CustomMonoBehaviour
    {
        /// <summary>
        /// The version of this asset. Can be used to migrate across versions. This value should
        /// only be changed when the editor upgrades the version.
        /// </summary>
        [SerializeField, HideInInspector]
#pragma warning disable 414
        int _version = 0;
#pragma warning restore 414

        [Tooltip("How much this effect applies. Values less than 1 attenuate light less underwater. Value of 1 is physically based."), SerializeField, Range(0f, 10f)]
        float _weight = 1f;

        [Tooltip("How much to scale this effect for the depth fog factor alone (for better control over the visuals)."), SerializeField, Range(0.5f, 2.5f)]
        float _depthFogFactorScale = 1f;

        [Tooltip("Seafloor object transform (to get max depth from)"), SerializeField]
        Transform _seafloorTransform;

        [Tooltip("Curve for falloff of primary light intensity only; depth range 0 to the seafloor depth"), SerializeField]
        AnimationCurve _primaryLightIntensityFalloff = AnimationCurve.EaseInOut(0f,1f, 1f,0f);


        Light _primaryLight;
        float _lightIntensity;
        float _ambientIntensity;
        float _reflectionIntensity;
        float _fogDensity;
        float _underwaterDepthFogDensityFactor;

        float _averageDensity = 0f;

        public const float DEPTH_OUTSCATTER_CONSTANT = 0.25f;

        bool _isInitialised = false;

        void OnEnable()
        {
            if (OceanRenderer.Instance == null)
            {
                enabled = false;
                return;
            }

            // Check to make sure the property exists. We might be using a test material.
            if (!OceanRenderer.Instance.OceanMaterial.HasProperty("_DepthFogDensity"))
            {
                enabled = false;
                return;
            }

            _primaryLight = OceanRenderer.Instance._primaryLight;

            // Store lighting settings
            if (_primaryLight)
            {
                _lightIntensity = _primaryLight.intensity;
            }
            _ambientIntensity = RenderSettings.ambientIntensity;
            _reflectionIntensity = RenderSettings.reflectionIntensity;
            _fogDensity = RenderSettings.fogDensity;
            _underwaterDepthFogDensityFactor = UnderwaterRenderer.DepthFogDensityFactor;

            var density = OceanRenderer.Instance.UnderwaterDepthFogDensity;
            _averageDensity = (density.x + density.y + density.z) / 3f;

            _isInitialised = true;
        }

        void OnDisable()
        {
            if (!_isInitialised)
            {
                return;
            }

            // Restore lighting settings
            if (_primaryLight)
            {
                _primaryLight.intensity = _lightIntensity;
            }
            RenderSettings.ambientIntensity = _ambientIntensity;
            RenderSettings.reflectionIntensity = _reflectionIntensity;
            RenderSettings.fogDensity = _fogDensity;
            UnderwaterRenderer.DepthFogDensityFactor = _underwaterDepthFogDensityFactor;

            _isInitialised = false;
        }

        void LateUpdate()
        {
            if (OceanRenderer.Instance == null || UnderwaterRenderer.Instance == null)
            {
                return;
            }

            var density = OceanRenderer.Instance.UnderwaterDepthFogDensity;
            _averageDensity = (density.x + density.y + density.z) / 3f;

            float depthMultiplier = Mathf.Exp(_averageDensity *
                Mathf.Min(OceanRenderer.Instance.ViewerHeightAboveWater * DEPTH_OUTSCATTER_CONSTANT, 0f) *
                _weight);

            float curvePosition = Mathf.Clamp01(OceanRenderer.Instance.ViewerHeightAboveWater / _seafloorTransform.position.y);
            float curveValue = Mathf.Clamp01(_primaryLightIntensityFalloff.Evaluate(curvePosition));

            // Darken environmental lighting when viewer underwater
            if (_primaryLight)
            {
                // ORIGINAL: use the depth multiplier as-is
                // _primaryLight.intensity = Mathf.Lerp(0, _lightIntensity, depthMultiplier);

                // CURVE-BASED: use a standard lerp
                // (both the viewer height and the seafloor position y-coord are assumed negative in this range)
                _primaryLight.intensity = curveValue * _lightIntensity;
            }
            //RenderSettings.ambientIntensity = Mathf.Lerp(0, _ambientIntensity, depthMultiplier);
            //RenderSettings.reflectionIntensity = Mathf.Lerp(0, _reflectionIntensity, depthMultiplier);
            //RenderSettings.fogDensity = Mathf.Lerp(0, _fogDensity, depthMultiplier);

            RenderSettings.ambientIntensity = curveValue * _ambientIntensity;
            RenderSettings.reflectionIntensity = curveValue * _reflectionIntensity;
            RenderSettings.fogDensity = curveValue * _fogDensity;

            // UnderwaterRenderer.DepthFogDensityFactor = Mathf.Lerp(_underwaterDepthFogDensityFactor, 0.01f, depthMultiplier / _depthFogFactorScale);
            UnderwaterRenderer.DepthFogDensityFactor = Mathf.Lerp(_underwaterDepthFogDensityFactor, 0.01f, curveValue / _depthFogFactorScale);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UnderwaterEnvironmentalLighting))]
    public class UnderwaterEnvironmentalLightingEditor : CustomBaseEditor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("This is an example component that will likely require modification to work " +
                "correctly with your project. It implements out-scattering when underwater. It does so, primarily, " +
                "by changing the intensity of the primary light. The deeper underwater, the less intense the light. " +
                "There may be unsuitable performance costs or required features to be enabled.", MessageType.Info);
            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
#endif
}
