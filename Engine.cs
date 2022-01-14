using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TDG
{
    public class Engine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private Texture2D texture;
        int passedtime = 0;
        int changedtime = 0;
        public Engine(Texture2D texture, Vector2 location)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle>();
            random = new Random();
        }

        private Particle GenerateNewParticle()
        {
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 40;

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public  void TimeReset()
        {
            changedtime = 0;
        }
        public void Update(GameTime gametime)
        {
            int total = 5;
            int timer = 10;
            if (passedtime <= gametime.ElapsedGameTime.Milliseconds)
            {
                passedtime = gametime.ElapsedGameTime.Milliseconds;
                changedtime++;
            }
            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
            if (timer <= changedtime)
            {
                for (int particle = 0; particle < particles.Count; particle++)
                {
                        particles.RemoveAt(particle);
                        particle--;
                }
            }
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
    }
}
