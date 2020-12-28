using Glint.Config;
using Glint.Networking;
using Microsoft.Xna.Framework;
using Nez;
using Nez.BitmapFonts;

namespace Lem {
    public class Config : GameConfig {
        public const string GAME_NAME = "Lem";
        public const string GAME_VERSION = "0.1.0";

        public override string title => GAME_NAME;
        public override string version => GAME_VERSION;

        // - server config
        public const string NET = "net";
        public int netPort = GlintNetServer.DEF_PORT;
        public int netTimeout = GlintNetServer.DEF_TIMEOUT;
        public int netInterval = GlintNetServer.DEF_INTERVAL;
        public bool netDebug = false;

        public override void load() {
            base.load();

            pr.bind(ref netPort, NET, rename(nameof(netPort)));
            pr.bind(ref netTimeout, NET, rename(nameof(netTimeout)));
            pr.bind(ref netInterval, NET, rename(nameof(netInterval)));
            pr.bind(ref netDebug, NET, rename(nameof(netDebug)));
        }
    }

    public class GameContext : ContextBase<Config> {
        public Assets assets { get; } = new Assets();

        public class Assets {
            public BitmapFont font;

            // - default palette
            public Color[] palette = {
                new Color(237, 229, 206), // white
                new Color(142, 156, 157), // gray
                new Color(170, 92, 86), // orange
                new Color(136, 113, 99), // brown
                new Color(47, 39, 50), // purple
            };

            public ref Color fgColor => ref paletteWhite;
            public ref Color bgColor => ref palettePurple;

            public ref Color paletteWhite => ref palette[0];
            public ref Color paletteGray => ref palette[1];
            public ref Color paletteOrange => ref palette[2];
            public ref Color paletteBrown => ref palette[3];
            public ref Color palettePurple => ref palette[4];

            // some more useful colors
            public Color colGreen = new Color(137, 202, 143);
            public Color colBlue = new Color(98, 161, 179);
            public Color colYellow = new Color(190, 175, 91);
            public Color colOrange = new Color(189, 133, 91);
            public Color colRed = new Color(189, 91, 91);
        }

        public GameContext(Config config) : base(config) { }

        public override void loadContent() {
            assets.font = Core.Content.LoadBitmapFont("Data/fonts/pixel_square.fnt");
        }
    }
}