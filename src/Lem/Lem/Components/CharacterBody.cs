using System;
using System.Collections.Generic;
using Glint;
using Glint.Physics;
using Glint.Util;
using Microsoft.Xna.Framework;
using Nez;

namespace Lem.Components {
    public class CharacterBody : KinBody {
        private InputController? input;

        public float runSpeed;
        public float jumpSpeed;
        public bool canJump = true;

        public override void Initialize() {
            base.Initialize();

            accel = new Vector2(0, 80f);
        }

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();

            input = Entity.GetComponent<InputController>();
            if (input == null) {
                Global.log.err($"char body couldn't find input controller on {Entity}");
            }
        }

        public override void Update() {
            base.Update();

            updateInput();
        }

        private void updateInput() {
            if (input == null) return;
            
            // 1. left right running
            var lrMove = input.move.Value.X;

            var xSpeed = runSpeed * lrMove;
            if (Math.Abs(xSpeed) > 0) {
                velocity.X = xSpeed;
            }
            else {
                velocity.X = 0;
            }
            
            // 2. jump
            if (input.jump && canJump) {
                velocity.Y = -jumpSpeed;
                canJump = false;
            }
        }

        protected override void applyMotion(Vector2 posDelta) {
            var motion = posDelta;
            if (Entity.GetComponent<BoxCollider>().CollidesWithAnyMultiple(motion, out var results)) {
                foreach (var res in results) {
                    if (res.Collider.Entity.HasComponent<TiledMapRenderer>()) {
                        // collision with a wall
                        motion -= res.MinimumTranslationVector;

                        var ground = res.Normal.Y < 0;
                        var roof = res.Normal.Y > 0;
                        var leftWall = res.Normal.X > 0;
                        var rightWall = res.Normal.X > 0;
                        
                        if (ground || roof) {
                            velocity.Y = 0;
                        }

                        if (leftWall || rightWall) {
                            velocity.X = 0;
                        }
                        
                        if (ground) { // hit ground
                            canJump = true;
                        }
                    }       
                }
            }

            base.applyMotion(motion);
        }
    }
}