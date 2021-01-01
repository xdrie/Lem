using Glint.Networking.Components;
using Glint.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace Lem.Components.Things {
    public abstract class Thing : GAnimatedSprite {
        public SyncBody body;
        
        protected Thing(Texture2D texture, int width, int height) : base(texture, width, height) { }
        protected Thing(Texture2D texture) : this(texture, texture.Width, texture.Height) { }
    }
}