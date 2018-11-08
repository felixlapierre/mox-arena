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
    public class SandboxScreen : Screen
    {
        #region Properties
        StaticEntity background;
        List<ItemBox> itemBoxes;
        List<ItemBox> equippedItemBoxes;
        List<StaticEntity> itemBoxBackgrounds;
        List<StaticEntity> equippedItemBoxBackgrounds;
        StaticEntity playButton;
        StaticEntity startButton;
        StaticEntity backButton;

        Texture2D playButtonSprite;
        Texture2D startButtonSprite;
        Texture2D backButtonSprite;

        Texture2D actionBarBackground;

        SpriteFont font;
        string hoveredItemName = "";
        string hoveredItemType = "";
        string hoveredItemDamage = "";
        string hoveredItemCooldown = "";
        string hoveredItemEffects = "";
        string hoveredItemTooltip = "";

        MouseState mouse;

        //TODO: Remove these, bad practice
        const int WindowWidth = GameConstants.WINDOW_WIDTH;
        const int WindowHeight = GameConstants.WINDOW_HEIGHT;
        const int TileSize = GameConstants.TILE_SIZE;

        //TODO: Find a way to remove these eventually
        //or otherwise improve
        Weapon weapon1;
        Weapon weapon2;
        Shield shield1;
        Charm charm1;
        WeaponFactory weaponFactory;
        ShieldFactory shieldFactory;
        CharmFactory charmFactory;

        ContentManager Content { get; set; }
        #endregion

        public SandboxScreen(OnScreenChanged screenChanged, ContentManager content, Game1 tempGame1) : base(screenChanged)
        {
            Content = content;
            actionBarBackground = content.Load<Texture2D>("graphics/actionBarBackground");
            font = content.Load<SpriteFont>("font/font");

            background = new StaticEntity("Background", new Vector2(WindowWidth / 2, WindowHeight / 2), actionBarBackground);
            itemBoxes = new List<ItemBox>();
            itemBoxBackgrounds = new List<StaticEntity>();
            equippedItemBoxes = new List<ItemBox>();
            equippedItemBoxBackgrounds = new List<StaticEntity>();

            playButtonSprite = content.Load<Texture2D>("graphics/continueButton");
            startButtonSprite = content.Load<Texture2D>("graphics/startButton");
            backButtonSprite = content.Load<Texture2D>("graphics/backButton");

            playButton = new StaticEntity("Play Button", new Vector2(WindowWidth - TileSize * 5 / 2, WindowHeight - TileSize), playButtonSprite);
            startButton = new StaticEntity("Start Button", new Vector2(WindowWidth / 2, WindowHeight / 2), startButtonSprite);
            backButton = new StaticEntity("Back Button", new Vector2(TileSize * 2, WindowHeight - TileSize), backButtonSprite);

            weaponFactory = tempGame1.weaponFactory;
            shieldFactory = tempGame1.shieldFactory;
            charmFactory = tempGame1.charmFactory;

            weapon1 = weaponFactory.CreateSword();
            weapon2 = weaponFactory.CreateBow();
            shield1 = shieldFactory.CreateBasicShield();
            charm1 = charmFactory.CreateEmptyCharm();

            #region Weapon Item Boxes
            int weaponBoxY = (int)(TileSize * 1.5);
            int weaponBoxX = (int)(TileSize * 1.5);

            Weapon sword = weaponFactory.CreateSword();
            Vector2 itemBoxSwordLocation = new Vector2(weaponBoxX, weaponBoxY);
            itemBoxes.Add(new ItemBox("Sword", itemBoxSwordLocation, sword.Sprite, sword));
            itemBoxBackgrounds.Add(new StaticEntity("SwordBackground", itemBoxSwordLocation, actionBarBackground));

            Weapon broadSword = weaponFactory.CreateBroadsword();
            Vector2 itemBoxBroadswordLocation = new Vector2(weaponBoxX + TileSize, weaponBoxY);
            itemBoxes.Add(new ItemBox("Broadsword", itemBoxBroadswordLocation, broadSword.Sprite, broadSword));
            itemBoxBackgrounds.Add(new StaticEntity("BroadswordBackground", itemBoxBroadswordLocation, actionBarBackground));

            Weapon bow = weaponFactory.CreateBow();
            Vector2 itemBoxBowLocation = new Vector2(weaponBoxX + TileSize * 2, weaponBoxY);
            itemBoxes.Add(new ItemBox("Bow", itemBoxBowLocation, bow.Sprite, bow));
            itemBoxBackgrounds.Add(new StaticEntity("BowBackground", itemBoxBowLocation, actionBarBackground));

            Weapon spear = weaponFactory.CreateSpear();
            Vector2 itemBoxSpearLocation = new Vector2(weaponBoxX + TileSize * 3, weaponBoxY);
            itemBoxes.Add(new ItemBox("Spear", itemBoxSpearLocation, spear.Sprite, spear));
            itemBoxBackgrounds.Add(new StaticEntity("SpearBackground", itemBoxSpearLocation, actionBarBackground));

            Weapon throwingAxe = weaponFactory.CreateThrowingAxe();
            Vector2 itemBoxThrowingaxeLocation = new Vector2(weaponBoxX + TileSize * 4, weaponBoxY);
            itemBoxes.Add(new ItemBox("ThrowingAxe", itemBoxThrowingaxeLocation, throwingAxe.Sprite, throwingAxe));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingAxeBackground", itemBoxThrowingaxeLocation, actionBarBackground));

            Weapon throwingDagger = weaponFactory.CreateThrowingDagger();
            Vector2 itemBoxThrowingDaggerLocation = new Vector2(weaponBoxX + TileSize * 5, weaponBoxY);
            itemBoxes.Add(new ItemBox("Throwing Dagger", itemBoxThrowingDaggerLocation, throwingDagger.Sprite, throwingDagger));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingDaggerBackground", itemBoxThrowingDaggerLocation, actionBarBackground));

            Weapon iceBow = weaponFactory.CreateIceBow();
            Vector2 itemBoxIcebowLocation = new Vector2(weaponBoxX + TileSize * 6, weaponBoxY);
            itemBoxes.Add(new ItemBox("Icebow", itemBoxIcebowLocation, iceBow.Sprite, iceBow));
            itemBoxBackgrounds.Add(new StaticEntity("IcebowBackground", itemBoxIcebowLocation, actionBarBackground));

            Weapon dwarvenAxe = weaponFactory.CreateDwarvenAxe();
            Vector2 itemBoxDwarvenAxeLocation = new Vector2(weaponBoxX + TileSize * 7, weaponBoxY);
            itemBoxes.Add(new ItemBox("DwarvenAxe", itemBoxDwarvenAxeLocation, dwarvenAxe.Sprite, dwarvenAxe));
            itemBoxBackgrounds.Add(new StaticEntity("DwarvenAxeBackground", itemBoxDwarvenAxeLocation, actionBarBackground));

            Weapon maul = weaponFactory.CreateMaul();
            Vector2 itemBoxMaulLocation = new Vector2(weaponBoxX + TileSize * 8, weaponBoxY);
            itemBoxes.Add(new ItemBox("Maul", itemBoxMaulLocation, maul.Sprite, maul));
            itemBoxBackgrounds.Add(new StaticEntity("MaulBackground", itemBoxMaulLocation, actionBarBackground));

            Weapon hammer = weaponFactory.CreateHammer();
            Vector2 itemBoxHammerLocation = new Vector2(weaponBoxX + TileSize * 9, weaponBoxY);
            itemBoxes.Add(new ItemBox("Maul", itemBoxHammerLocation, hammer.Sprite, hammer));
            itemBoxBackgrounds.Add(new StaticEntity("MaulBackground", itemBoxHammerLocation, actionBarBackground));

            Weapon throwingSpear = weaponFactory.CreateThrowingSpear();
            Vector2 itemBoxThrowingSpearLocation = new Vector2(weaponBoxX + TileSize * 10, weaponBoxY);
            itemBoxes.Add(new ItemBox("ThrowingSpear", itemBoxThrowingSpearLocation, throwingSpear.Sprite, throwingSpear));
            itemBoxBackgrounds.Add(new StaticEntity("ThrowingSpearBackground", itemBoxThrowingSpearLocation, actionBarBackground));

            Weapon jungleSpear = weaponFactory.CreateJungleSpear();
            Vector2 itemBoxJungleSpearLocation = new Vector2(weaponBoxX + TileSize * 11, weaponBoxY);
            itemBoxes.Add(new ItemBox("JungleSpear", itemBoxJungleSpearLocation, jungleSpear.Sprite, jungleSpear));
            itemBoxBackgrounds.Add(new StaticEntity("JungleSpearBackground", itemBoxJungleSpearLocation, actionBarBackground));

            Weapon fireball = weaponFactory.CreateFireball();
            Vector2 itemBoxFireballLocation = new Vector2(weaponBoxX + TileSize * 12, weaponBoxY);
            itemBoxes.Add(new ItemBox("Fireball", itemBoxFireballLocation, fireball.Sprite, fireball));
            itemBoxBackgrounds.Add(new StaticEntity("FireballBackground", itemBoxFireballLocation, actionBarBackground));

            Weapon superAxe = weaponFactory.CreateMaverick();
            Vector2 itemBoxSuperAxeLocation = new Vector2(weaponBoxX + TileSize * 13, weaponBoxY);
            itemBoxes.Add(new ItemBox("Superaxe", itemBoxSuperAxeLocation, superAxe.Sprite, superAxe));
            itemBoxBackgrounds.Add(new StaticEntity("SuperaxeBackground", itemBoxSuperAxeLocation, actionBarBackground));

            Weapon firebolt = weaponFactory.CreateFirebolt();
            Vector2 itemBoxFireboltLocation = new Vector2(weaponBoxX + TileSize * 14, weaponBoxY);
            itemBoxes.Add(new ItemBox("Firebolt", itemBoxFireboltLocation, firebolt.Sprite, firebolt));
            itemBoxBackgrounds.Add(new StaticEntity("FireboltBackground", itemBoxFireboltLocation, actionBarBackground));

            Weapon firework = weaponFactory.CreateFirework();
            Vector2 itemBoxFireworkLocation = new Vector2(weaponBoxX + TileSize * 15, weaponBoxY);
            itemBoxes.Add(new ItemBox("Firework", itemBoxFireworkLocation, firework.Sprite, firework));
            itemBoxBackgrounds.Add(new StaticEntity("FireworkBackground", itemBoxFireworkLocation, actionBarBackground));

            Weapon shuriken1 = weaponFactory.CreateBoteng();
            Vector2 itemBoxShuriken1Location = new Vector2(weaponBoxX + TileSize * 16, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken1", itemBoxShuriken1Location, shuriken1.Sprite, shuriken1));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken1Background", itemBoxShuriken1Location, actionBarBackground));

            Weapon shuriken2 = weaponFactory.CreateHira();
            Vector2 itemBoxShuriken2Location = new Vector2(weaponBoxX + TileSize * 17, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken2", itemBoxShuriken2Location, shuriken2.Sprite, shuriken2));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken2Background", itemBoxShuriken2Location, actionBarBackground));

            Weapon shuriken3 = weaponFactory.CreateTaago();
            Vector2 itemBoxShuriken3Location = new Vector2(weaponBoxX + TileSize * 18, weaponBoxY);
            itemBoxes.Add(new ItemBox("Shuriken3", itemBoxShuriken3Location, shuriken3.Sprite, shuriken3));
            itemBoxBackgrounds.Add(new StaticEntity("Shuriken3Background", itemBoxShuriken3Location, actionBarBackground));

            Weapon plasmaBolt = weaponFactory.CreatePlasmaBolt();
            Vector2 itemBoxPlasmaBoltLocation = new Vector2(weaponBoxX + TileSize * 15, weaponBoxY + TileSize);
            itemBoxes.Add(new ItemBox("plasmaBolt", itemBoxPlasmaBoltLocation, plasmaBolt.Sprite, plasmaBolt));
            itemBoxBackgrounds.Add(new StaticEntity("plasmaBoltBackground", itemBoxPlasmaBoltLocation, actionBarBackground));

            Weapon grapple = weaponFactory.CreateGrapple();
            Vector2 itemBoxGrappleLocation = new Vector2(weaponBoxX + TileSize * 16, weaponBoxY + TileSize);
            itemBoxes.Add(new ItemBox("Grapple", itemBoxGrappleLocation, grapple.Sprite, grapple));
            itemBoxBackgrounds.Add(new StaticEntity("grappleBackground", itemBoxGrappleLocation, actionBarBackground));
            #endregion

            #region Shield Item Boxes
            int shieldBoxY = (int)(TileSize * 3.5);
            int shieldBoxX = (int)(TileSize * 1.5);

            Shield basicShield = shieldFactory.CreateBasicShield();
            Vector2 itemBoxBasicShieldLocation = new Vector2(shieldBoxX, shieldBoxY);
            itemBoxes.Add(new ItemBox("BasicShield", itemBoxBasicShieldLocation, basicShield.Sprite, basicShield));
            itemBoxBackgrounds.Add(new StaticEntity("BasicShieldBackground", itemBoxBasicShieldLocation, actionBarBackground));

            Shield speedBoost = shieldFactory.CreateSpeedboost();
            Vector2 itemBoxSpeedboostLocation = new Vector2(shieldBoxX + TileSize, shieldBoxY);
            itemBoxes.Add(new ItemBox("Speedboost", itemBoxSpeedboostLocation, speedBoost.Sprite, speedBoost));
            itemBoxBackgrounds.Add(new StaticEntity("BasicShieldBackground", itemBoxSpeedboostLocation, actionBarBackground));

            Shield thunderStoneItem = shieldFactory.CreateThunderStone();
            Vector2 itemBoxThunderStoneLocation = new Vector2(shieldBoxX + TileSize * 2, shieldBoxY);
            itemBoxes.Add(new ItemBox("ThunderStone", itemBoxThunderStoneLocation, thunderStoneItem.Sprite, thunderStoneItem));
            itemBoxBackgrounds.Add(new StaticEntity("ThunderStoneBackground", itemBoxThunderStoneLocation, actionBarBackground));

            Shield towerShield = shieldFactory.CreateTowerShield();
            Vector2 itemBoxTowerShieldLocation = new Vector2(shieldBoxX + TileSize * 3, shieldBoxY);
            itemBoxes.Add(new ItemBox("TowerShield", itemBoxTowerShieldLocation, towerShield.Sprite, towerShield));
            itemBoxBackgrounds.Add(new StaticEntity("TowerShieldBackground", itemBoxTowerShieldLocation, actionBarBackground));

            Shield elvenTrinketItem = shieldFactory.CreateElvenTrinket();
            Vector2 itemBoxElvenTrinketLocation = new Vector2(shieldBoxX + TileSize * 4, shieldBoxY);
            itemBoxes.Add(new ItemBox("ElvenTrinket", itemBoxElvenTrinketLocation, elvenTrinketItem.Sprite, elvenTrinketItem));
            itemBoxBackgrounds.Add(new StaticEntity("ElvenTrinketBackground", itemBoxElvenTrinketLocation, actionBarBackground));

            Shield bullrushItem = shieldFactory.CreateBullrush();
            Vector2 itemBoxBullrushLocation = new Vector2(shieldBoxX + TileSize * 5, shieldBoxY);
            itemBoxes.Add(new ItemBox("Bullrush", itemBoxBullrushLocation, bullrushItem.Sprite, bullrushItem));
            itemBoxBackgrounds.Add(new StaticEntity("BullrushBackground", itemBoxBullrushLocation, actionBarBackground));
            #endregion

            #region Charm Item Boxes
            int charmBoxY = (int)(TileSize * 5.5);
            int charmBoxX = (int)(TileSize * 1.5);

            Charm burstCharm = charmFactory.CreateBurstCharm();
            Vector2 itemBoxBurstCharmLocation = new Vector2(charmBoxX, charmBoxY);
            itemBoxes.Add(new ItemBox("Burst Charm", itemBoxBurstCharmLocation, burstCharm.Sprite, burstCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Burst Charm Background", itemBoxBurstCharmLocation, actionBarBackground));

            Charm cooldownCharm = charmFactory.CreateLowerCooldown();
            Vector2 itemBoxCoolCharmLocation = new Vector2(charmBoxX + TileSize, charmBoxY);
            itemBoxes.Add(new ItemBox("Cool Charm", itemBoxCoolCharmLocation, cooldownCharm.Sprite, cooldownCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Cool Charm Background", itemBoxCoolCharmLocation, actionBarBackground));

            Charm damageCharm = charmFactory.CreateHigherCooldown();
            Vector2 itemBoxDamageCharmLocation = new Vector2(charmBoxX + TileSize * 2, charmBoxY);
            itemBoxes.Add(new ItemBox("Damage Charm", itemBoxDamageCharmLocation, damageCharm.Sprite, damageCharm));
            itemBoxBackgrounds.Add(new StaticEntity("Damage Charm Background", itemBoxDamageCharmLocation, actionBarBackground));

            Charm speedCharm = charmFactory.CreateSpeedCharm();
            Vector2 itemBoxSpeedCharmLocation = new Vector2(charmBoxX + TileSize * 3, charmBoxY);
            itemBoxes.Add(new ItemBox("speedCharm", itemBoxSpeedCharmLocation, speedCharm.Sprite, speedCharm));
            itemBoxBackgrounds.Add(new StaticEntity("speedCharm Background", itemBoxSpeedCharmLocation, actionBarBackground));

            Charm vampiricCharm = charmFactory.CreateLifestealCharm();
            Vector2 itemBoxVampiricCharmLocation = new Vector2(charmBoxX + TileSize * 4, charmBoxY);
            itemBoxes.Add(new ItemBox("vampiricCharm", itemBoxVampiricCharmLocation, vampiricCharm.Sprite, vampiricCharm));
            itemBoxBackgrounds.Add(new StaticEntity("vampiricCharm Background", itemBoxVampiricCharmLocation, actionBarBackground));
            #endregion

            #region Equipped Item Boxes
            int equipBoxX = (int)(TileSize * 1.5);
            int equipBoxY = (int)(WindowHeight - TileSize * 2.5);

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
            #endregion
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mouse = Mouse.GetState();

            background.Draw(spriteBatch, new Rectangle(0, 0, WindowWidth, WindowHeight));

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
                spriteBatch.DrawString(font, hoveredItemName, new Vector2(TileSize, TileSize * 7), Color.Black);

                spriteBatch.DrawString(font, hoveredItemType, new Vector2(TileSize, TileSize * 7 + font.LineSpacing), Color.Black);

                if (hoveredItemDamage != "")
                    spriteBatch.DrawString(font, "Damage: " + hoveredItemDamage, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 2), Color.Black);

                spriteBatch.DrawString(font, "Effects: " + hoveredItemEffects, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 3), Color.Black);
                if (hoveredItemCooldown != "")
                    spriteBatch.DrawString(font, "Cooldown: " + hoveredItemCooldown + " second(s)", new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 4), Color.Black);
                spriteBatch.DrawString(font, hoveredItemTooltip, new Vector2(TileSize, TileSize * 7 + font.LineSpacing * 5), Color.Black);
            }
            #endregion

            if (playButton.CollisionRectangle.Contains(mouse.Position))
                playButton.Draw(spriteBatch, Color.LightSteelBlue);
            else
                playButton.Draw(spriteBatch);

            #region General text

            spriteBatch.DrawString(font, "Left click on a weapon to equip it in the first slot.", new Vector2(TileSize / 2, 0), Color.Black);
            spriteBatch.DrawString(font, "Right click on a weapon to equip in in the second slot.", new Vector2(TileSize / 2, font.LineSpacing), Color.Black);
            spriteBatch.DrawString(font, "Click on a piece of equipment to put it in the third slot.", new Vector2(TileSize / 2, TileSize * 2 + font.LineSpacing), Color.Black);

            spriteBatch.DrawString(font, "Controls:", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7), Color.Black);
            spriteBatch.DrawString(font, "Left click to use the weapon in Slot One.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing), Color.Black);
            spriteBatch.DrawString(font, "Right click to use the weapon in Slot Two.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 2), Color.Black);
            spriteBatch.DrawString(font, "Spacebar to use equipment.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 3), Color.Black);
            spriteBatch.DrawString(font, "Press Escape to pause and unpause the game.", new Vector2(WindowWidth / 2 + TileSize * 2, TileSize * 7 + font.LineSpacing * 4), Color.Black);
            #endregion

            #region Item boxes
            foreach (StaticEntity itemBoxBackground in itemBoxBackgrounds)
                itemBoxBackground.Draw(spriteBatch, Color.DimGray);

            foreach (ItemBox itemBox in itemBoxes)
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
        }

        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();
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
            if (backButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                ScreenChanged(new MainMenuScreen(ScreenChanged, Content));
            }
            //TODO: Uncomment this and fix the logic
            //if (playButton.CollisionRectangle.Contains(new Vector2(mouse.X, mouse.Y)) && mouse.LeftButton == ButtonState.Pressed && !leftMousePreviouslyPressed)
            //{
            //    InitiateCombat(gameTime);
            //}
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

                }
                else if (mouse.RightButton == ButtonState.Pressed)
                {
                    weapon2 = itemBox.Weapon.Copy();
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
                    charm1 = itemBox.Charm.Copy();
                }
            }
        }
    }
}
