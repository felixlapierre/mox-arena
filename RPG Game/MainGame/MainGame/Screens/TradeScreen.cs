using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainGame.Items;
using MainGame.Screens.Trade_Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MainGame.ContentLoaders;
using MainGame.ContentLoaders.Textures;

namespace MainGame.Screens
{
    class TradeScreen : Screen
    {
        #region Properties
        bool saveGameOverrideEnabled;
        List<ItemBox> tradeItemBoxes;
        List<StaticEntity> tradeItemBoxBackgrounds;
        StaticEntity resetButton;
        StaticEntity saveButton;
        HealthBar playerTradeMenuHealth;
        StaticEntity healingButton;
        bool playerIsHealing;

        StaticEntity background;
        Texture2D actionBarBackground;
        Texture2D healthBarSprite;

        Weapon weapon1;
        Weapon weapon2;
        Shield shield1;
        Charm charm1;

        Weapon oldWeapon1;
        Weapon oldWeapon2;
        Shield oldShield1;
        Charm oldCharm1;

        List<StaticEntity> equippedItemBoxBackgrounds;
        List<ItemBox> equippedItemBoxes;

        float Health;
        float oldHealth;

        int level;

        string hoveredItemName = "";
        string hoveredItemType = "";
        string hoveredItemDamage = "";
        string hoveredItemCooldown = "";
        string hoveredItemEffects = "";
        string hoveredItemTooltip = "";

        TradeScreenContents TradeContents { get; set; }

        SpriteFont font;

        Texture2D playButtonSprite;
        StaticEntity playButton;

        //TODO: Change code to remove this one buttons are refactored
        bool leftMousePreviouslyPressed = false;
        #endregion

        #region Constructors
        public TradeScreen(OnScreenChanged screenChanged, TradeScreenContents tradeContents) : base(screenChanged)
        {
            UserInterfaceLoader uiLoader = UserInterfaceLoader.GetInstance();
            FontLoader fontLoader = FontLoader.GetInstance();
            CreatureLoader creatureLoader = CreatureLoader.GetInstance();

            TradeContents = tradeContents;
            saveGameOverrideEnabled = false;
            tradeItemBoxes = new List<ItemBox>();
            tradeItemBoxBackgrounds = new List<StaticEntity>();

            level = tradeContents.Level;
            Health = tradeContents.Health;
            oldHealth = Health;
            weapon1 = tradeContents.Weapon1;
            weapon2 = tradeContents.Weapon2;
            shield1 = tradeContents.Shield1;
            charm1 = tradeContents.Charm1;

            oldWeapon1 = weapon1;
            oldWeapon2 = weapon2;
            oldShield1 = shield1;
            oldCharm1 = charm1;

            actionBarBackground = uiLoader.Get("blankBackground");
            background = new StaticEntity("Background", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2), actionBarBackground);
            playButton = new StaticEntity("Play Button", new Vector2(GameConstants.WINDOW_WIDTH - GameConstants.TILE_SIZE * 5 / 2, GameConstants.WINDOW_HEIGHT - GameConstants.TILE_SIZE), uiLoader.Get("continue"));

            //TODO: Remove

            font = fontLoader.Get("font");

            healthBarSprite = creatureLoader.Get("healthBar1");
            playerTradeMenuHealth = new HealthBar("Trade Menu Health", new Vector2(GameConstants.TILE_SIZE, GameConstants.WINDOW_HEIGHT - (GameConstants.TILE_SIZE * 3 + 15)), healthBarSprite);

            Vector2 firstBoxLocation = new Vector2(GameConstants.TILE_SIZE / 2 + GameConstants.TILE_SIZE * 1, GameConstants.TILE_SIZE * 3);
            Vector2 secondBoxLocation = new Vector2(GameConstants.TILE_SIZE / 2 + GameConstants.TILE_SIZE * 3, GameConstants.TILE_SIZE * 3);
            Vector2 thirdBoxLocation = new Vector2(GameConstants.TILE_SIZE / 2 + GameConstants.TILE_SIZE * 5, GameConstants.TILE_SIZE * 3);
            Vector2 fourthBoxLocation = new Vector2(GameConstants.TILE_SIZE / 2 + GameConstants.TILE_SIZE * 7, GameConstants.TILE_SIZE * 3);
            Vector2 fifthBoxLocation = new Vector2(GameConstants.TILE_SIZE / 2 + GameConstants.TILE_SIZE * 9, GameConstants.TILE_SIZE * 3);

