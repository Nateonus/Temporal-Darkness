using System;
using TemporalDarkness.Config;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
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

        public override void StartServerSide(ICoreServerAPI api)
        {
            //The real-darkness system should be enabled/disabled server side.
            //Here we load it from the config and use the world config to share the data to clients.
            api.World.Config.SetBool("temporaldarkness-realdarkness", ServerConfig.main.EnableRealDarknessSystem);
            base.StartServerSide(api);
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            base.StartClientSide(api);

            //We sent whether the real darkness is enabled or disabled from the server, so let's access that.
            bool enableDarkness = api.World.Config.GetBool("temporaldarkness-realdarkness", true);
            if (!enableDarkness)
            {
                api.Logger.Log(EnumLogType.Event, "[Temporal Darkness] This server has disabled real darkness.");
                return;
            }

            capi = api;

            //We use a pitch-black fog ambient modifier to make the nights stronger.
            //The fog color has a weight of 1, since this'll always be black. However the weight of the density will change based on time.
            nightTimeDarkness = new AmbientModifier()
            {
                FogColor = new WeightedFloatArray(new float[] { 0, 0, 0 }, 1),
                FogDensity = new WeightedFloat(0.15f, 0)
            }.EnsurePopulated();
            api.Ambient.CurrentModifiers.Add("temporaldarkness-realdarkness", nightTimeDarkness);

            //Register the tick that modify's the ambient modifier for every 0.2s.
            commonTickID = api.Event.RegisterGameTickListener(ModifyWeightsBasedOnTime, 200);

            //Register the tick that checks the ambient modifier exists every 5 seconds.
            uncommonTickID = api.Event.RegisterGameTickListener(EnsureAmbientModifierExists, 5000);
        }

        /// <summary>
        /// Calculates the weights of the darkness based on the sun's brightness.
        /// </summary>
        /// <param name="dt"></param>
        public void ModifyWeightsBasedOnTime(float dt)
        {
            //Client only.
            if (capi == null || capi.World == null) return;
            if (capi.World.Calendar is ClientGameCalendar cCalendar)
            {
                //Just some funky maths - Essentially SunLightStrength seems to vary between 0.1 and 0.7 usually.
                //I'm stretching those values to make the drop-off more significant.
                //Using the SunLightStrength means this system reacts completely to seasonal changes to times - Winters have a considerably longer dark period.
                float strength = 1 - Math.Clamp(cCalendar.SunLightStrength, 0f, 1f);
                strength = GameMath.Lerp(-1.5f, 1.5f, strength);
                if (strength <= 0.005f) strength = 0;
                if (strength >= 0.995f) strength = 1;
                //Finally, set the weight of the fog density.
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
            if (capi == null) return;
            if (commonTickID != 0) capi.Event.UnregisterGameTickListener(commonTickID);
            if (uncommonTickID != 0) capi.Event.UnregisterGameTickListener(uncommonTickID);
        }

    }
}
