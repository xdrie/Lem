using System;
using System.IO;
using System.Reflection;
using Glint;
using Glint.Config;
using Glint.Util;
using Lem.Server;
using Nez;

#if !DEBUG
using Glint.Util;
#endif

namespace Lem {
    class Program {
        public const string conf = "game.conf";

        static void Main(string[] args) {
#if CORERT
            // CoreRT switch (https://github.com/dotnet/corert/issues/6946#issuecomment-464611673)
            AppContext.SetSwitch("Switch.System.Reflection.Assembly.SimulatedLocationInBaseDirectory", true);
#endif
            var asm = Assembly.GetExecutingAssembly();
            var banner = asm.GetManifestResourceStream($"{asm.GetName().Name}.Res.banner.txt");
            using (var sr = new StreamReader(banner!)) {
                Console.WriteLine(sr.ReadToEnd());
                Console.WriteLine(Config.GAME_VERSION);
#if DEBUG
                Console.WriteLine("[DEBUG] build, debug code paths enabled.");
#endif
            }

            // load configuration
            var defaultConf = asm.GetManifestResourceStream($"{asm.GetName().Name}.Res.game.conf.example");
            var configHelper = new ConfigHelper<Config>();
            var confPath = Path.Join(Global.baseDir, conf);
            configHelper.ensureDefaultConfig(confPath, defaultConf);
            var confStr = File.ReadAllText(confPath);
            var config = configHelper.load(confStr, args); // load and parse config
            // run in crash-cradle (only if NOT debug)
#if !DEBUG
            try {
#endif
            if (args.Contains("--server")) {
                Global.log.writeLine("created server", Logger.Verbosity.Information);
                
                var host = new ServerHost();
                host.init(config);
                host.run();
            }
            else {
                using var game = new NGame(config);
                game.Run();
            }
#if !DEBUG
        }
        catch (Exception ex) {
            Global.log.writeLine($"fatal error: {ex}", Logger.Verbosity.Critical);
            throw;
        }
#endif
        }
    }
}