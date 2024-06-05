using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemporalDarkness.Config
{
    public class ServerConfig
    {
        /// <summary>
        /// Simple singleton access to the config.
        /// I know this is "bad practice", sod off.
        /// </summary>
        public static ServerConfig main;

        /// <summary>
        /// Should the 'Real Darkness' System be enabled?
        /// </summary>
        public bool EnableRealDarknessSystem = true; 

    }
}
