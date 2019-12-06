using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    class UserForce : IParticleForceGenerator {

        private bool _wasForced = false;
        private float _durationAcc = 0.0f;

        public void updateForce(Particlef particle, float duration) {

            _durationAcc += duration;

            /*
            if (Keyboard.IsKeyDown(Key.D) || Keyboard.IsKeyDown(Key.Right)) {
                particle.addForce(new Vec3f(1000.0f, 0.0f, 0.0f));
            }

            if (Keyboard.IsKeyDown(Key.A) || Keyboard.IsKeyDown(Key.Left)) {
                particle.addForce(new Vec3f(-1000.0f, 0.0f, 0.0f));
            }

            if (Keyboard.IsKeyDown(Key.Space)) {
                if (!_wasForced && _durationAcc > 0.5f) {
                    particle.addForce(new Vec3f(0.0f, -200000.0f, 0.0f));
                    _wasForced = true;
                    _durationAcc = 0.0f;
                }
            }
            else {
                _wasForced = false;
            }
            */
        }
    }
}
