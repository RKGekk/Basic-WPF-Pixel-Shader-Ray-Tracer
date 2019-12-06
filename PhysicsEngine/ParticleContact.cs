using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class ParticleContact {

        public Particlef particle1;
        public Particlef particle2;
        public float restitution;
        public Vec3f contactNormal;
        public float penetration;
        public Vec3f particleMovement1;
        public Vec3f particleMovement2;

        public void resolve(float duration) {
            resolveVelocity(duration);
            resolveInterpenetration(duration);
        }

        public float calculateSeparatingVelocity() {
            Vec3f relativeVelocity = particle1.Velocity;
            if (particle2 != null) {
                relativeVelocity -= particle2.Velocity;
            }

            return relativeVelocity.dot(contactNormal);
        }

        private void resolveVelocity(float duration) {
            float separatingVelocity = calculateSeparatingVelocity();

            if (separatingVelocity > 0.0f) {
                return;
            }

            float newSeparaiongVelocity = -separatingVelocity * restitution;

            Vec3f accCausedVelocity = particle1.Acceleration;
            if (particle2 != null) {
                accCausedVelocity -= particle2.Acceleration;
            }

            float accCausedSepVelocity = accCausedVelocity.dot(contactNormal) * duration;
            if (accCausedSepVelocity < 0.0f) {
                newSeparaiongVelocity += restitution * accCausedSepVelocity;
                if (newSeparaiongVelocity < 0.0f) {
                    newSeparaiongVelocity = 0.0f;
                }
            }

            float deltaVelocity = newSeparaiongVelocity - separatingVelocity;

            float totalInversMass = particle1.InverseMass;
            if (particle2 != null) {
                totalInversMass += particle2.InverseMass;
            }

            if (totalInversMass <= 0.0f) {
                return;
            }

            float impulse = deltaVelocity / totalInversMass;
            Vec3f impulsePerIMass = contactNormal * impulse;

            particle1.Velocity = particle1.Velocity + impulsePerIMass * particle1.InverseMass;
            if (particle2 != null) {
                particle2.Velocity = particle2.Velocity + impulsePerIMass * -particle2.InverseMass;
            }
        }

        private void resolveInterpenetration(float duration) {
            if (penetration <= 0) {
                return;
            }

            float totalInvertMass = particle1.Mass;
            if (particle2 != null) {
                totalInvertMass += particle2.Mass;
            }

            if (totalInvertMass <= 0.0f) {
                return;
            }

            Vec3f movePerIMass = contactNormal * (penetration / totalInvertMass);
            particleMovement1 = movePerIMass * particle1.InverseMass;

            if (particleMovement1 == null) {
                Console.Write("");
            }

            if (particle2 != null) {
                particleMovement2 = movePerIMass * -particle2.InverseMass;
                if (particleMovement2 == null) {
                    Console.Write("");
                }
            }
            else {
                particleMovement2 = new Vec3f(0.0f, 0.0f, 0.0f);
            }

            particle1.Position = particle1.Position + particleMovement1;
            if (particle2 != null) {
                particle2.Position = particle2.Position + particleMovement2;
            }
        }
    }
}
