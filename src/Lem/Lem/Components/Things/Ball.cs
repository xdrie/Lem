using Glint.Networking.Components;
using Nez;

namespace Lem.Components.Things {
    public class Ball : Thing {
        public Ball() : base(Core.Content.LoadTexture("Data/spr/ball.png")) { }

        public override void Initialize() {
            base.Initialize();

            body = Entity.AddComponent(new Body());
        }

        class Body : SyncBody {
            public override uint bodyType { get; } = Constants.SyncTags.TAG_BALL;
            public override InterpolationType interpolationType { get; } = InterpolationType.Hermite;
        }
    }
}