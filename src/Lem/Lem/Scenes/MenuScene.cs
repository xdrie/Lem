using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class MenuScene : BaseScene {
        public override void OnStart() {
            base.OnStart();

            var ui = CreateEntity("ui");
            var uiTitle = CreateEntity("ui_title");

            // display game version
            var versionStr = Config.GAME_VERSION;
            var versionText = ui.AddComponent(new TextComponent(gameContext.assets.font, versionStr,
                new Vector2(10, DesignResolution.Y - 20f), gameContext.assets.fgColor));

            // add placeholder title text
            var titleText = uiTitle.AddComponent(new TextComponent(gameContext.assets.font, Config.GAME_NAME,
                new Vector2(DesignResolution.X / 2f, 200f), gameContext.assets.fgColor));
            titleText.LocalOffset += new Vector2(-titleText.Width, 0);
            uiTitle.Scale = new Vector2(2f, 2f);

            // add placeholder start text
            var startStr = "Start (E) [A btn]";
            var startText = ui.AddComponent(new TextComponent(gameContext.assets.font, startStr,
                new Vector2(DesignResolution.X / 2f, 200f), gameContext.assets.fgColor));
            startText.LocalOffset += new Vector2(-startText.Width / 2f, titleText.Height * 2f);

            // add placeholder fullscreen text
            var fullscreenStr = "Fullscreen (Alt+Enter)";
            var fullscreenText = ui.AddComponent(new TextComponent(gameContext.assets.font, fullscreenStr,
                new Vector2(DesignResolution.X / 2f, 200f), gameContext.assets.fgColor));
            fullscreenText.LocalOffset += new Vector2(-fullscreenText.Width / 2f, titleText.Height * 2f + startText.Height * 2f);

            // add placeholder exit text
            var exitStr = "Exit (Esc) [Back btn]";
            var exitText = ui.AddComponent(new TextComponent(gameContext.assets.font, exitStr,
                new Vector2(DesignResolution.X / 2f, 200f), gameContext.assets.fgColor));
            exitText.LocalOffset += new Vector2(-exitText.Width / 2f, titleText.Height * 2f + startText.Height * 4f);
        }

        public override void Update() {
            base.Update();


            if (Input.IsKeyPressed(Keys.E) ||
                (Input.GamePads.Length > 0 && Input.GamePads[0].IsButtonPressed(Buttons.A))) {
                TransitionScene<NetPlayScene>();
            }

            if (Input.IsKeyPressed(Keys.Escape) ||
                (Input.GamePads.Length > 0 && Input.GamePads[0].IsButtonPressed(Buttons.Back))) {
                // end this scene[0] = GamePadData 
                Core.Exit();
            }
        }
    }
}