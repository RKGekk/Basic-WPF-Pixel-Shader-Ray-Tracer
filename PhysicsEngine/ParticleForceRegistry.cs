using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class ParticleForceRegistry {

        protected struct ParticleForceRegistration {
            public Particlef particle;
            public IParticleForceGenerator forceGenerator;
        }

        List<ParticleForceRegistration> registrations;

        public ParticleForceRegistry() {
            registrations = new List<ParticleForceRegistration>();
        }

        public void add(Particlef particle, IParticleForceGenerator forceGenerator) {

            ParticleForceRegistration registration = new ParticleForceRegistration();
            registration.particle = particle;
            registration.forceGenerator = forceGenerator;
            registrations.Add(registration);
        }

        public void remove(Particlef particle, IParticleForceGenerator forceGenerator) {

            foreach (ParticleForceRegistration i in registrations) {
                if (i.particle == particle && i.forceGenerator == forceGenerator) {
                    registrations.Remove(i);
                    return;
                }
            }
        }

        public void clear() {
            registrations.Clear();
        }

        public void updateForces(float duration) {

            foreach (ParticleForceRegistration i in registrations) {
                i.forceGenerator.updateForce(i.particle, duration);
            }
        }
    }
}
