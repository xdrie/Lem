using Glint;
using Glint.Networking;
using Glint.Networking.Game;
using Glint.Util;
using Lem.Net.Relays;
using Lime;

namespace Lem.Server {
    public class ServerHost {
        public GlintNetServer server;

        public void init(Config cfg) {
            Global.log.verbosity = (Logger.Verbosity) cfg.verbosity;
            
            // 1. configure the server
            var serverConfig = new GlintNetServerContext.Config {
                ups = cfg.netUps,
                logMessages = cfg.netDebug,
            };

            var peerConfig = NetConfigurator.createServerPeerConfig("Glint-Lem", cfg.netPort, cfg.netTimeout);
            
            var node = new LimeServer(new LimeNode.Configuration {
                peerConfig = peerConfig,
                messageAssemblies = new [] {
                    typeof(NetPlayer).Assembly, typeof(NGame).Assembly
                }
            });

            server = new GlintNetServer(node, serverConfig);

            // register custom message handlers
            server.handlers.register(new BeepRelay(server.context));
        }

        public void run() {
            Global.log.writeLine("server started", Logger.Verbosity.Information);
            server.start();
        }
    }
}