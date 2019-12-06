using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class ParticleWorld {

        List<Particlef> particles;
        bool calculateIterations;
        ParticleForceRegistry registry;
        ParticleContactResolver resolver;
        List<IParticleContactGenerator> contactGenerators;
        ParticleContact[] contacts;
        int maxContacts;

        public ParticleWorld(int maxContacts, int iterations = 0) {

            this.registry = new ParticleForceRegistry();
            this.particles = new List<Particlef>();
            this.contactGenerators = new List<IParticleContactGenerator>();
            this.resolver = new ParticleContactResolver(iterations);
            this.maxContacts = maxContacts;

            contacts = new ParticleContact[maxContacts];
            for (int i = 0; i < maxContacts; ++i) {
                contacts[i] = new ParticleContact();
            }

            calculateIterations = (iterations == 0);
        }

        public int generateContacts() {
            int limit = maxContacts;
            int i = 0;

            foreach (IParticleContactGenerator g in contactGenerators) {

                int used = g.addContact(contacts, /*nextContact,*/ i, limit);
                limit -= used;
                i += used;

                if (limit <= 0) {
                    break;
                }
            }

            return maxContacts - limit;
        }

        public void integrate(float duration) {
            foreach (Particlef p in particles) {
                p.integrate(duration);
            }
        }

        public void runPhysics(float duration) {
            registry.updateForces(duration);
            integrate(duration);
            int usedContacts = generateContacts();

            if (usedContacts > 0) {
                if (calculateIterations) {
                    resolver.Iterations = usedContacts * 2;
                }
                resolver.resolveContacts(contacts, usedContacts, duration);
            }
        }

        public void startFrame() {
            foreach (Particlef p in particles) {
                p.clearAccumulator();
            }
        }

        public List<Particlef> getParticles() {
            return particles;
        }

        public List<IParticleContactGenerator> getContactGenerators() {
            return contactGenerators;
        }

        public ParticleForceRegistry getForceRegistry() {
            return registry;
        }
    }
}
