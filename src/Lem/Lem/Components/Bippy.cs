using Nez;

namespace Lem.Components {
    public class Bippy : Character {
        public Bippy() : base(Core.Content.LoadTexture("Data/spr/player.png"), 8, 8) { }

        public override void Initialize() {
            base.Initialize();

            Entity.AddComponent(new CharacterBody {runSpeed = 16, jumpSpeed = 40});
            Entity.AddComponent(new BoxCollider(-3, -4, 4, 7));
        }
    }
}