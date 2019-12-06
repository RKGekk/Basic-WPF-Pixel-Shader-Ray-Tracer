using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class GroundContact : IParticleContactGenerator {

        private List<Particlef> _particles = new List<Particlef>();
        private float _w = 640.0f;
        private float _h = 480.0f;

        public GroundContact(float w, float h) {
            this._w = w;
            this._h = h;
        }

        public void init(List<Particlef> particles) {
            this._particles = particles;
        }

        public int addContact(ParticleContact[] contacts, int current, int limit) {

            int count = 0;
            ParticleContact contact = contacts[current];
            foreach (Particlef p in _particles) {

                float y = p.Position.y;
                float x = p.Position.x;
                float r = p.Radius * 2.0f;
                if (y + r > _h) {
                    contact.contactNormal = new Vec3f(0.0f, -1.0f, 0.0f);
                    contact.particle1 = p;
                    contact.particle2 = null;
                    contact.penetration = y - _h;
                    contact.restitution = 0.4f;
                    current++;
                    contact = contacts[current];
                    count++;
                }

                if (y < 0.0f) {
                    contact.contactNormal = new Vec3f(0.0f, 1.0f, 0.0f);
                    contact.particle1 = p;
                    contact.particle2 = null;
                    contact.penetration = -y - r;
                    contact.restitution = 0.4f;
                    current++;
                    contact = contacts[current];
                    count++;
                }

                if (x + r > _w) {
                    contact.contactNormal = new Vec3f(-1.0f, 0.0f, 0.0f);
                    contact.particle1 = p;
                    contact.particle2 = null;
                    contact.penetration = x - _w;
                    contact.restitution = 0.7f;
                    current++;
                    contact = contacts[current];
                    count++;
                }

                if (x < 0.0f) {
                    contact.contactNormal = new Vec3f(1.0f, 0.0f, 0.0f);
                    contact.particle1 = p;
                    contact.particle2 = null;
                    contact.penetration = -x - r;
                    contact.restitution = 0.4f;
                    current++;
                    contact = contacts[current];
                    count++;
                }

                if (count >= limit) return count;
            }
            return count;
        }
    }
}
