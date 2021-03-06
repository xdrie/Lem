using Glint;
using Glint.Networking;
using Glint.Networking.Pipeline;
using Lem.Net.Messages;

namespace Lem.Net.Relays {
    public class BeepRelay : ServerMessageRelay<BeepMessage> {
        public BeepRelay(GlintNetServerContext context) : base(context) { }

        protected override ProcessResult process(BeepMessage msg) {
            Global.log.info($"relayed beep: {msg}");
            return base.process(msg);
        }
    }
}