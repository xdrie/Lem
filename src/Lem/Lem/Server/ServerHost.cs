using Glint;
using Glint.Networking;
using Glint.Util;
using Lem.Net.Relays;

namespace Lem.Server {
    public class ServerHost {
        public GlintNetServer server;

        public void init(Config cfg) {
            Global.log.verbosity = (Logger.Verbosity) cfg.verbosity;

            // configure the server
            var serverConfig = new GlintNetServerContext.Config {
                protocol = "Glint-Lem",
                port = cfg.netPort,
                ups = cfg.netUps,
                timeout = cfg.netTimeout,
                logMessages = cfg.netDebug,
            };

            server = new GlintNetServer(serverConfig);
            server.configure();
            // register client assembly for message types
            server.context.assemblies.Add(typeof(NGame).Assembly);

            // register custom message handlers
            server.handlers.register(new BeepRelay(server.context));
        }

        public void run() {
            Global.log.writeLine("server started", Logger.Verbosity.Information);
            server.run();
        }
    }
}