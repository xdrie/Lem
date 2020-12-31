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

            // kbd 0
            move.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            jump.AddKeyboardKey(Keys.Z);
            
            // pad 0
            move.AddGamePadLeftStick(0);
            jump.AddGamePadButton(0, Buttons.A);
        }
    }
}