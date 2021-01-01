using System;
using Glint;
using Glint.Networking.Components;
using Glint.Networking.Game;
using Glint.Physics;
using Lem.Game;
using Microsoft.Xna.Framework;
using Nez;

namespace Lem.Components.Characters {
    public abstract class CharacterBody : SyncBody {
        private InputController? input;

        public bool isRemote { get; private set; }
        public float runSpeed;
        public float jumpSpeed;
        public bool canJump = true;
        private float shootTimer = 0;
        private const float shootDelay = 1f;
        private const float muzzleVel = 100f;

        public override void Initialize() {
            base.Initialize();

            accel = new Vector2(0, 80f);
            
            // check if remote
            var syncer = Core.Services.GetService<ClientGameSyncer>();
            isRemote = syncer?.uid != bodyId;
        }

        public override void OnAddedToEntity() {
            base.OnAddedToEntity();

            input = Entity.GetComponent<InputController>();
            if (input == null && !isRemote) {
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
            
            // 3. shoot
            if (input.shoot && Time.TotalTime >= shootTimer) {
                shootTimer = Time.TotalTime + shootDelay;
                // make a bullet
                var xDir = Entity.GetComponent<Character>().facing == Direction.Right ? 1 : -1;
                ThingMaker.makeBullet(Entity.Position + new Vector2(xDir * 4, 1), muzzleVel, xDir);
            }
        }

        protected override void applyMotion(Vector2 posDelta) {
            var motion = posDelta;
            if (Entity.GetComponent<BoxCollider>().CollidesWithAnyMultiple(motion, out var results)) {
                foreach (var res in results) {
                    var ground = res.Normal.Y < 0;
                    var roof = res.Normal.Y > 0;
                    var leftWall = res.Normal.X > 0;
                    var rightWall = res.Normal.X > 0;

                    if (res.Collider.Tag == Constants.Colliders.TAG_MAP) {
                        // collision with a wall
                        motion -= res.MinimumTranslationVector;

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

                    if (res.Collider.Tag == Constants.Colliders.TAG_CHARACTER) {
                        if (ground) {
                            if (velocity.Y > 0) {
                                // headstool
                                var other = res.Collider.Entity.GetComponent<KinBody>();
                                other.velocity.Y += velocity.Y; // give my velocity to them
                            }

                            velocity.Y = 0; // zero out mine
                            canJump = true;
                        }
                    }
                }
            }

            base.applyMotion(motion);
        }

        public override InterpolationType interpolationType { get; } = InterpolationType.Linear;
    }
}