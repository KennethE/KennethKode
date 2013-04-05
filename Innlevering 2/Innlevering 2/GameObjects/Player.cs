using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using C3.XNA;
using Innlevering_2.Graphics;
using Innlevering_2.Guns;

namespace Innlevering_2.GameObjects
{
    public class Player : GameObject
    {
        private Rectangle relativeBounds;
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(relativeBounds.X + Position.X), (int)(relativeBounds.Y + Position.Y), relativeBounds.Width, relativeBounds.Height);
            }
        }
        //public Physics physics;
        public float Speed { get; protected set; }
        public float FallingSpeed { get; protected set; }

        private float gravity = 1000f;
        private float jumpForce = 400f;

        public bool Grounded { get; protected set; }

        private int walkSlope = 5;

        private Sprite reticule;
        private float reticuleAngle;

        private PlayerIndex playerIndex;
        private GamePadController controller;

        public Gun Wepon
        {
            get
            {
                return guns[activeGun];
            }
        }

        private List<Gun> guns;
        private int activeGun = 0;

        //Kenneth: Health blir hardkodet i konstruktør til 100.
        public int Health { get; protected set; }

        //Kenneth: PlayerName er foreløpig hardkodet i konstruktør, ønsker å ta det inn som parameter ved senere tidspunkt.
        public string PlayerName { get; protected set; }

        //Kenneth: La til Life-property. Hardkoder inn i konstruktør, ta som parameter senere.
        public int Life { get; protected set; }

        //Kenneth:
        public int Deaths { get; protected set; }

        //Kenneth:
        public int Kills { get; protected set; }

        //Kenneth: Må ha keyboard for å teste inventory
        KeyboardState oldKeyboardState;
        KeyboardState currentKeyboardState;
        

        public Player(Game game, PlayerIndex playerIndex, Vector2 PlayerPosition)
            : base(game)
        {
            guns = new List<Gun>(new Gun[]{
                new GrenadeLauncher(Game),
                new HandGun(Game)}
            );
            this.playerIndex = playerIndex;
            Position = PlayerPosition;
            relativeBounds = new Rectangle(-10, -10, 20, 20);
            Speed = 200;
            reticule = new Sprite(Game, "reticule_new");
            controller = new GamePadController(playerIndex);

            //Kenneth: Hardkoder inn spillernavn,health, life, deaths, kills. ønsker som parameter senere
            PlayerName = "Godzilla!";
            Health = 100;
            Life = 5;
            Deaths = 0;
            Kills = 0;
            //la til keyboard
            oldKeyboardState = Keyboard.GetState();
        }

        public void Update(World world, GameTime gameTime)
        {
            controller.Update(gameTime);
            
            oldKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            Wepon.Update(gameTime);

            //if (controller.ButtonWasPressed(Buttons.RightTrigger))
            if (controller.gamePadState.IsButtonDown(Buttons.RightTrigger) || currentKeyboardState.IsKeyDown(Keys.R))
            {
                Wepon.Fire(world, this, gameTime);
                //((DestructableLevel)level).removeCircle(getReticulePosition(), 20);
            }
            if (controller.ButtonWasPressed(Buttons.X))
            {
                Wepon.Reload();
                //((DestructableLevel)level).removeCircle(getReticulePosition(), 20);
            }
            //Kenneth: Tester ut inventory
            if (currentKeyboardState.IsKeyDown(Keys.E) && oldKeyboardState.IsKeyUp(Keys.E))
            {
                if (activeGun >= guns.Count - 1)
                {
                    activeGun = 0;
                }
                else
                    activeGun++;
                }
            
            //reticule position

            if (controller.gamePadState.ThumbSticks.Right.LengthSquared() > .75f)
            {
                reticuleAngle = (float)Math.Atan2(controller.gamePadState.ThumbSticks.Right.Y, controller.gamePadState.ThumbSticks.Right.X);
            }

            Vector2 move = Vector2.Zero;

            //Movement
            if (Math.Abs(controller.gamePadState.ThumbSticks.Left.X) >= 0.2f)
                move += controller.gamePadState.ThumbSticks.Left * Vector2.UnitX;
            //if (Math.Abs(controller.gamePadState.ThumbSticks.Left.Y) >= 0.2f)
            //    move -= controller.gamePadState.ThumbSticks.Left * Vector2.UnitY;



            if (Grounded)
            {
                if (controller.ButtonWasPressed(Buttons.A))
                {
                    jump();
                }
                TryWalk(move * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, world.Level);
            }
            else
            {
                TryMove(move * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds, world.Level);
                Fall(gameTime, world.Level);
            }
        }

        public bool TryWalk(Vector2 rel, ICollidable collision)
        {
            int tries = -walkSlope;
            while (collision.Collide(new Rectangle(Bounds.X + (int)Math.Round(rel.X * (walkSlope - tries) / walkSlope), Bounds.Y + (int)Math.Round(rel.Y) - tries, Bounds.Width, Bounds.Height)) &&
                tries < walkSlope)
            {
                tries++;
            }
            if (tries == walkSlope)
            {
                return false;
            }
            if (tries == -walkSlope)
            {
                Grounded = false;
                move(rel);
                return true;
            }

            move(rel * new Vector2((walkSlope - tries) / walkSlope, 1) - Vector2.UnitY * tries);
            return true;
        }
        public bool TryMove(Vector2 rel, ICollidable collision)
        {
            if (collision.Collide(new Rectangle(Bounds.X + (int)Math.Round(rel.X), Bounds.Y + (int)Math.Round(rel.Y), Bounds.Width, Bounds.Height)))
            {
                return false;
            }

            move(rel);
            return true;
        }

        public void Fall(GameTime gameTime, ICollidable collision)
        {
            FallingSpeed += 400 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Grounded = !TryMove(Vector2.UnitY * (float)gameTime.ElapsedGameTime.TotalSeconds * FallingSpeed, collision);
            if (Grounded)
            {
                FallingSpeed = 0;
                TryWalk(Vector2.Zero, collision);
            }
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            //debug
            Primitives2D.DrawRectangle(spriteBatch, Bounds,
                Color.Brown);

            reticule.DrawCenter(spriteBatch, getReticulePosition(), gameTime);
        }

        public Vector2 getReticulePosition()
        {
            return new Vector2((float)Math.Cos(reticuleAngle), -(float)Math.Sin(reticuleAngle)) * 40 + Position;
        }

        //Kenneth: prøvde noe her i sammenheng med testing av "particle-system".
        public Vector2 getReticulePositionNormalized()
        {
            Vector2 temp = new Vector2((float)Math.Cos(reticuleAngle), -(float)Math.Sin(reticuleAngle));
            temp.Normalize();
            return temp;
        }

        public void Damage(Projectile projectile)
        {
            // TODO take damage
        }

        /*protected bool Collide()
        {
            Rectangle playerRect = new Rectangle((int)Position.X, (int)Position.Y, PlayerSize.X, PlayerSize.Y);
            Rectangle mapFrameRect = new Rectangle(room.PosX, room.PosY, room.Width, room.Height);

            return (playerRect.Intersects(mapFrameRect));
        }*/

        internal void jump()
        {
            Grounded = false;
            FallingSpeed = -400;
        }
    }
}
