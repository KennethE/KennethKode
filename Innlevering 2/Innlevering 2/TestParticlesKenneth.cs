using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Innlevering_2
{
    //Foreløpig fungerer klassen dårlig.
    class TestParticlesKenneth
    {

        private Texture2D particleTexture;
        private Vector2 position { get; set; }
        private Vector2 direction { get; set; }
        private float radius;
        private float lifeTime;
        private Color color;

        public TestParticlesKenneth(Game game, string particleTextureName, Vector2 position, Vector2 direction, float radius, float lifeTime, Color color)
        {
            this.particleTexture = ((ContentLoader<Texture2D>)game.Services.GetService(typeof(ContentLoader<Texture2D>))).get(particleTextureName);
           // this.particleTexture = particleTexture;
            this.position = position;
            this.direction = direction;
            this.radius = radius;
            this.lifeTime = lifeTime;
            this.color = color;
        }

        public bool Update(GameTime gameTime)
        {
            this.position += this.direction * gameTime.ElapsedGameTime.Milliseconds;
            

            this.lifeTime -= gameTime.ElapsedGameTime.Seconds;
            if(this.lifeTime > 0)
            {
                return true;
            }else
                return false;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            spriteBatch.Draw(particleTexture,position,Color.White);

        }
    }
}
