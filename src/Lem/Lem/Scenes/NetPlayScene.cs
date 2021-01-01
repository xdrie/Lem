using System.Linq;
using System.Reflection;
using Glint;
using Glint.Networking;
using Glint.Networking.EntitySystems;
using Glint.Networking.Game;
using Lem.Components;
using Lem.Net.Handlers;
using Lem.Net.Messages;
using Lime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class NetPlayScene : BaseScene {
        private const int renderlayer_overlay = 1 << 30;
        private const int renderlayer_map = 512;

        public ClientGameSyncer syncer;
        private string netHost;
        private TextComponent statusText;

        public override void Initialize() {
            base.Initialize();

            ClearColor = new Color(219, 207, 177);

            // SetDesignResolution(480, 270, SceneResolutionPolicy.ShowAllPixelPerfect);

            // load map
            var mapAsset = Content.LoadTiledMap("Data/map/hill1.tmx");
            var mapNt = CreateEntity("map");
            var mapRen =
                mapNt.AddComponent(new TiledMapRenderer(mapAsset, "ground", true, Constants.Colliders.TAG_MAP));

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
            statusText =
                statusNt.AddComponent(new TextComponent(gameContext.assets.font, string.Empty, Vector2.Zero,
                    Color.White));
            statusText.RenderLayer = renderlayer_overlay;

            // add camera
            var cam = Camera.Entity.AddComponent(new FollowCamera(me, FollowCamera.CameraStyle.LockOn));
            cam.FollowLerp = 0.3f;
            cam.RoundPosition = false;
            Camera.SetMinimumZoom(1f);
            Camera.SetMaximumZoom(4f);
            Camera.SetZoom(1f);
            cam.FocusOffset = new Vector2(360, 202); // compensate for zoom

            // update renderers
            var fixedRenderer =
                AddRenderer(new ScreenSpaceRenderer(1023, renderlayer_overlay));
            fixedRenderer.ShouldDebugRender = false;

            mainRenderer.RenderLayers.AddRange(new[] {renderlayer_map});

            // set up syncer
            var netClient = new LimeClient(new LimeNode.Configuration {
                peerConfig = NetConfigurator.createClientPeerConfig("Glint-Lem", gameContext.config.netTimeout),
                messageAssemblies = new[]
                    {typeof(GameSyncer).Assembly, Assembly.GetExecutingAssembly()}
            });

            var ups = 1000 / gameContext.config.netUps;
            var nick = "player";
            netHost = gameContext.config.netHost;
            syncer = new ClientGameSyncer(netClient, netHost, gameContext.config.netPort,
                nick, ups, ups, 64,
                debug: gameContext.config.netDebug);
            syncer.connectionStatusChanged += connectionChanged;
            Core.Services.AddService(syncer); // register syncer

            // register custom message handlers
            syncer.handlers.register(new BeepHandler(syncer));

            // add the processor for syncing bodies
            AddEntityProcessor(
                new RemoteBodySyncerSystem(syncer,
                    RemoteBodySyncerSystem.createMatcher(Assembly.GetExecutingAssembly())) {
                    createSyncedEntity = createSyncedEntity
                });
        }

        public override void OnStart() {
            base.OnStart();
            // start the main syncer
            syncer.connect();
            Global.log.info($"connecting to server {netHost}");
            statusText.Text = "connecting...";
        }

        private void connectionChanged(bool connected) {
            if (connected) {
                Global.log.info("connected to server");
                statusText.Text = "connected";
            }
            else {
                Global.log.info("disconnected from server");
                statusText.Text = "disconnected";
                TransitionScene<MenuScene>();
            }
        }

        public Entity createSyncedEntity(string entityName, uint syncTag) {
            var syncNt = CreateEntity(entityName);
            var component = default(Component);
            switch (syncTag) {
                case Constants.SyncTags.TAG_PLAYER:
                    var bippy = syncNt.AddComponent<Bippy>();
                    bippy.spriteRenderer.Color = new Color(220, 160, 255);
                    component = bippy;
                    break;
                default:
                    return null;
            }

            component.Entity = syncNt;

            return syncNt;
        }

        public override void Update() {
            base.Update();

            if (Input.IsKeyPressed(Keys.Escape) ||
                (Input.GamePads.Length > 0 && Input.GamePads[0].IsButtonPressed(Buttons.Back))) {
                // request disconnect
                syncer.disconnect();
                TransitionScene<MenuScene>();
            }

            if (Input.IsKeyPressed(Keys.Space) ||
                (Input.GamePads.Length > 0 && Input.GamePads[0].IsButtonPressed(Buttons.Start))) {
                // send net signal
                var beep = syncer.createGameUpdate<BeepMessage>();
                beep.pips = Random.Range(1, 8);
                syncer.sendGameUpdate(beep);
            }

            if (Input.IsKeyPressed(Keys.OemQuestion)) {
                Core.DebugRenderEnabled = !Core.DebugRenderEnabled;
            }
        }

        public override void Unload() {
            base.Unload();

            Core.Services.RemoveService(typeof(ClientGameSyncer)); // unregister syncer
            syncer.stop();
            syncer.Dispose();
        }
    }
}