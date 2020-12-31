using Nez;

namespace Lem.Components {
    public class Bippy : Character {
        public Bippy() : base(Core.Content.LoadTexture("Data/spr/player.png"), 8, 8) { }

        public override void Initialize() {
            base.Initialize();
        }
    }
}