using System.Linq;
using Nez;

namespace Lem.Components {
    public class Bippy : Character {
        public Bippy() : base(Core.Content.LoadTexture("Data/spr/player.png"), 8, 8) { }

        public override void Initialize() {
            base.Initialize();

            body = Entity.AddComponent(new CharacterBody {runSpeed = 16, jumpSpeed = 40});
            hitbox = Entity.AddComponent(new BoxCollider(-3, -4, 4, 7) {Tag = Constants.Colliders.TAG_CHARACTER});

            // animation
            var fps = 10;

            animator.AddAnimation("idle", sprites.Skip(6).Take(4).ToArray(), 2);
            animator.AddAnimation("run", sprites.Skip(0).Take(6).ToArray(), fps);
            animator.AddAnimation("jump", sprites.Skip(10).Take(4).ToArray(), fps);
            animator.AddAnimation("hit", sprites.Skip(14).Take(1).ToArray(), fps);
        }
    }
}