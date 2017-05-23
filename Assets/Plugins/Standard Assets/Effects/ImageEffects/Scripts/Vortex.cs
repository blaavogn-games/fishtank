using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Displacement/Vortex")]
    public class Vortex : ImageEffectBase
    {
        public Vector2 radius = new Vector2(0.8F,0.6F);
        public float angle = 10;
        public Vector2 center = new Vector2(0.5F, 0.5F);
        public Vector2 center2 = new Vector2(0.5F, 0.5F);

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination)
        {
            ImageEffects.RenderDistortion (material, source, destination, angle, center, radius);
        }
    }
}
