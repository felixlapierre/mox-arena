using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MainGame.Screens
{
    class TradeScreen : Screen
    {
        #region Properties

        ContentManager Content { get; set; }

        bool saveGameOverrideEnabled;
        List<ItemBox> tradeItemBoxes;
        List<StaticEntity> tradeItemBoxBackgrounds;
        StaticEntity resetButton;
        StaticEntity saveButton;
        HealthBar playerTradeMenuHealth;
        StaticEntity healingButton;
        bool playerIsHealing;

        Texture2D actionBarBackground;
        Texture2D healthBarSprite;

        Player player1;

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

        string hoveredItemName = "";
        string hoveredItemType = "";
        string hoveredItemDamage = "";
        string hoveredItemCooldown = "";
        string hoveredItemEffects = "";
        string hoveredItemTooltip = "";
        #endregion

        #region Constructors
        public TradeScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
            Content = content;
            saveGameOverrideEnabled = false;
            tradeItemBoxes = new List<ItemBox>();
            tradeItemBoxBackgrounds = new List<StaticEntity>();

            actionBarBackground = Content.Load<Texture2D>("graphics/actionBarBackground");

            //TODO: Remove
            int TileSize = GameConstants.TILE_SIZE;

            healthBarSprite = Content.Load<Texture2D>("graphics/HealthBar2");
            playerTradeMenuHealth = new HealthBar("Trade Menu Health", new Vector2(TileSize, GameConstants.TILE_SIZE - (TileSize * 3 + 15)), healthBarSprite);

            Vector2 firstBoxLocation = new Vector2(TileSize / 2 + TileSize * 1, TileSize * 3);
            Vector2 secondBoxLocation = new Vector2(TileSize / 2 + TileSize * 3, TileSize * 3);
            Vector2 thirdBoxLocation = new Vector2(TileSize / 2 + TileSize * 5, TileSize * 3);
            Vector2 fourthBoxLocation = new Vector2(TileSize / 2 + TileSize * 7, TileSize * 3);
            Vector2 fifthBoxLocation = new Vector2(TileSize / 2 + TileSize * 9, TileSize * 3);

            equippedItemBoxBackgrounds = new List<StaticEntity>();
            equippedItemBoxes = new List<ItemBox>();

            int equipBoxX = (int)(TileSize * 1.5);
            int equipBoxY = (int)(GameConstants.WINDOW_HEIGHT - TileSize * 2.5);

            Vector2 box1Location = new Vector2(equipBoxX, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon1", box1Location, weapon1.Sprite, weapon1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon1", box1Location, actionBarBackground));

            Vector2 box2Location = new Vector2(equipBoxX + TileSize * 1, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Weapon2", box2Location, weapon2.Sprite, weapon2));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Weapon2", box2Location, actionBarBackground));

            Vector2 box3Location = new Vector2(equipBoxX + TileSize * 2, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Shield1", box3Location, shield1.Sprite, shield1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Shield1", box3Location, actionBarBackground));

            Vector2 box4Location = new Vector2(equipBoxX + TileSize * 3, equipBoxY);
            equippedItemBoxes.Add(new ItemBox("Charm1", box4Location, charm1.Sprite, charm1));
            equippedItemBoxBackgrounds.Add(new StaticEntity("Charm1", box4Location, actionBarBackground));

            tradeItemBoxes.Add(new ItemBox("item1", firstBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            tradeItemBoxes.Add(new ItemBox("item2", secondBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            tradeItemBoxes.Add(new ItemBox("item3", thirdBoxLocation, actionBarBackground, weaponFactory.CreateSword()));
            healingButton = new StaticEntity("Healing", fourthBoxLocation, Content.Load<Texture2D>("graphics/Potions"));
            resetButton = new StaticEntity("Reset Button", fifthBoxLocation, Content.Load<Texture2D>("graphics/resetButton"));
            Texture2D saveButtonSprite = Content.Load<Texture2D>("graphics/saveButton");
            saveButton = new StaticEntity("Save Button", new Vector2(GameConstants.WINDOW_WIDTH - TileSize * 3, TileSize * 2), saveButtonSprite);

            tradeItemBoxBackgrounds.Add(new StaticEntity("item1Background", firstBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item2Background", secondBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item3Background", thirdBoxLocation, actionBarBackground));
            tradeItemBoxBackgrounds.Add(new StaticEntity("item4Background", fourthBoxLocation, actionBarBackground));
            playerIsHealing = false;
        }
        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
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
                gameState = GameState.SaveGame;
                fileSelected = -1;
                for (int i = 0; i < NumberOfSaves; i++)
                    CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);
            }
            DoItemBoxUpdate(mouse, tradeItemBoxes, tradeItemBoxBackgrounds, equippedItemBoxes);

            //playerTradeMenuHealth.Update(gameTime, player1); Health bar does not need to update as it is not moving
            if (playerIsHealing)
            {
                Health = oldHealth + player1.MaxHitPoints / 2;
                if (Health > player1.MaxHitPoints)
                    Health = player1.MaxHitPoints;
            }
            else
                Health = oldHealth;

            player1.HitPoints = Health;
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

        public void ItemBoxHovered(ItemBox itemBox, MouseState mouse)
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
