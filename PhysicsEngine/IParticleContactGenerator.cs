using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicsEngine {

    public interface IParticleContactGenerator {

        int addContact(ParticleContact[] contacts, int current, int limit);
    }
}
