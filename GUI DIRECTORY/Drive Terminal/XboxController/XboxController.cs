using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace XboxController {

    // A delegate type for hooking up change notifications.
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class XboxController {

        // Set up Button events
        public event ChangedEventHandler ButtonAPressed;
        public event ChangedEventHandler ButtonAReleased;

        public event ChangedEventHandler ButtonBPressed;
        public event ChangedEventHandler ButtonBReleased;

        public event ChangedEventHandler ButtonXPressed;
        public event ChangedEventHandler ButtonXReleased;

        public event ChangedEventHandler ButtonYPressed;
        public event ChangedEventHandler ButtonYReleased;

        public event ChangedEventHandler ButtonStartPressed;
        public event ChangedEventHandler ButtonStartReleased;

        public event ChangedEventHandler ButtonBackPressed;
        public event ChangedEventHandler ButtonBackReleased;

        public event ChangedEventHandler ButtonLeftShoulderPressed;
        public event ChangedEventHandler ButtonLeftShoulderReleased;

        public event ChangedEventHandler ButtonRightShoulderPressed;
        public event ChangedEventHandler ButtonRightShoulderReleased;

        public event ChangedEventHandler ThumbStickLeftPressed;
        public event ChangedEventHandler ThumbStickLeftReleased;

        public event ChangedEventHandler ThumbStickRightPressed;
        public event ChangedEventHandler ThumbStickRightReleased;

        // Set up DPad events
        public event ChangedEventHandler DPadUpPressed;
        public event ChangedEventHandler DPadUpReleased;

        public event ChangedEventHandler DPadDownPressed;
        public event ChangedEventHandler DPadDownReleased;

        public event ChangedEventHandler DPadLeftPressed;
        public event ChangedEventHandler DPadLeftReleased;

        public event ChangedEventHandler DPadRightPressed;
        public event ChangedEventHandler DPadRightReleased;


        // Set up Trigger events
        public event ChangedEventHandler TriggerLeft;
        public event ChangedEventHandler TriggerRight;

        // Set up ThumbStick events
        public event ChangedEventHandler ThumbStickLeft;
        public event ChangedEventHandler ThumbStickRight;

        // Map of buttons to their events:
        private XboxButton[] Buttons;

        // Set up Trigger state: Trigger<value, TriggerEvent>
        private XboxTrigger[] Triggers;

        // Set up ThumbStick state: ThumbStick<X, Y, ThumbStickEvent>
        private XboxThumbStick[] ThumbSticks;

        // Time in milliseconds to sleep the thread before checking the state again
        private int SLEEPTIME = 1;

        public XboxController() {

            // Initialize Buttons
            Buttons = new XboxButton[14];
            Buttons[0] = new XboxButton(OnButtonAPressed, OnButtonAReleased);
            Buttons[1] = new XboxButton(OnButtonBPressed, OnButtonBReleased);
            Buttons[2] = new XboxButton(OnButtonXPressed, OnButtonXReleased);
            Buttons[3] = new XboxButton(OnButtonYPressed, OnButtonYReleased);
            Buttons[4] = new XboxButton(OnButtonStartPressed, OnButtonStartReleased);
            Buttons[5] = new XboxButton(OnButtonBackPressed, OnButtonBackReleased);
            Buttons[6] = new XboxButton(OnButtonLeftShoulderPressed, OnButtonLeftShoulderReleased);
            Buttons[7] = new XboxButton(OnButtonRightShoulderPressed, OnButtonRightShoulderReleased);
            Buttons[8] = new XboxButton(OnThumbStickLeftPressed, OnThumbStickLeftReleased);
            Buttons[9] = new XboxButton(OnThumbStickRightPressed, OnThumbStickRightReleased);
            Buttons[10] = new XboxButton(OnDPadUpPressed, OnDPadUpReleased);
            Buttons[11] = new XboxButton(OnDPadDownPressed, OnDPadDownReleased);
            Buttons[12] = new XboxButton(OnDPadLeftPressed, OnDPadLeftReleased);
            Buttons[13] = new XboxButton(OnDPadRightPressed, OnDPadRightReleased);

            // Initialize Triggers
            Triggers = new XboxTrigger[2];
            Triggers[0] = new XboxTrigger(OnTriggerLeftChanged);
            Triggers[1] = new XboxTrigger(OnTriggerRightChanged);

            // Initialize ThumbSticks
            ThumbSticks = new XboxThumbStick[2];
            ThumbSticks[0] = new XboxThumbStick(OnThumbStickLeftChanged);
            ThumbSticks[1] = new XboxThumbStick(OnThumbStickRightChanged);

            Thread t = new Thread(x => { while (true) Listen(); });
            t.Start();
        }

        /// <summary>
        /// Takes the state of the xbox controller and fills each out array with the appropriate
        /// state values.
        /// </summary>
        private void GetAllStates(GamePadState state, out ButtonState[] buttons, out float[] triggers, out Vector2[] thumbsticks) {
            // Fill buttons
            ButtonState[] b = new ButtonState[14];
            b[0] = state.Buttons.A;
            b[1] = state.Buttons.B;
            b[2] = state.Buttons.X;
            b[3] = state.Buttons.Y;
            b[4] = state.Buttons.Start;
            b[5] = state.Buttons.Back;
            b[6] = state.Buttons.LeftShoulder;
            b[7] = state.Buttons.RightShoulder;
            b[8] = state.Buttons.LeftStick;
            b[9] = state.Buttons.RightStick;
            b[10] = state.DPad.Up;
            b[11] = state.DPad.Down;
            b[12] = state.DPad.Left;
            b[13] = state.DPad.Right;

            // Fill triggers
            float[] trigs = new float[2];
            trigs[0] = state.Triggers.Left;
            trigs[1] = state.Triggers.Right;

            // Fill thumbsticks
            Vector2[] thumbs = new Vector2[2];
            thumbs[0] = new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            thumbs[1] = new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);

            // Set each out parameter
            buttons = b;
            triggers = trigs;
            thumbsticks = thumbs;
        }

        /// <summary>
        /// Performs the checks for each of the different parts of the Xbox controller
        /// and triggers the specific event if there has been a change.
        /// </summary>
        private void Listen() {
            GamePadState state = GamePad.GetState(PlayerIndex.One);

            // Initialize the different state arrays
            ButtonState[] buttons;
            float[] triggers;
            Vector2[] thumbsticks;

            // Fill the arrays
            GetAllStates(state, out buttons, out triggers, out thumbsticks);

            // Check buttons
            for (int i = 0; i < buttons.Length; i++) {
                if (CheckButtonPressed(buttons[i], Buttons[i]))
                    Buttons[i].OnPressed(new XboxEventArgs(state));
                if (CheckButtonReleased(buttons[i], Buttons[i]))
                    Buttons[i].OnReleased(new XboxEventArgs(state));
            }

            // Check triggers
            for (int i = 0; i < triggers.Length; i++) {
                if (CheckTrigger(triggers[i], Triggers[i]))
                    Triggers[i].OnChanged(new XboxEventArgs(state));
            }

            // Check thumbsticks
            for (int i = 0; i < thumbsticks.Length; i++) {
                if (CheckThumbStick(thumbsticks[i], ThumbSticks[i]))
                    ThumbSticks[i].OnChanged(new XboxEventArgs(state));
            }

            // Sleep the thead, making it wait before checking again
            Thread.Sleep(SLEEPTIME);
        }


        /// <summary>
        /// Checks if the button has been pressed, returning true if it has, false otherwise.
        /// </summary>
        private bool CheckButtonPressed(ButtonState state, XboxButton button) {
            if (button == null)
                return false;

            if (state == ButtonState.Pressed && button.State == ButtonState.Released) {
                //button = new Tuple<ButtonState, ChangedEventHandler, ChangedEventHandler>(state, button.Item2, button.Item3);
                button.State = state;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Checks if the button has been released, returning true if it has, false otherwise.
        /// </summary>
        private bool CheckButtonReleased(ButtonState state, XboxButton button) {
            if (button == null)
                return false;

            if (state == ButtonState.Released && button.State == ButtonState.Pressed) {
                //button = new Tuple<ButtonState, ChangedEventHandler, ChangedEventHandler>(state, button.Item2, button.Item3);
                button.State = state;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Checks if the trigger has changed, returning true if it has, false otherwise.
        /// </summary>
        private bool CheckTrigger(float trigger, XboxTrigger Trigger) {
            if (Trigger == null)
                return false;

            if (trigger != Trigger.State) {
                Trigger.State = trigger;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the thumbstick has changed, returning true if it has, false otherwise.
        /// </summary>
        private bool CheckThumbStick(Vector2 stick, XboxThumbStick ThumbStick) {
            if (ThumbStick == null)
                return false;

            if (stick.X != ThumbStick.Vector.X) {
                ThumbStick.Vector = new Vector2(stick.X, stick.Y);
                return true;
            }
            else if (stick.Y != ThumbStick.Vector.Y) {
                ThumbStick.Vector = new Vector2(stick.X, stick.Y);
                return true;
            }
            return false;
        }

        /*
         * Set up the event triggers
         */

        /************************* BUTTONS *******************/

        // A

        protected virtual void OnButtonAPressed(EventArgs e) {
            if (ButtonAPressed != null)
                ButtonAPressed(this, e);
        }
        protected virtual void OnButtonAReleased(EventArgs e) {
            if (ButtonAReleased != null)
                ButtonAReleased(this, e);
        }

        // B

        protected virtual void OnButtonBPressed(EventArgs e) {
            if (ButtonBPressed != null)
                ButtonBPressed(this, e);
        }
        protected virtual void OnButtonBReleased(EventArgs e) {
            if (ButtonBReleased != null)
                ButtonBReleased(this, e);
        }

        // X

        protected virtual void OnButtonXPressed(EventArgs e) {
            if (ButtonXPressed != null)
                ButtonXPressed(this, e);
        }
        protected virtual void OnButtonXReleased(EventArgs e) {
            if (ButtonXReleased != null)
                ButtonXReleased(this, e);
        }

        // Y

        protected virtual void OnButtonYPressed(EventArgs e) {
            if (ButtonYPressed != null)
                ButtonYPressed(this, e);
        }
        protected virtual void OnButtonYReleased(EventArgs e) {
            if (ButtonYReleased != null)
                ButtonYReleased(this, e);
        }

        // Start

        protected virtual void OnButtonStartPressed(EventArgs e) {
            if (ButtonStartPressed != null)
                ButtonStartPressed(this, e);
        }
        protected virtual void OnButtonStartReleased(EventArgs e) {
            if (ButtonStartReleased != null)
                ButtonStartReleased(this, e);
        }

        // Back

        protected virtual void OnButtonBackPressed(EventArgs e) {
            if (ButtonBackPressed != null)
                ButtonBackPressed(this, e);
        }
        protected virtual void OnButtonBackReleased(EventArgs e) {
            if (ButtonBackReleased != null)
                ButtonBackReleased(this, e);
        }

        // Left Shoulder

        protected virtual void OnButtonLeftShoulderPressed(EventArgs e) {
            if (ButtonLeftShoulderPressed != null)
                ButtonLeftShoulderPressed(this, e);
        }
        protected virtual void OnButtonLeftShoulderReleased(EventArgs e) {
            if (ButtonLeftShoulderReleased != null)
                ButtonLeftShoulderReleased(this, e);
        }

        // Right Shoulder

        protected virtual void OnButtonRightShoulderPressed(EventArgs e) {
            if (ButtonRightShoulderPressed != null)
                ButtonRightShoulderPressed(this, e);
        }
        protected virtual void OnButtonRightShoulderReleased(EventArgs e) {
            if (ButtonRightShoulderReleased != null)
                ButtonRightShoulderReleased(this, e);
        }

        // Left ThumbStick Button

        protected virtual void OnThumbStickLeftPressed(EventArgs e) {
            if (ThumbStickLeftPressed != null)
                ThumbStickLeftPressed(this, e);
        }
        protected virtual void OnThumbStickLeftReleased(EventArgs e) {
            if (ThumbStickLeftReleased != null)
                ThumbStickLeftReleased(this, e);
        }

        // Right ThumbStick Button

        protected virtual void OnThumbStickRightPressed(EventArgs e) {
            if (ThumbStickRightPressed != null)
                ThumbStickRightPressed(this, e);
        }
        protected virtual void OnThumbStickRightReleased(EventArgs e) {
            if (ThumbStickRightReleased != null)
                ThumbStickRightReleased(this, e);
        }


        /*************************** DPAD ****************************/


        // DPad Up

        protected virtual void OnDPadUpPressed(EventArgs e) {
            if (DPadUpPressed != null)
                DPadUpPressed(this, e);
        }
        protected virtual void OnDPadUpReleased(EventArgs e) {
            if (DPadUpReleased != null)
                DPadUpReleased(this, e);
        }

        // DPad Down

        protected virtual void OnDPadDownPressed(EventArgs e) {
            if (DPadDownPressed != null)
                DPadDownPressed(this, e);
        }
        protected virtual void OnDPadDownReleased(EventArgs e) {
            if (DPadDownReleased != null)
                DPadDownReleased(this, e);
        }

        // DPad Left

        protected virtual void OnDPadLeftPressed(EventArgs e) {
            if (DPadLeftPressed != null)
                DPadLeftPressed(this, e);
        }
        protected virtual void OnDPadLeftReleased(EventArgs e) {
            if (DPadLeftReleased != null)
                DPadLeftReleased(this, e);
        }

        // DPad Right

        protected virtual void OnDPadRightPressed(EventArgs e) {
            if (DPadRightPressed != null)
                DPadRightPressed(this, e);
        }
        protected virtual void OnDPadRightReleased(EventArgs e) {
            if (DPadRightReleased != null)
                DPadRightReleased(this, e);
        }


        /************************** TRIGGERS ************************/


        // Left Trigger

        protected virtual void OnTriggerLeftChanged(EventArgs e) {
            if (TriggerLeft != null)
                TriggerLeft(this, e);
        }

        // Right Trigger

        protected virtual void OnTriggerRightChanged(EventArgs e) {
            if (TriggerRight != null)
                TriggerRight(this, e);
        }


        /***************************** THUMBSTICKS ****************************/

        // Left ThumbStick

        protected virtual void OnThumbStickLeftChanged(EventArgs e) {
            if (ThumbStickLeft != null)
                ThumbStickLeft(this, e);
        }

        // Right ThumbStick

        protected virtual void OnThumbStickRightChanged(EventArgs e) {
            if (ThumbStickRight != null)
                ThumbStickRight(this, e);
        }
    }

    class XboxButton {
        public ButtonState State { get; set; }
        public Action<EventArgs> OnPressed { get; set; }
        public Action<EventArgs> OnReleased { get; set; }

        public XboxButton(Action<EventArgs> pressed, Action<EventArgs> released) {
            State = ButtonState.Released;
            OnPressed = pressed;
            OnReleased = released;
        }
    }

    class XboxTrigger {
        public float State { get; set; }
        public Action<EventArgs> OnChanged { get; set; }

        public XboxTrigger(Action<EventArgs> changed) {
            State = 0.0f;
            OnChanged = changed;
        }
    }

    class XboxThumbStick {
        public Vector2 Vector { get; set; }
        public Action<EventArgs> OnChanged { get; set; }

        public XboxThumbStick(Action<EventArgs> changed) {
            Vector = new Vector2(0.0f, 0.0f);
            OnChanged = changed;
        }
    }


    /// <summary>
    /// Class that is used to get the state of the Xbox controller, also returns
    /// values (how far is the trigger pulled, what is the vector of where the
    /// thumbstick is, etc) for solutions that don't have access to the necessary
    /// assemblies and references.
    /// </summary>
    public class XboxEventArgs : EventArgs {

        private GamePadState State;

        public XboxEventArgs(GamePadState state) {
            State = state;
        }

        /// <summary>
        /// Returns the state of the GamePad.
        /// </summary>
        public GamePadState GetState() {
            return State;
        }

        /// <summary>
        /// Returns the left trigger float of how far it is being pulled.
        /// </summary>
        public float GetLeftTrigger() {
            return State.Triggers.Left;
        }

        /// <summary>
        /// Returns the right trigger float of how far it is being pulled.
        /// </summary>
        public float GetRightTrigger() {
            return State.Triggers.Right;
        }

        /// <summary>
        /// Returns the vector as a tuple of where the left thumbstick is located.
        /// </summary>
        public Tuple<float, float> GetLeftThumbStick() {
            return new Tuple<float, float>(State.ThumbSticks.Left.X, State.ThumbSticks.Left.Y);
        }

        /// <summary>
        /// Returns the vector as a tuple of where the right thumbstick is located.
        /// </summary>
        public Tuple<float, float> GetRightThumbStick() {
            return new Tuple<float, float>(State.ThumbSticks.Right.X, State.ThumbSticks.Right.Y);
        }
    }
}