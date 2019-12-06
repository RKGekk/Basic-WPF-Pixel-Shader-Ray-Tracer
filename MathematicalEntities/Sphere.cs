using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Sphere {

        public Vec3f center;        // position of the sphere
        public float radius;        //sphere radius
        public float radius2;       //sphere radius^2
        public Vec3f surfaceColor;  //surface color
        public Vec3f emissionColor; //surface emission (light)
        public float transparency;  //surface transparency
        public float reflection;    //surface reflectivity

        public Sphere(Vec3f c, float r, Vec3f sc, float refl = 0, float transp = 0, Vec3f ec = null) {

            center = c;
            radius = r;
            radius2 = r * r;
            surfaceColor = sc;
            emissionColor = ec == null ? new Vec3f() : ec;
            transparency = transp;
            reflection = refl;
        }

        public (bool, float, float) intersect(Vec3f rayorig, Vec3f raydir) {

            float t0 = 0.0f;
            float t1 = 0.0f;
            Vec3f l = center - rayorig;
            float tca = l.dot(raydir);
            if (tca < 0) return (false, t0, t1);
            float d2 = l.dot(l) - tca * tca;
            if (d2 > radius2) return (false, t0, t1);
            float thc = (float)Math.Sqrt(radius2 - d2);
            t0 = tca - thc;
            t1 = tca + thc;

            return (true, t0, t1);
        }
    }
}
