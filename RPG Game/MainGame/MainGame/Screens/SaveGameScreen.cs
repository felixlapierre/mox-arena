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
    class SaveGameScreen : Screen
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

        ContentManager Content { get; set; }
        TradeScreenContents TradeContents { get; set; }
        #endregion

        #region Constructors
        public SaveGameScreen(OnScreenChanged screenChanged, ContentManager content, TradeScreenContents tradeContents) : base(screenChanged)
        {
            TradeContents = tradeContents;
            Content = content;

            for (int i = 0; i < GameConstants.NUMBER_OF_SAVES; i++)
                CheckGameData(i, levelData, weapon1ItemBoxes, weapon2ItemBoxes, shield1ItemBoxes, charm1ItemBoxes);

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
                ScreenChanged(new TradeScreen(ScreenChanged, Content, TradeContents));
            }
            else if (confirmButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                SaveGame(fileSelected);
                ScreenChanged(new TradeScreen(ScreenChanged, Content, TradeContents));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            MouseState mouse = Mouse.GetState();
            if (backButton.CollisionRectangle.Contains(mouse.Position))
                backButton.Draw(spriteBatch, Color.LightBlue);
            else
                backButton.Draw(spriteBatch);
        }
        #endregion

        public void SaveGame(int slot)
        {
            StreamWriter outputFile;
            outputFile = File.CreateText("SaveGame" + slot.ToString());

            outputFile.WriteLine(TradeContents.Level);
            outputFile.WriteLine(TradeContents.Health);
            outputFile.WriteLine(TradeContents.Weapon1.ID);
            outputFile.WriteLine(TradeContents.Weapon2.ID);
            outputFile.WriteLine(TradeContents.Shield1.ID);
            outputFile.WriteLine(TradeContents.Charm1.ID);

            //TODO: Clean up this code
            List<Item> items = new List<Item>();
            items.Add(TradeContents.Item1);
            items.Add(TradeContents.Item2);
            items.Add(TradeContents.Item3);

            foreach (Item item in items)
            {
                if (item == null)
                {
                    outputFile.WriteLine(-1);
                    outputFile.WriteLine(-1);
                }
                else
                {
                    if (item is Weapon)
                    {
                        outputFile.WriteLine(0);
                        outputFile.WriteLine(((Weapon)item).ID);
                    }
                    if (item is Shield)
                    {
                        outputFile.WriteLine(1);
                        outputFile.WriteLine(((Shield)item).ID);
                    }
                    if (item is Charm)
                    {
                        outputFile.WriteLine(2);
                        outputFile.WriteLine(((Charm)item).ID);
                    }
                }
            }

            outputFile.Close();
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
