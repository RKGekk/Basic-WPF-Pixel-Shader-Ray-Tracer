using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public class ParticleContactResolver {

        protected int iterations;
        protected int iterationsUsed;

        public ParticleContactResolver(int iterations) {
            this.iterations = iterations;
        }

        public int Iterations {
            get {
                return iterations;
            }
            set {
                iterations = value;
            }
        }

        public void resolveContacts(ParticleContact[] contactArray, int numContacts, float duration) {
            int i = 0;
            iterationsUsed = 0;
            while (iterationsUsed < iterations) {

                float max = float.MaxValue;
                int maxIndex = numContacts;
                for (i = 0; i < numContacts; i++) {
                    float sepVel = contactArray[i].calculateSeparatingVelocity();
                    if (sepVel < max && (sepVel < 0.0f || contactArray[i].penetration > 0.0f)) {
                        max = sepVel;
                        maxIndex = i;
                    }
                }

                if (maxIndex == numContacts) {
                    break;
                }

                contactArray[maxIndex].resolve(duration);

                Vec3f move1 = contactArray[maxIndex].particleMovement1;
                Vec3f move2 = contactArray[maxIndex].particleMovement2;
                if (move1 != null) {
                    for (i = 0; i < numContacts; i++) {
                        if (contactArray[i].particle1 == contactArray[maxIndex].particle1) {
                            contactArray[i].penetration -= move1.dot(contactArray[i].contactNormal);
                        }
                        else if (contactArray[i].particle1 == contactArray[maxIndex].particle2) {
                            contactArray[i].penetration -= move2.dot(contactArray[i].contactNormal);
                        }
                        if (contactArray[i].particle2 != null) {
                            if (contactArray[i].particle2 == contactArray[maxIndex].particle1) {
                                contactArray[i].penetration += move1.dot(contactArray[i].contactNormal);
                            }
                            else if (contactArray[i].particle2 == contactArray[maxIndex].particle2) {
                                contactArray[i].penetration += move2.dot(contactArray[i].contactNormal);
                            }
                        }
                    }
                }

                iterationsUsed++;
            }
        }
    }
}
