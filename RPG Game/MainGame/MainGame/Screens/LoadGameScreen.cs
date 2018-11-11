using MainGame.Items;
using MainGame.Screens.Trade_Screen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainGame.Screens
{
    class LoadGameScreen : Screen
    {
        #region Properties
        int fileSelected;

        Texture2D blankButtonSprite;
        Texture2D confirmButtonSprite;
        Texture2D backButtonSprite;

        List<StaticEntity> fileButtons;
        List<String> levelData;
        List<ItemBox> weapon1ItemBoxes;
        List<ItemBox> weapon2ItemBoxes;
        List<ItemBox> shield1ItemBoxes;
        List<ItemBox> charm1ItemBoxes;
        StaticEntity confirmButton;
        StaticEntity backButton;

        SpriteFont font;
        SpriteFont font20;
        Texture2D actionBarBackground;
        StaticEntity background;

        ContentManager Content { get; set; }
        TradeScreenContents TradeContents { get; set; }
        #endregion

        #region Constructors
        public LoadGameScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
            Content = content;
            var TileSize = GameConstants.TILE_SIZE;
            fileSelected = -1;
            blankButtonSprite = Content.Load<Texture2D>("graphics/blankButton");
            confirmButtonSprite = Content.Load<Texture2D>("graphics/confirmButton");
            backButtonSprite = Content.Load<Texture2D>("graphics/backButton");
            backButton = new StaticEntity("Back Button", new Vector2(TileSize * 2, GameConstants.WINDOW_HEIGHT - TileSize), backButtonSprite);

            fileButtons = new List<StaticEntity>();
            levelData = new List<String>();
            weapon1ItemBoxes = new List<ItemBox>();
            weapon2ItemBoxes = new List<ItemBox>();
            shield1ItemBoxes = new List<ItemBox>();
            charm1ItemBoxes = new List<ItemBox>();
            for (int i = 0; i < GameConstants.NUMBER_OF_SAVES; i++)
            {
                fileButtons.Add(new StaticEntity("Button " + i.ToString(), new Vector2(TileSize * 2, TileSize * 3 + i * TileSize * 2), blankButtonSprite));
                levelData.Add("");
                weapon1ItemBoxes.Add(new ItemBox("Weapon1 File " + i.ToString(), new Vector2(TileSize * 7, TileSize * 3 + i * TileSize * 2)));
                weapon2ItemBoxes.Add(new ItemBox("Weapon2 File " + i.ToString(), new Vector2(TileSize * 8, TileSize * 3 + i * TileSize * 2)));
                shield1ItemBoxes.Add(new ItemBox("Shield1 File " + i.ToString(), new Vector2(TileSize * 9, TileSize * 3 + i * TileSize * 2)));
                charm1ItemBoxes.Add(new ItemBox("Charm1 File " + i.ToString(), new Vector2(TileSize * 10, TileSize * 3 + i * TileSize * 2)));
            }
            confirmButton = new StaticEntity("Confirm Button", new Vector2(TileSize * 5, GameConstants.WINDOW_HEIGHT - TileSize), confirmButtonSprite);

            font = Content.Load<SpriteFont>("font/font");
            font20 = Content.Load<SpriteFont>("font/font20");
            actionBarBackground = Content.Load<Texture2D>("graphics/actionBarBackground");
            background = new StaticEntity("Background", new Vector2(GameConstants.WINDOW_WIDTH / 2, GameConstants.WINDOW_HEIGHT / 2), actionBarBackground);

            for (int i = 0; i < GameConstants.NUMBER_OF_SAVES; i++)
                CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            foreach (StaticEntity button in fileButtons)
            {
                if (button.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
                {
                    fileSelected = fileButtons.IndexOf(button);
                }
            }
            if (backButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                ScreenChanged(new MainMenuScreen(ScreenChanged, Content));
            }
            else if (confirmButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                if (LoadGame(fileSelected) == true)
                    ScreenChanged(new TradeScreen(ScreenChanged, Content, TradeContents));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();

            background.Draw(spriteBatch, new Rectangle(0, 0, GameConstants.WINDOW_WIDTH, GameConstants.WINDOW_HEIGHT));

            if (fileSelected != -1)
            {
                if (confirmButton.CollisionRectangle.Contains(mouse.Position))
                    confirmButton.Draw(spriteBatch, Color.LightBlue);
                else
                    confirmButton.Draw(spriteBatch);
            }

            foreach (StaticEntity button in fileButtons)
            {
                if (fileButtons.IndexOf(button) == fileSelected)
                    button.Draw(spriteBatch, Color.SteelBlue);
                else if (button.CollisionRectangle.Contains(mouse.Position))
                    button.Draw(spriteBatch, Color.LightBlue);
                else
                    button.Draw(spriteBatch);
            }

            for (int i = 0; i < levelData.Count; i++)
                spriteBatch.DrawString(font, levelData.ElementAt(i), new Vector2(GameConstants.TILE_SIZE * 3.1f, GameConstants.TILE_SIZE * 2.7f + GameConstants.TILE_SIZE * 2 * i), Color.Black);
            foreach (ItemBox box in weapon1ItemBoxes)
                if(box.IsActive)
                    box.Draw(spriteBatch);
            foreach (ItemBox box in weapon2ItemBoxes)
                if (box.IsActive)
                    box.Draw(spriteBatch);
            foreach (ItemBox box in shield1ItemBoxes)
                if (box.IsActive)
                    box.Draw(spriteBatch);
            foreach (ItemBox box in charm1ItemBoxes)
                if (box.IsActive)
                    box.Draw(spriteBatch);

            spriteBatch.DrawString(font20, "Load Game", new Vector2(GameConstants.TILE_SIZE, GameConstants.TILE_SIZE), Color.Black);

            if (backButton.CollisionRectangle.Contains(mouse.Position))
                backButton.Draw(spriteBatch, Color.LightBlue);
            else
                backButton.Draw(spriteBatch);
        }
        #endregion

        private bool LoadGame(int slot)
        {
            TradeContents = new TradeScreenContents();
            StreamReader inputFile;
            bool loadSuccessful = true;
            try
            {
                inputFile = File.OpenText("SaveGame" + slot.ToString());

                //TODO: Clean up this code

                TradeContents.Level = int.Parse(inputFile.ReadLine());
                TradeContents.Health = int.Parse(inputFile.ReadLine());
                //oldHealth = Health;
                //player1.HitPoints = Health;
                TradeContents.Weapon1 = ItemFactoryContainer.GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                //oldWeapon1 = weapon1;
                //player1.Weapon1 = weapon1;
                TradeContents.Weapon2 = ItemFactoryContainer.GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                //oldWeapon2 = weapon2;
                //player1.Weapon2 = weapon2;
                TradeContents.Shield1 = ItemFactoryContainer.GetShieldFromID(int.Parse(inputFile.ReadLine()));
                //oldShield1 = shield1;
                //player1.Shield1 = shield1;
                TradeContents.Charm1 = ItemFactoryContainer.GetCharmFromID(int.Parse(inputFile.ReadLine()));
                //oldCharm1 = charm1;
                //player1.Charm1 = charm1;
                List<Item> items = new List<Item>();
                for (int i = 0; i < 3; i++)
                {
                    switch (int.Parse(inputFile.ReadLine()))
                    {
                        case -1:
                            items.Add(null);
                            inputFile.ReadLine();
                            break;
                        case 0:
                            items.Add(ItemFactoryContainer.GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 1:
                            items.Add(ItemFactoryContainer.GetShieldFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 2:
                            items.Add(ItemFactoryContainer.GetCharmFromID(int.Parse(inputFile.ReadLine())));
                            break;
                    }
                }

                TradeContents.Item1 = items.ElementAt(0);
                TradeContents.Item2 = items.ElementAt(1);
                TradeContents.Item3 = items.ElementAt(2);

                inputFile.Close();
            }
            catch
            {
                loadSuccessful = false;
            }

            return loadSuccessful;
        }

        public bool CheckGameData(int slot, List<String> levelData, List<ItemBox> weapon1Box, List<ItemBox> weapon2Box, List<ItemBox> shield1Box, List<ItemBox> charm1Box)
        {
            StreamReader inputFile;
            bool checkSuccessful = true;
            try
            {
                inputFile = File.OpenText("SaveGame" + slot.ToString());

                //Put the get items methods in their own class with the weapon factories
                levelData.RemoveAt(slot);
                levelData.Insert(slot, "Level " + inputFile.ReadLine() + "    " + inputFile.ReadLine() + " hitpoints.");
                weapon1Box.ElementAt(slot).ReplaceItem(ItemFactoryContainer.GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                weapon2Box.ElementAt(slot).ReplaceItem(ItemFactoryContainer.GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                shield1Box.ElementAt(slot).ReplaceItem(ItemFactoryContainer.GetShieldFromID(int.Parse(inputFile.ReadLine())));
                charm1Box.ElementAt(slot).ReplaceItem(ItemFactoryContainer.GetCharmFromID(int.Parse(inputFile.ReadLine())));

                inputFile.Close();
            }
            catch
            {
                checkSuccessful = false;
            }
            return checkSuccessful;
        }

    }
}
