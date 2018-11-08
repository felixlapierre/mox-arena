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

        ContentManager Content { get; set; }
        #endregion

        #region Constructors
        public LoadGameScreen(OnScreenChanged screenChanged, ContentManager content) : base(screenChanged)
        {
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
                gameState = GameState.MainMenu;
            }
            else if (confirmButton.CollisionRectangle.Contains(mouse.Position) && mouse.LeftButton == ButtonState.Pressed)
            {
                if (LoadGame(fileSelected) == true)
                    gameState = GameState.Trade;
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

        private bool LoadGame(int slot)
        {
            StreamReader inputFile;
            bool loadSuccessful = true;
            try
            {
                inputFile = File.OpenText("SaveGame" + slot.ToString());

                level = int.Parse(inputFile.ReadLine());
                Health = int.Parse(inputFile.ReadLine());
                oldHealth = Health;
                player1.HitPoints = Health;
                weapon1 = GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                oldWeapon1 = weapon1;
                player1.Weapon1 = weapon1;
                weapon2 = GetWeaponFromID(int.Parse(inputFile.ReadLine()));
                oldWeapon2 = weapon2;
                player1.Weapon2 = weapon2;
                shield1 = GetShieldFromID(int.Parse(inputFile.ReadLine()));
                oldShield1 = shield1;
                player1.Shield1 = shield1;
                charm1 = GetCharmFromID(int.Parse(inputFile.ReadLine()));
                oldCharm1 = charm1;
                player1.Charm1 = charm1;
                for (int i = 0; i < 3; i++)
                {
                    switch (int.Parse(inputFile.ReadLine()))
                    {
                        case -1:
                            tradeItemBoxes.ElementAt(i).IsActive = false;
                            inputFile.ReadLine();
                            break;
                        case 0:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetWeaponFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 1:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetShieldFromID(int.Parse(inputFile.ReadLine())));
                            break;
                        case 2:
                            tradeItemBoxes.ElementAt(i).ReplaceItem(GetCharmFromID(int.Parse(inputFile.ReadLine())));
                            break;
                    }
                }

                inputFile.Close();
            }
            catch
            {
                loadSuccessful = false;
            }

            return loadSuccessful;
        }
    }
}
