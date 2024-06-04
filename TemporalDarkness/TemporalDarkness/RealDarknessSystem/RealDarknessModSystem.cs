using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Common;

namespace TemporalDarkness.RealDarknessSystem
{
    /// <summary>
    /// This mod system controls the darkness effect at night time.
    /// </summary>
    internal class RealDarknessModSystem : ModSystem
    {
        AmbientModifier nightTimeDarkness;
        ICoreClientAPI capi;

        long commonTickID;
        long uncommonTickID;

        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);
            capi = api;

            nightTimeDarkness = new AmbientModifier()
            {
                FogColor = new WeightedFloatArray(new float[] { 0, 0, 0 }, 1),
                FogDensity = new WeightedFloat(0.15f, 0)
            }.EnsurePopulated();
            api.Ambient.CurrentModifiers.Add("temporaldarkness-realdarkness", nightTimeDarkness);

            commonTickID = api.Event.RegisterGameTickListener(ModifyWeightsBasedOnTime, 200);
            uncommonTickID = api.Event.RegisterGameTickListener(EnsureAmbientModifierExists, 5000);
        }

        public void ModifyWeightsBasedOnTime(float dt)
        {
            if (capi == null || capi.World == null) return;
            if (capi.World.Calendar is ClientGameCalendar cCalendar)
            {
                float strength = 1 - Math.Clamp(cCalendar.SunLightStrength, 0f, 1f);
                strength = GameMath.Lerp(-1.5f, 1.5f, strength);
                if (strength <= 0.005f) strength = 0;
                if (strength >= 0.995f) strength = 1;
                nightTimeDarkness.FogColor.Weight = 1;
                nightTimeDarkness.FogDensity.Weight = strength;
            }
        }

        /// <summary>
        /// Ensures the ambient modifier still exists in the api's modifiers.
        /// </summary>
        public void EnsureAmbientModifierExists(float dt)
        {
            AmbientModifier stored;
            if (capi.Ambient.CurrentModifiers.TryGetValue("temporaldarkness-realdarkness",out stored))
            {
                if (stored == nightTimeDarkness)
                {
                    //No changes needed...
                    return;
                }
            }
            capi.Ambient.CurrentModifiers["temporaldarkness-realdarkness"] = nightTimeDarkness;
        }

        public override void Dispose()
        {
            capi.Event.UnregisterGameTickListener(commonTickID);
            capi.Event.UnregisterGameTickListener(uncommonTickID);
        }

    }
}
