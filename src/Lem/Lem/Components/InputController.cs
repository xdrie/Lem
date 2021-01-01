using Microsoft.Xna.Framework.Input;
using Nez;

namespace Lem.Components {
    public abstract class InputController : Component {
        public VirtualJoystick move = new(true);
        public VirtualButton jump = new();
        public VirtualButton shoot = new();
    }

    public class PlayerController : InputController {
        public override void Initialize() {
            base.Initialize();

            // kbd 0
            move.AddKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right, Keys.Up, Keys.Down);
            jump.AddKeyboardKey(Keys.Z);
            shoot.AddKeyboardKey(Keys.X);
            
            // pad 0
            move.AddGamePadLeftStick(0);
            jump.AddGamePadButton(0, Buttons.A);
            shoot.AddGamePadButton(0, Buttons.X);
        }
    }
}