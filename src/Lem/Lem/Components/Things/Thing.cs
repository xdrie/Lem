using Glint.Networking.Components;
using Glint.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Lem.Components.Things {
    public abstract class Thing : GAnimatedSprite, IUpdatable {
        public SyncBody body;
        public float lifetime { get; private set; }
        public float lifespan = 0f;

        protected Thing(Texture2D texture, int width, int height) : base(texture, width, height) { }
        protected Thing(Texture2D texture) : this(texture, texture.Width, texture.Height) { }

        public override void Initialize() {
            base.Initialize();

            lifetime = 0;
        }


        public void Update() {
            lifetime += Time.DeltaTime;
            if (lifespan > 0 && lifetime >= lifespan) {
                Entity.Destroy();
            }
        }
    }
}