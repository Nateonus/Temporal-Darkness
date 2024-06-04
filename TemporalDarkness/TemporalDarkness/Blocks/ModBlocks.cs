using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace TemporalDarkness.Blocks
{
    public static class ModBlocks
    {
        /// <summary>
        /// Registers all mod blocks.
        /// </summary>
        public static void RegisterAllBlocks(ICoreAPI api)
        {
            Dictionary<string, Type> blocks = new Dictionary<string, Type>()
            {
                //Add mod blocks here to register.
            };

            foreach (KeyValuePair<string, Type> block in blocks)
            {
                api.RegisterBlockClass("temporaldarkness-"+block.Key, block.Value);
            }
        }
    }
}
