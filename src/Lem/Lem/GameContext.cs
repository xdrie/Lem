using Glint.Config;
using Glint.Networking;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;

namespace Lem {
    public class Config : GameConfig {
        public const string GAME_NAME = "Lem";
        public const string GAME_VERSION = "v0.2.3";

        public override string title => GAME_NAME;
        public override string version => GAME_VERSION;

        // - server config
        public const string NET = "net";
        public int netPort = 14887;
        public string netHost = "127.0.0.1";
        public int netTimeout = GlintNetServer.DEF_TIMEOUT;
        public int netUps = GlintNetServer.DEF_UPS;
        public bool netDebug = false;

        public override void load() {
            base.load();

            pr.bind(ref netPort, NET, "port");
            pr.bind(ref netHost, NET, "host");
            pr.bind(ref netTimeout, NET, "timeout");
            pr.bind(ref netUps, NET, "ups");
            pr.bind(ref netDebug, NET, "debug");
        }
    }

    public class GameContext : ContextBase<Config> {
        public Assets assets { get; } = new();

        public class Assets {
            public BitmapFont font;

            // - default palette
            public Color[] palette = {
                new(237, 229, 206), // white
                new(142, 156, 157), // gray
                new(170, 92, 86), // orange
                new(136, 113, 99), // brown
                new(47, 39, 50), // purple
            };

            public ref Color fgColor => ref paletteWhite;
            public ref Color bgColor => ref palettePurple;

            public ref Color paletteWhite => ref palette[0];
            public ref Color paletteGray => ref palette[1];
            public ref Color paletteOrange => ref palette[2];
            public ref Color paletteBrown => ref palette[3];
            public ref Color palettePurple => ref palette[4];

            // some more useful colors
            public Color colGreen = new(137, 202, 143);
            public Color colBlue = new(98, 161, 179);
            public Color colYellow = new(190, 175, 91);
            public Color colOrange = new(189, 133, 91);
            public Color colRed = new(189, 91, 91);
        }

        public GameContext(Config config) : base(config) { }

        public override void loadContent() {
            assets.font = Core.Content.LoadBitmapFont("Data/fonts/pixel_square.fnt");
        }
    }
}