using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace TileGame
{
    public class TileGame : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public TextureList textureList;
        public GameState gameState;

        private Camera camera;
        private KeyboardState currentKeyState;
        private KeyboardState previousKeyState;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        private TitleScreen titleScreen;
        private Map map;
        private Character character;

        public TileGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 512;
            graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            gameState = GameState.TitleScreen;
            camera = new Camera(graphics.GraphicsDevice.Viewport);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            titleScreen = new TitleScreen();
            textureList = new TextureList(Content);
            map = new Map(spriteBatch, textureList, graphics.GraphicsDevice.Viewport);
            character = new Character(graphics.GraphicsDevice.Viewport, map.tileList.First());
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // Exit
            if (gameState == GameState.Quit || currentKeyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (gameState == GameState.TitleScreen)
            {
                titleScreen.Update(currentMouseState, previousMouseState, currentKeyState, previousKeyState);
                gameState = titleScreen.gameState;
            }
            else if (gameState == GameState.Game)
            {
                character.Update(currentMouseState, previousMouseState, currentKeyState, previousKeyState, gameTime, map);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));

            spriteBatch.Begin
            (
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.GetViewMatrix(new Vector2(0.5f))
            );

            if (gameState == GameState.TitleScreen)
            {
                titleScreen.Draw(spriteBatch, textureList);
            }
            else if (gameState == GameState.Game)
            {
                map.Draw(spriteBatch, textureList);
                character.Draw(spriteBatch, textureList);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}