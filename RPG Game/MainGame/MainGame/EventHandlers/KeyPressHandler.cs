using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.ButtonManager
{
    public delegate void OnKeyPress();

    class KeyPressHandler
    {
        private Keys key;
        private OnKeyPress onKeyPress;
        private bool keyPreviouslyPressed;


        public KeyPressHandler(Keys key, OnKeyPress onKeyPress)
        {
            this.key = key;
            this.onKeyPress = onKeyPress;
            keyPreviouslyPressed = false;
        }

        public void Update()// this is the function that calls the delegate
        {
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(key)&& !keyPreviouslyPressed)
            {
                    onKeyPress.Invoke(); 
            }

            keyPreviouslyPressed = keyboard.IsKeyDown(key);
        }
    }

}
