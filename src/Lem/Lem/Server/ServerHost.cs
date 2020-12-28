using Glint;
using Glint.Networking;
using Glint.Util;

namespace Lem.Server {
    public class ServerHost {
        public GlintNetServer server;

        public void init(Config cfg) {
            // configure the server
            var serverConfig = new GlintNetServerContext.Config {
                port = cfg.netPort,
                updateInterval = cfg.netInterval,
                timeout = cfg.netTimeout,
                verbosity = (Logger.Verbosity) cfg.verbosity,
                logMessages = cfg.netDebug,
            };

            server = new GlintNetServer(serverConfig);
            server.configure();
            // register client assembly for message types
            server.context.assemblies.Add(typeof(NGame).Assembly);

            // register custom message handlers
            // TODO: custom message relay handlers
            // server.handlers.register(new SomeRelayHandler(server.context));
        }

        public void run() {
            Global.log.writeLine("server started", Logger.Verbosity.Information);
            server.run();
        }
    }
}