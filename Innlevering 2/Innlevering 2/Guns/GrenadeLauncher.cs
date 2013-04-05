using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Innlevering_2.GameObjects;
using Microsoft.Xna.Framework;
using Innlevering_2.ProjectileTypes;

namespace Innlevering_2.Guns
{
    public class GrenadeLauncher : Gun
    {
        //Kenneth: la til 5 isteden for 2 i magazinesize i konstruktør her for å teste HUD.
        //Kenneth: la til hud-texture i konstruktør
        public GrenadeLauncher(Game game)
            : base(game, "granadeLauncher", "rock", .1f, 5, 5)
        {
            //projectile = new ProjectileData("RPG", new Rectangle(-5, -5, 10, 10), 0, Vector2.UnitY * 150, 30, 30, true);
        }

        public override void Fire(World world, Player player, GameTime gameTime)
        {
            if (CooldownTimer <= 0 /*&& MagazineCount > 0*/)
            {
                world.AddProjectile(new HandGrenade(player, player.Position, Vector2.Normalize(player.getReticulePosition() - player.Position) * 200, 5f));
                CooldownTimer = Cooldown;
                //MagazineCount--;
            }

        }
    }
}
