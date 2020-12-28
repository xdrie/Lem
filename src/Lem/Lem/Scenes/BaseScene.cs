using Glint.Scenes;

namespace Lem.Scenes {
    public abstract class BaseScene : BaseGameScene<GameContext, Config> {
        public override void Initialize() {
            base.Initialize();

            ClearColor = gameContext.assets.bgColor;
        }
    }
}