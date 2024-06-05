using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace TemporalDarkness.Config
{
    public class ConfigLoaderModSystem : ModSystem
    {

        const string CLIENT = "temporaldarkness/client.json";
        const string SERVER = "temporaldarkness/server.json";

        public override double ExecuteOrder()
        {
            // We want to load the config before anything else, so let's make sure we do.
            return 0;
        }

        public override void StartPre(ICoreAPI api)
        {
            //We need to load custom StartPre's based on api side.
            if (api is ICoreClientAPI capi) StartPreClient(capi);
            else if (api is ICoreServerAPI sapi) StartPreServer(sapi);
        }

        /// <summary>
        /// StartPre for the client.
        /// </summary>
        void StartPreClient(ICoreClientAPI capi)
        {
            //Load the client config if it exists.
            try
            {
                ClientConfig.main = capi.LoadModConfig<ClientConfig>(CLIENT);
                if (ClientConfig.main == null)
                {
                    ClientConfig.main = new ClientConfig();
                    capi.StoreModConfig(ClientConfig.main, CLIENT);
                }
            }
            catch
            {
                ClientConfig.main = new ClientConfig();
                capi.StoreModConfig(ClientConfig.main, CLIENT);
            }
        }

        /// <summary>
        /// StartPre for the server.
        /// </summary>
        void StartPreServer(ICoreServerAPI sapi)
        {
            //Load the server config if it exists.
            try
            {
                ServerConfig.main = sapi.LoadModConfig<ServerConfig>(SERVER);
                if (ServerConfig.main == null)
                {
                    ServerConfig.main = new ServerConfig();
                    sapi.StoreModConfig(ServerConfig.main, SERVER);
                }
            }
            catch
            {
                ServerConfig.main = new ServerConfig();
                sapi.StoreModConfig(ServerConfig.main, SERVER);
            }
        }

    }
}
