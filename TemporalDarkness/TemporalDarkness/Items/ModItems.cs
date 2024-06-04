using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Common;

namespace TemporalDarkness.Blocks
{
    public static class ModItems
    {
        /// <summary>
        /// Registers all mod items.
        /// </summary>
        public static void RegisterAllItems(ICoreAPI api)
        {
            Dictionary<string, Type> items = new Dictionary<string, Type>()
            {
                //Add mod items here to register.
            };

            foreach (KeyValuePair<string, Type> item in items)
            {
                api.RegisterItemClass("temporaldarkness-" + item.Key, item.Value);
            }
        }
    }
}
