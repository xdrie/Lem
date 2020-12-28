using Glint.Networking.Messages;
using Lidgren.Network;
using MsgPack.Serialization;

namespace Lem.Net.Messages {
    public class BeepMessage : GameUpdateMessage {
        [MessagePackMember(2)] public int pips { get; set; }

        public override NetDeliveryMethod deliveryMethod { get; } = NetDeliveryMethod.ReliableOrdered;

        public override string ToString() {
            return $"Beep(pips={pips})";
        }
    }
}