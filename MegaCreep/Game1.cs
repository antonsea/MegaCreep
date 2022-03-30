using MegaCreep.GameScreens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MegaCreep
{
    public class Game1 : Game
    {

        private GraphicsDeviceManager _graphics;
        private static SpriteBatch _spriteBatch;
        public static SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }
        #region Tileset Region
        private static Tileset tileset;
        public static Tileset Tileset
        {
            get { return tileset; }
        }

        public static readonly int TileSize = 16;
        #endregion
        #region Font Regions
        private static SpriteFont font;
        public static SpriteFont Font
        {
            get { return font; }
        }

        private static SpriteFont smallFont;
        public static SpriteFont SmallFont
        {
            get { return smallFont; }
        }
        #endregion

        #region Panel size region
        //A note about notation: Positions, sizes, etc are in terms of Tiles not pixels UNLESS EXPLICITLY STATED
        //I bit sloppy, but I need to hardcode the various panel sizes. 
        public static readonly int ScreenWidth = 90;
        public static readonly int ScreenHeight = 50;
        public static readonly int WorldPanelWidth = ScreenWidth;
        public static readonly int WorldPanelHeight = ScreenHeight - BuildPanelHeight;
        public static readonly int BuildPanelWidth = ScreenWidth / 2;
        public static readonly int BuildPanelHeight = 10;
        public static readonly int ResourcePanelWidth = ScreenWidth / 2;
        public static readonly int ResourcePanelHeight = 10;
        //These rectangles need to be made with their sizes in pixels (not tiles)
        public static readonly Rectangle WorldPanel = new Rectangle(0, 0, WorldPanelWidth * TileSize, WorldPanelHeight * TileSize);
        public static readonly Rectangle BuildPanel = new Rectangle(0, WorldPanelWidth * TileSize, BuildPanelWidth * TileSize, BuildPanelHeight * TileSize);
        public static readonly Rectangle ResourcePanel = new Rectangle(BuildPanelWidth * TileSize, WorldPanelWidth * TileSize, ResourcePanelWidth * TileSize, ResourcePanelHeight * TileSize);
        #endregion
        GameStateManager gameStateManager;
        public GameScreen GamePlayScreen;

        #region Performance Region
        private float fps;
        private float updateInterval = 1.0f;
        private float timeSinceLastUpdate = 0.0f;
        private float frameCount = 0;

        #endregion

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Components.Add(new InputHandler(this));

            gameStateManager = new GameStateManager(this);
            Components.Add(gameStateManager);

            GamePlayScreen = new GameScreen(this, gameStateManager);
            gameStateManager.PushState(GamePlayScreen);

            this.IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;

        }

        protected override void Initialize()
        {
            //This sets the window dimensions
            _graphics.PreferredBackBufferWidth = ScreenWidth * TileSize;
            _graphics.PreferredBackBufferHeight = ScreenHeight * TileSize;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //I use monogame which has a content manager that I can load content (images, fonts, etc)
            //This code gets it all loaded in.
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            Texture2D tilesetSheet = Content.Load<Texture2D>(@"Tilesets\CreepTileSet");
            //When loading a tileset need to manually input: the dimensions of a tile (right now I'm assuming all tiles are squares), # tiles wide and # tiles high
            tileset = new Tileset(tilesetSheet, 16, 6, 9);

            font = Content.Load<SpriteFont>(@"Fonts\Font");
            smallFont = Content.Load<SpriteFont>(@"Fonts\SmallFont");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            
            //This code below displays the FPS in the window tile. I use this to see if I have code that is inefficent that makes the game lag.
            //Usually on my computer its between 200 - 300. I become worried if it ever drops below 100 (and it will be really bad if it ever drops below 60) 
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCount++;
            timeSinceLastUpdate += elapsed;


            if (timeSinceLastUpdate > updateInterval)
            {
                fps = frameCount / timeSinceLastUpdate;
                this.Window.Title = "FPS: " + fps.ToString();
                frameCount = 0;
                timeSinceLastUpdate -= updateInterval;
            }
        }
    }
}
