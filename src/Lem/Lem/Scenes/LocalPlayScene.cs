using System.Linq;
using Lem.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class LocalPlayScene : BaseScene {
        public override void Initialize() {
            base.Initialize();

            ClearColor = new Color(219, 207, 177);

            SetDesignResolution(480, 270, SceneResolutionPolicy.ShowAllPixelPerfect);

            // load map
            var mapAsset = Content.LoadTiledMap("Data/map/hill1.tmx");
            var mapNt = CreateEntity("map");
            var mapRen = mapNt.AddComponent(new TiledMapRenderer(mapAsset, "ground", true));

            // get spawn point
            var markGroup = mapAsset.GetObjectGroup("mark");
            var spawnPoints = markGroup.ObjectsWithType("spawn");
            var meSpawn = spawnPoints.First();

            // spawn player
            var me = CreateEntity("player", new Vector2(meSpawn.X, meSpawn.Y));
            me.AddComponent(new PlayerController());
            me.AddComponent(new Bippy());

            // add camera
            var cam = Camera.Entity.AddComponent(new FollowCamera(me, FollowCamera.CameraStyle.LockOn));
            cam.FollowLerp = 0.3f;
            cam.RoundPosition = false;
            Camera.SetMinimumZoom(1f);
            Camera.SetMaximumZoom(2f);
            Camera.SetZoom(1f);
            cam.FocusOffset = new Vector2(120, 68); // compensate for zoom
        }

        public override void Update() {
            base.Update();

            if (Input.IsKeyPressed(Keys.Escape)) {
                TransitionScene<MenuScene>();
            }

            if (Input.IsKeyPressed(Keys.OemQuestion)) {
                Core.DebugRenderEnabled = !Core.DebugRenderEnabled;
            }
        }
    }
}