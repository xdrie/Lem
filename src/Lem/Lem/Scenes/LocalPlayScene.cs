using System.Linq;
using Lem.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class LocalPlayScene : BaseScene {
        private const int renderlayer_overlay = 1 << 30;
        private const int renderlayer_map = 512;

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
            
            // text
            var statusNt = CreateEntity("status_text", new Vector2(20, 20));
            var statusText = statusNt.AddComponent(new TextComponent(gameContext.assets.font, "local game", Vector2.Zero, Color.White));
            statusText.RenderLayer = renderlayer_overlay;

            // add camera
            var cam = Camera.Entity.AddComponent(new FollowCamera(me, FollowCamera.CameraStyle.LockOn));
            cam.FollowLerp = 0.3f;
            cam.RoundPosition = false;
            Camera.SetMinimumZoom(1f);
            Camera.SetMaximumZoom(2f);
            Camera.SetZoom(1f);
            cam.FocusOffset = new Vector2(120, 68); // compensate for zoom

            // update renderers
            var fixedRenderer =
                AddRenderer(new ScreenSpaceRenderer(1023, renderlayer_overlay));
            fixedRenderer.ShouldDebugRender = false;

            mainRenderer.RenderLayers.AddRange(new[] {renderlayer_map});
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