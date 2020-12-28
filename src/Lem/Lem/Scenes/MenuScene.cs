using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class MenuScene : BaseScene {
        public override void OnStart() {
            base.OnStart();

            var ui = CreateEntity("ui");

            // display game version
            var versionStr = Config.GAME_VERSION;
            var versionText = ui.AddComponent(new TextComponent(gameContext.assets.font, versionStr,
                new Vector2(10, DesignResolution.Y - 20f), gameContext.assets.fgColor));

            // add placeholder title text
            var titleText = ui.AddComponent(new TextComponent(gameContext.assets.font, Config.GAME_NAME,
                new Vector2(DesignResolution.X / 2f, 200f), gameContext.assets.fgColor));
            titleText.LocalOffset += new Vector2(-titleText.Width / 2f, 0);
        }

        public override void Update() {
            base.Update();

            if (Input.IsKeyPressed(Keys.Escape)) {
                // end this scene
                Core.Exit();
            }
        }
    }
}