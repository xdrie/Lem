using Glint.Networking.Components;
using Nez;

namespace Lem.Components.Things {
    public class Bullet : Thing {
        public Bullet() : base(Core.Content.LoadTexture("Data/spr/bullet.png")) { }

        public override void Initialize() {
            base.Initialize();

            body = Entity.AddComponent(new Body());
        }

        class Body : SyncBody {
            public override uint bodyType { get; } = Constants.SyncTags.TAG_BULLET;
            public override InterpolationType interpolationType { get; } = InterpolationType.Hermite;
        }
    }
}