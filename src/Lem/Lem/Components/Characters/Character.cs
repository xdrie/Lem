using System;
using Glint.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;

namespace Lem.Components.Characters {
    public abstract class Character : GAnimatedSprite, IUpdatable {
        public CharacterBody? body;
        public BoxCollider? hitbox;
        
        protected Character(Texture2D texture, int width, int height) : base(texture, width, height) { }

        public override void Initialize() {
            base.Initialize();
        }

        public virtual void Update() {
            if (body == null) return;
                
            // update animation
            var anim = "idle";
            var loop = SpriteAnimator.LoopMode.Loop;
            var movingX = Math.Abs(body.velocity.X) > 0;
            var movingY = Math.Abs(body.velocity.Y) > 0;

            if (movingX) {
                anim = "run";
                facing = body.velocity.X >= 0 ? Direction.Right : Direction.Left;
                animator.FlipX = facing == Direction.Left;
            }

            if (movingY) {
                anim = "jump";
                loop = SpriteAnimator.LoopMode.Once;
            }

            if (!animator.IsAnimationActive(anim))
                animator.Play(anim, loop);
        }
    }
}