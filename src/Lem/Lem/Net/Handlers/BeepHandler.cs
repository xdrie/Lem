using System.Linq;
using Glint;
using Glint.Networking.Game;
using Glint.Networking.Pipeline;
using Lem.Net.Messages;

namespace Lem.Net.Handlers {
    public class BeepHandler : ClientMessageHandler<BeepMessage> {
        public BeepHandler(GameSyncer syncer) : base(syncer) { }

        public override bool handle(BeepMessage msg) {
            if (msg.sourceUid == syncer.uid) {
                Global.log.info($"I SAID beep: {msg}!");
            }
            else {
                var peer = syncer.peers.Single(x => x.uid == msg.sourceUid);
                Global.log.info($"{peer.nick} SAID beep: {msg}!");
            }

            return true;
        }
    }
}