            equippedItemBoxBackgrounds = new List<StaticEntity>();
            equippedItemBoxes = new List<ItemBox>();

            int equipBoxX = (int)(GameConstants.TILE_SIZE * 1.5);
            int equipBoxY = (int)(GameConstants.WINDOW_HEIGHT - GameConstants.TILE_SIZE * 2.5);

            Vector2 box1Location = new Vector2(equipBoxX, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon1", box1Location, weapon1.Sprite, weapon1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon1", box1Location, actionBarBackground));

            Vector2 box2Location = new Vector2(equipBoxX + GameConstants.TILE_SIZE * 1, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon2", box2Location, weapon2.Sprite, weapon2));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon2", box2Location, actionBarBackground));

            Vector2 box3Location = new Vector2(equipBoxX + GameConstants.TILE_SIZE * 2, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Shield1", box3Location, shield1.Sprite, shield1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Shield1", box3Location, actionBarBackground));

            Vector2 box4Location = new Vector2(equipBoxX + GameConstants.TILE_SIZE * 3, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Charm1", box4Location, charm1.Sprite, charm1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Charm1", box4Location, actionBarBackground));

            //TODO: Rewrite once items inheritance is sorted out
            List<Item> items = new List<Item>();
            items.Add(tradeContents.Item1);
            items.Add(tradeContents.Item2);
            items.Add(tradeContents.Item3);

            for (int i = 0; i < items.Count; i++)
            {
                Item item = items.ElementAt(i);
                Vector2 location;
                switch(i)
                {
                    //Default case or there will be warnings
                    default:
                        location = firstBoxLocation;
                        break;
                    case 1:
                        location = secondBoxLocation;
                        break;
                    case 2:
                        location = thirdBoxLocation;
                        break;
                }
                //TODO: Fix this spaghetti
                //The constructor of ItemBox is misused, don't but actionBarBackground as the sprite.
                if (item is Weapon)
                {
                    tradeItemBoxes.Add(new ItemBox("item", location, ((Weapon)item).Sprite, (Weapon)item));
                }
                else if (item is Shield)
                {
                    tradeItemBoxes.Add(new ItemBox("item", location, ((Shield)item).Sprite, (Shield)item));
                }
                else if (item is Charm)
                {
                    tradeItemBoxes.Add(new ItemBox("item", location, ((Charm)item).Sprite, (Charm)item));
                }
            }

            //tradeItemBoxes.Add(new ItemBox("item1", firstBoxLocation, actionBarBackground, tradeContents.Item1));
            //tradeItemBoxes.Add(new ItemBox("item2", secondBoxLocation, actionBarBackground, tradeContents.Item2));
            //tradeItemBoxes.Add(new ItemBox("item3", thirdBoxLocation, actionBarBackground, tradeContents.Item3));
            healingButton = new StaticEntity("Healing", fourthBoxLocation, uiLoader.Get("potions"));
            resetButton = new StaticEntity("Reset Button", fifthBoxLocation, uiLoader.Get("reset"));
            saveButton = new StaticEntity("Save Button", new Vector2(GameConstants.WINDOW_WIDTH - GameConstants.TILE_SIZE * 3, GameConstants.TILE_SIZE * 2), uiLoader.Get("saveGame"));

            tradeItemBoxBackgrounds.Add(new StaticEntity("item1Background", firstBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item2Background", secondBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item3Background", thirdBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item4Background", fourthBoxLocation, actionBarBackground));
            playerIsHealing = false;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();

            background.Draw(spriteBatch, new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT));

            #region Item Boxes
            foreach (StaticEntity equipItemBoxBackground in equippedItemBoxBackgrounds)
                equipItemBoxBackground.Draw(spriteBatch, Color.DimGray);

            foreach (ItemBox itemBox in equippedItemBoxes)
            {
                if (itemBox.Sprite != null)
                {
                    if (itemBox.IsHovered)
                        itemBox.Draw(spriteBatch, Color.LightSteelBlue);
                    else
                        itemBox.Draw(spriteBatch);
                }
            }
            #endregion

            #region Tooltip text
            if (hoveredItemName != "")
            {
                spriteBatch.DrawString(font, hoveredItemName, new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7), Color.Black);

                spriteBatch.DrawString(font, hoveredItemType, new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7 + font.LineSpacing), Color.Black);

                if (hoveredItemDamage != "")
                    spriteBatch.DrawString(font, "Damage: " + hoveredItemDamage, new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7 + font.LineSpacing * 2), Color.Black);

                spriteBatch.DrawString(font, "Effects: " + hoveredItemEffects, new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7 + font.LineSpacing * 3), Color.Black);
                if (hoveredItemCooldown != "")
                    spriteBatch.DrawString(font, "Cooldown: " + hoveredItemCooldown + " second(s)", new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7 + font.LineSpacing * 4), Color.Black);
                spriteBatch.DrawString(font, hoveredItemTooltip, new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE * 7 + font.LineSpacing * 5), Color.Black);
            }
            #endregion

            if (playButton.CollisionRectangle.Contains(mouse.Position))
                playButton.Draw(spriteBatch, Color.LightSteelBlue);
            else
                playButton.Draw(spriteBatch);

            #region Text
            spriteBatch.DrawString(font, "The arena allows you to choose one item to take from your defeated foes.", new Vector2(GameConstants.TILE_SIZE / 2, GameConstants.TILE_SIZE / 2), Color.Black);
            spriteBatch.DrawString(font, "If you are injured, you may choose to take a healing potion instead.", new Vector2(GameConstants.TILE_SIZE / 2, GameConstants.TILE_SIZE / 2 + font.LineSpacing), Color.Black);
            spriteBatch.DrawString(font, "Left click on a piece of equipment to select it. Right click if you'd like to equip a weapon in slot 2.", new Vector2(GameConstants.TILE_SIZE / 2, GameConstants.TILE_SIZE / 2 + font.LineSpacing * 3), Color.Black);
            Color healTextColor = Color.Black;
            if (oldCharm1.Type == CharmType.Lifesteal)
                healTextColor = Color.Crimson;
            spriteBatch.DrawString(font, "Heal", new Vector2(GameConstants.TILE_SIZE * 7 + (GameConstants.TILE_SIZE - font.MeasureString("Heal").X) / 2, GameConstants.TILE_SIZE * 3 - (GameConstants.TILE_SIZE / 2 + font.LineSpacing)), healTextColor);
            spriteBatch.DrawString(font, "Undo", new Vector2(GameConstants.TILE_SIZE * 9 + (GameConstants.TILE_SIZE - font.MeasureString("Undo").X) / 2, GameConstants.TILE_SIZE * 3 - (GameConstants.TILE_SIZE / 2 + font.LineSpacing)), Color.Black);
            spriteBatch.DrawString(font, "Level: " + level.ToString(), new Vector2(GameConstants.WINDOW_WIDTH - font.MeasureString("Level:     ").X, 0), Color.Black);

            #endregion

            #region Item boxes
            foreach (StaticEntity itemBoxBackground in tradeItemBoxBackgrounds)
                itemBoxBackground.Draw(spriteBatch, Color.DimGray);

            foreach (ItemBox itemBox in tradeItemBoxes)
            {
                if (itemBox.Sprite != null && itemBox.IsActive)
                {
                    if (itemBox.IsHovered)
                        itemBox.Draw(spriteBatch, Color.LightSteelBlue);
                    else
                        itemBox.Draw(spriteBatch);
                }
            }

            #endregion

            #region Buttons

            if (resetButton.CollisionRectangle.Contains(mouse.Position))
                resetButton.Draw(spriteBatch, Color.LightSteelBlue);
            else
                resetButton.Draw(spriteBatch);

            if (oldCharm1.Type == CharmType.Lifesteal)
                healingButton.Draw(spriteBatch, Color.DarkGray);
            else if (healingButton.CollisionRectangle.Contains(mouse.Position))
                healingButton.Draw(spriteBatch, Color.LightSteelBlue);
            else
                healingButton.Draw(spriteBatch);
            if ((level - 1) % 10 == 0 || saveGameOverrideEnabled)
            {
                if (saveButton.CollisionRectangle.Contains(mouse.Position))
                    saveButton.Draw(spriteBatch, Color.LightSteelBlue);
                else
                    saveButton.Draw(spriteBatch);
            }
            playerTradeMenuHealth.Draw(spriteBatch, (int)Health, GameConstants.PLAYER_MAX_HIT_POINTS, new Vector2(0, 0));
            #endregion
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();
            MouseState mouse = Mouse.GetState();

            if (keyboard.IsKeyDown(Keys.Home))
                saveGameOverrideEnabled = true;
            if (resetButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                weapon1 = oldWeapon1;
                weapon2 = oldWeapon2;
                shield1 = oldShield1;
                charm1 = oldCharm1;
                playerIsHealing = false;
            }
            if (healingButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed && oldCharm1.Type != CharmType.Lifesteal)
            {
                weapon1 = oldWeapon1;
                weapon2 = oldWeapon2;
                shield1 = oldShield1;
                charm1 = oldCharm1;
                playerIsHealing = true;
            }
            if (((level - 1) % 10 == 0 || saveGameOverrideEnabled) && saveButton.CollisionRectangle.Contains(mouse.Position)
                && mouse.LeftButton == ButtonState.Pressed)
            {
                ScreenChanged(new SaveGameScreen(ScreenChanged, TradeContents));
                //fileSelected = -1;
                //for (int i = 0; i < NumberOfSaves; i++)
                //    CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);
            }
            DoItemBoxUpdate(mouse, tradeItemBoxes, tradeItemBoxBackgrounds, equippedItemBoxes);

            //playerTradeMenuHealth.Update(gameTime, player1); Health bar does not need to update as it is not moving
            if (playerIsHealing)
            {
                Health = oldHealth + GameConstants.PLAYER_MAX_HIT_POINTS / 2;
                if (Health > GameConstants.PLAYER_MAX_HIT_POINTS)
                    Health = GameConstants.PLAYER_MAX_HIT_POINTS;
            }
            else
                Health = oldHealth;

            if (playButton.CollisionRectangle.Contains(new Vector2(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            {
                TradeScreenContents tradeContents = new TradeScreenContents(Health, level, weapon1, weapon2, shield1, charm1);
                ScreenChanged(new CombatScreen(ScreenChanged, tradeContents));
            }
            leftMousePreviouslyPressed = mouse.LeftButton == ButtonState.Pressed;
        }

        private void DoItemBoxUpdate(MouseState mouse, List<ItemBox> itemBoxes, List<StaticEntity> itemBoxBackgrounds, List<ItemBox> equippedItemBoxes)
        {
            foreach (StaticEntity itemBoxBackground in itemBoxBackgrounds)
            {
                if (itemBoxBackgrounds.Count == itemBoxes.Count || itemBoxBackgrounds.IndexOf(itemBoxBackground) < itemBoxes.Count)
                {
                    ItemBox correspondingBox = itemBoxes.ElementAt(itemBoxBackgrounds.IndexOf(itemBoxBackground));
                    if (itemBoxBackground.CollisionRectangle.Contains(mouse.Position.X, mouse.Position.Y))
                        correspondingBox.IsHovered = true;
                    else
                        correspondingBox.IsHovered = false;
                }
            }
            foreach (StaticEntity itemBoxBackground in equippedItemBoxBackgrounds)
            {
                ItemBox correspondingBox = equippedItemBoxes.ElementAt(equippedItemBoxBackgrounds.IndexOf(itemBoxBackground));
                if (itemBoxBackground.CollisionRectangle.Contains(mouse.Position.X, mouse.Position.Y))
                    correspondingBox.IsHovered = true;
                else
                    correspondingBox.IsHovered = false;
            }
            hoveredItemName = "";
            hoveredItemType = "";
            hoveredItemEffects = "";
            hoveredItemDamage = "";
            hoveredItemCooldown = "";
            foreach (ItemBox itemBox in itemBoxes)
            {
                if (itemBox.IsHovered)
                {
                    ItemBoxHovered(itemBox, mouse);
                }
            }
            foreach (ItemBox itemBox in equippedItemBoxes)
            {
                if (itemBox.IsHovered)
                    ItemBoxHovered(itemBox, mouse);
            }
            equippedItemBoxes.ElementAt(0).ReplaceItem(weapon1);
            equippedItemBoxes.ElementAt(1).ReplaceItem(weapon2);
            equippedItemBoxes.ElementAt(2).ReplaceItem(shield1);
            equippedItemBoxes.ElementAt(3).ReplaceItem(charm1);
            foreach (ItemBox box in equippedItemBoxes)
                box.Update();
        }

        private void ItemBoxHovered(ItemBox itemBox, MouseState mouse)
        {
            if (itemBox.IsActive == false)
                return;
            //Set the equipped item to the item in the box if the player is clicking on it
            if (itemBox.Type == ItemType.Shield && itemBox.Shield != null)
            {
                Shield shield = itemBox.Shield;
                hoveredItemName = shield.Name;
                hoveredItemType = "Equipment";
                hoveredItemTooltip = shield.Tooltip;
                if (shield.ActiveEffects.Count > 0)
                {
                    foreach (Effect effect in shield.ActiveEffects)
                    {
                        hoveredItemEffects += effect.Type.ToString() + " ";
                        if (effect.Strength > 0 && effect.Type != EffectType.Spin)
                            hoveredItemEffects += effect.Strength.ToString() + " ";
                        hoveredItemEffects += "for " + (effect.Duration / 1000f).ToString() + " sec, ";
                    }
                }
                else
                    hoveredItemEffects = "None";
                hoveredItemCooldown = ((float)shield.MaximumCooldown / 1000f).ToString();


                if (mouse.LeftButton == ButtonState.Pressed || mouse.RightButton == ButtonState.Pressed)
                {
                    shield1 = shield.Copy();
                    if (oldShield1 != null)
                    {
                        weapon1 = oldWeapon1;
                        weapon2 = oldWeapon2;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
            }
            else if (itemBox.Type == ItemType.Weapon && itemBox.Weapon != null)
            {
                Weapon weapon = itemBox.Weapon;
                hoveredItemName = weapon.Name;
                hoveredItemType = weapon.Type.ToString() + " weapon";
                hoveredItemTooltip = weapon.Tooltip;
                if (weapon.Type == WeaponType.Ranged)
                    hoveredItemDamage = weapon.ProjectileDamage.ToString();
                else if (weapon.Type == WeaponType.Melee)
                    hoveredItemDamage = weapon.Damage.ToString();
                if (weapon.OnHitEffects.Count > 0)
                {
                    foreach (Effect effect in weapon.OnHitEffects)
                        hoveredItemEffects += effect.Type.ToString() + " " + effect.Strength.ToString() + " for " + (effect.Duration / 1000f).ToString() + " sec, ";
                }
                else
                    hoveredItemEffects = "None";
                hoveredItemCooldown = ((float)weapon.Cooldown / 1000f).ToString();

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    weapon1 = itemBox.Weapon.Copy();
                    if (oldWeapon1 != null)
                    {
                        weapon2 = oldWeapon2;
                        shield1 = oldShield1;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
                else if (mouse.RightButton == ButtonState.Pressed)
                {
                    weapon2 = itemBox.Weapon.Copy();
                    if (oldWeapon2 != null)
                    {
                        weapon1 = oldWeapon1;
                        shield1 = oldShield1;
                        charm1 = oldCharm1;
                        playerIsHealing = false;
                    }
                }
            }
            else if (itemBox.Type == ItemType.Charm && itemBox.Charm != null)
            {
                Charm charm = itemBox.Charm;
                hoveredItemName = charm.Name;
                hoveredItemType = "Charm";
                hoveredItemDamage = "";
                hoveredItemTooltip = charm.Tooltip;
                hoveredItemEffects = charm.Type.ToString();
                hoveredItemCooldown = "";

                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    weapon1 = oldWeapon1;
                    weapon2 = oldWeapon2;
                    shield1 = oldShield1;
                    playerIsHealing = false;
                    charm1 = itemBox.Charm.Copy();
                }
            }
        }
    }
}
