using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {
    public class ParticleGravity : IParticleForceGenerator {

        private Vec3f gravity;

        public ParticleGravity() {
            gravity = new Vec3f();
        }

        public ParticleGravity(Vec3f gravityG) {
            this.gravity = gravityG;
        }

        public void updateForce(Particlef particle, float duration) {
            if (!particle.hasFiniteMass()) {
                return;
            }

            particle.addForce(gravity * particle.Mass);
        }
    }
}
