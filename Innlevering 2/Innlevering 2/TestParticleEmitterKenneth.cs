using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Innlevering_2
{
    //Foreløpig fungerer klassen dårlig.
    class TestParticleEmitterKenneth
    {
        private Random random;
        private List<TestParticlesKenneth> listParticles;

        public TestParticleEmitterKenneth(Game game, string particleTextureName,int amountOfParticles, Vector2 position, Vector2 direction, float radius, float lifeTime, Color color)
        {
            listParticles = new List<TestParticlesKenneth>();
            random = new Random();
            for (int i = 0; i < amountOfParticles; i++)
            {
                listParticles.Add(new TestParticlesKenneth(game, particleTextureName, position, new Vector2(direction.X + (float)random.NextDouble(), direction.Y + (float)random.NextDouble()), radius, lifeTime, color));
            }
        }

        public void Update(GameTime gameTime)
        {
            
            for (int i = 0; i < listParticles.Count; i++)
            {
                if (listParticles.ElementAt(i) != null)
                {

                    bool isAlive = listParticles.ElementAt(i).Update(gameTime);
                    if (isAlive == true)
                    {
                        listParticles.ElementAt(i).Update(gameTime);
                    }
                    else
                    {
                        listParticles.RemoveAt(i);
                    }
                }
                
                
            }

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int i = 0; i < listParticles.Count; i++)
            {
                if(listParticles.ElementAt(i) != null)
                listParticles.ElementAt(i).Draw(spriteBatch, gameTime);
            }

        }
        
    }
}
