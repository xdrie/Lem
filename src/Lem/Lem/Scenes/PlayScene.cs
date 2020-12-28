using System.Reflection;
using Glint.Networking;
using Glint.Networking.EntitySystems;
using Glint.Networking.Game;
using Lime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Scenes {
    public class PlayScene : BaseScene {
        public ClientGameSyncer syncer;

        public override void Initialize() {
            base.Initialize();

            ClearColor = new Color(0x2f2732);

            gameContext.loadContent();
            Glint.Global.log.info("creating play scene");

            // set up syncer
            var netClient = new LimeClient(new LimeNode.Configuration {
                peerConfig = NetConfigurator.createClientPeerConfig(gameContext.config.netTimeout),
                messageAssemblies = new[]
                    {typeof(GameSyncer).Assembly, Assembly.GetExecutingAssembly()}
            });

            var ups = 1000 / gameContext.config.netInterval;
            var nick = "player";
            syncer = new ClientGameSyncer(netClient, "127.0.0.1", gameContext.config.netPort,
                nick, ups, ups * 2, 64,
                debug: gameContext.config.netDebug);

            // register custom message handlers
            // TODO: register handlers

            Core.Services.AddService(syncer); // register syncer
        }

        public override void OnStart() {
            base.OnStart();

            // add the processor for syncing bodies
            AddEntityProcessor(
                new BodySyncerEntitySystem(syncer,
                    SyncBodyMatcherProvider.createMatcher(Assembly.GetExecutingAssembly())) {
                    createSyncedEntity = createSyncedEntity
                });
            // start the main syncer
            syncer.connect();
        }

        public Entity createSyncedEntity(string entityName, uint syncTag) {
            throw new System.NotImplementedException();
        }

        public override void Update() {
            base.Update();

            if (Input.IsKeyPressed(Keys.Escape)) {
                // request disconnect
                syncer.disconnect();
            }
        }

        public override void Unload() {
            base.Unload();

            Core.Services.RemoveService(typeof(ClientGameSyncer)); // unregister syncer
            syncer.stop();
        }
    }
}