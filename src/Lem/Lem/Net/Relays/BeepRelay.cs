using Glint;
using Glint.Networking;
using Glint.Networking.Handlers;
using Lem.Net.Messages;

namespace Lem.Net.Relays {
    public class BeepRelay : ServerMessageRelay<BeepMessage> {
        public BeepRelay(GlintNetServerContext context) : base(context) { }

        protected override bool process(BeepMessage msg) {
            Global.log.info($"relayed beep: {msg}");
            return base.process(msg);
        }
    }
}