using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class Particlef {

        const float zero = float.Epsilon + 0.0001f;
        const float maxMass = float.MaxValue;

        protected Vec3f position;
        protected Vec3f velocity;
        protected Vec3f acceleration;
        protected Vec3f forceAccum;

        protected float radius;
        protected float damping;
        protected float inverseMass;

        public Particlef() {
            this.position = new Vec3f();
            this.velocity = new Vec3f();
            this.acceleration = new Vec3f();
            this.forceAccum = new Vec3f();

            this.radius = 1.0f;
            this.damping = 1.0f;
            this.inverseMass = 1.0f;
        }

        public Particlef(Vec3f position, Vec3f velocity, float radius, float mass) {
            this.position = position;
            this.velocity = velocity;
            this.acceleration = new Vec3f();
            this.forceAccum = new Vec3f();

            this.radius = radius;
            this.damping = 1.0f;
            this.inverseMass = 1.0f / mass;
        }

        public Particlef(Sphere sphere, Vec3f velocity, float mass) {
            this.position = sphere.center;
            this.velocity = velocity;
            this.acceleration = new Vec3f();
            this.forceAccum = new Vec3f();

            this.radius = sphere.radius;
            this.damping = 1.0f;
            this.inverseMass = 1.0f / mass;
        }

        public void integrate(float duration) {

            if (inverseMass <= zero) {
                return;
            }

            position += velocity * duration;

            Vec3f resultAcc = acceleration;
            resultAcc += forceAccum * inverseMass;

            velocity += resultAcc * duration;
            velocity *= (float)Math.Pow(damping, duration);

            clearAccumulator();
        }

        public float Mass {
            get {
                if (inverseMass <= zero) {
                    return maxMass;
                }
                else {
                    return 1.0f / inverseMass;
                }
            }
            set {
                inverseMass = 1.0f / value;
            }
        }

        public float Radius {
            get {
                return radius;
            }
            set {
                radius = value;
            }
        }

        public float InverseMass {
            get {
                return inverseMass;
            }
            set {
                inverseMass = value;
            }
        }


        public bool hasFiniteMass() {
            return inverseMass >= zero;
        }

        public float Damping {
            get {
                return damping;
            }
            set {
                damping = value;
            }
        }

        public Vec3f Position {
            get {
                return position;
            }
            set {
                position = value;
            }
        }

        public void setPosition(float x, float y, float z) {
            position.x = x;
            position.y = y;
            position.z = z;
        }

        public Vec3f Velocity {
            get {
                return velocity;
            }
            set {
                velocity = value;
            }
        }

        public void setVelocity(float x, float y, float z) {
            velocity.x = x;
            velocity.y = y;
            velocity.z = z;
        }

        public Vec3f Acceleration {
            get {
                return acceleration;
            }
            set {
                acceleration = value;
            }
        }

        public void setAcceleration(float x, float y, float z) {
            acceleration.x = x;
            acceleration.y = y;
            acceleration.z = z;
        }

        public void clearAccumulator() {
            forceAccum.x = 0.0f;
            forceAccum.y = 0.0f;
            forceAccum.z = 0.0f;
        }

        public void addForce(Vec3f force) {
            forceAccum += force;
        }
    }
}
