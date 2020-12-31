using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Components {
    public abstract class InputController : Component {
        public VirtualJoystick move = new VirtualJoystick(true);
        public VirtualButton jump = new VirtualButton();
    }

    public class PlayerController : InputController {
        public override void Initialize() {
            base.Initialize();

            move.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            jump.AddKeyboardKey(Keys.Z);
        }
    }
}