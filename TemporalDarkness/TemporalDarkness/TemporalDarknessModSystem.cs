using TemporalDarkness.Blocks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace TemporalDarkness
{
    public class TemporalDarknessModSystem : ModSystem
    {
        /// <summary>
        /// Main Mod Entry Point.
        /// </summary>
        /// <param name="api"></param>
        public override void Start(ICoreAPI api)
        {
            ModBlocks.RegisterAllBlocks(api);
            ModItems.RegisterAllItems(api);
        }

    }
}
