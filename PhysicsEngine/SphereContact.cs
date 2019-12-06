using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class SphereContact : IParticleContactGenerator {

        private List<Particlef> particles;

        public SphereContact() {
            particles = new List<Particlef>();
        }

        public void init(List<Particlef> particles) {
            this.particles = particles;
        }

        public int addContact(ParticleContact[] contacts, int current, int limit) {

            int count = 0;
            ParticleContact contact = contacts[current];
            foreach (Particlef p1 in particles) {
                foreach (Particlef p2 in particles) {
                    if (p2 != p1) {
                        Vec3f particle1Pos = p1.Position;
                        Vec3f particle2Pos = p2.Position;
                        float rad1 = p1.Radius;
                        float rad2 = p2.Radius;

                        Vec3f contactTrace = particle1Pos - particle2Pos;
                        float distance = contactTrace.len();

                        if (distance < (rad1 + rad2)) {
                            contact.contactNormal = contactTrace.normal();
                            contact.particle1 = p1;
                            contact.particle2 = p2;
                            contact.penetration = (rad1 + rad2) - distance;
                            contact.restitution = 1.0f;
                            current++;
                            contact = contacts[current];
                            count++;
                        }

                        if (count >= limit) return count;
                    }
                }
            }
            return count;
        }
    }
}
