using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Innlevering_2.Graphics
{
    public class Sprite : IDrawable
    {
        protected Texture2D texture;

        public Sprite(Game game, String textureName)
        {
            texture = ((ContentLoader<Texture2D>)game.Services.GetService(typeof(ContentLoader<Texture2D>))).get(textureName);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// Use to scale texture.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position"></param>
        /// <param name="gameTime"></param>
        /// <param name="scale">1.0 = full size 0.5 = half size</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime, float scale)
        {
            spriteBatch.Draw(texture, position,null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void DrawCenter(SpriteBatch spriteBatch, Vector2 position, GameTime gameTime)
        {
            spriteBatch.Draw(texture, position - new Vector2(texture.Width/2, texture.Height/2), Color.White);
        }
    }
}
