using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.ButtonManager
{
    public delegate void OnClick();
    class MouseClickHandler
    {
        private ButtonState button;
        private OnClick onClick;
        private bool buttonPreviouslyPressed;

        public MouseClickHandler(ButtonState button, OnClick onClick)
        {
            this.button = button;
            this.onClick = onClick;
            buttonPreviouslyPressed = false;
        }
        
        // TODO find way to get rid of parameter in update method
        public void Update(ButtonState button) 
        {
            MouseState mouse = Mouse.GetState();
            if ((button== ButtonState.Pressed) && !buttonPreviouslyPressed)
            {
                    onClick.Invoke();
            }
            buttonPreviouslyPressed = true;
        }
    }

}
