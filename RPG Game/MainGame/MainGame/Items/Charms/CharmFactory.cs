using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MainGame.ContentLoaders.Textures;

namespace MainGame.Items.Charms
{
    public class CharmFactory
    {
        #region Properties
        CharmLoader charmLoader;
        Texture2D charmSprite;
        #endregion

        #region Constructors
        public CharmFactory()
        {
            charmLoader = CharmLoader.GetInstance();
            charmSprite = charmLoader.Get("basic");
        }
        #endregion

        public Charm CreateEmptyCharm()
        {
            Charm charm = new Charm("Empty Charm", CharmType.Blank, 0f, charmSprite, Color.White);
            charm.ID = 0;
            charm.Tooltip = "This charm does nothing.";
            return charm;
        }

        public Charm CreateLowerCooldown()
        {
            Charm charm = new Charm("Overclock Charm", CharmType.LowerCooldown, 2f, charmSprite, Color.Orange);
            charm.ID = 1;
            charm.Tooltip = "Your weapons' cooldowns are half as long, but they deal half as much damage.";
            return charm;
        }

        public Charm CreateHigherCooldown()
        {
            Charm charm = new Charm("Impact Charm", CharmType.HigherCooldown, 2f, charmSprite, Color.Crimson);
            charm.ID = 2;
            charm.Tooltip = "Your weapons deal twice as much damage, but their cooldown is twice as long.";
            return charm;
        }

        public Charm CreateBurstCharm()
        {
            Charm charm = new Charm("Burst Charm", CharmType.Burst, 1.0f, charmSprite, Color.Blue);
            charm.ID = 3;
            charm.Tooltip = "Your ranged weapons have 1 extra burst.";
            return charm;
        }

        public Charm CreateSpeedCharm()
        {
            Charm charm = new Charm("Speed Charm", CharmType.Speed, 1.3f, charmSprite, Color.LimeGreen);
            charm.ID = 4;
            charm.Tooltip = "Your speed is increased by 30%.";
            return charm;
        }

        public Charm CreateLifestealCharm()
        {
            Charm charm = new Charm("Vampiric Charm", CharmType.Lifesteal, 0.4f, charmSprite, Color.DarkViolet);
            charm.ID = 5;
            charm.Tooltip = "Your melee attacks heal you for " + (charm.Strength * 100).ToString() + "% of damage dealt. You cannot take the Heal option in the Trade menu.";
            return charm;
        }

    }

}
