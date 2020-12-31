using Glint.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez;

namespace Lem.Components {
    public abstract class Character : GAnimatedSprite, IUpdatable {
        protected Character(Texture2D texture, int width, int height) : base(texture, width, height) { }
        
        public virtual void Update() {
            // TODO: stuff
        }
    }
}