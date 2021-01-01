using Lem.Components.Things;
using Microsoft.Xna.Framework;
using Nez;

namespace Lem.Game {
    public static class ThingMaker {
        public static void makeBullet(Vector2 pos, float speed, int dir) {
            var bulletNt = Core.Scene.CreateEntity("bullet", pos);
            var bullet = bulletNt.AddComponent(new Bullet());
            bullet.body.velocity = new Vector2(dir * speed, 0);
            bullet.spriteRenderer.FlipX = dir < 0;
            bullet.lifespan = 1f;
        }
    }
}