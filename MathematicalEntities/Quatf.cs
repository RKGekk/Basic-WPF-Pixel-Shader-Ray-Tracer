using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Quatf : IEquatable<Quatf> {

        public float r;
        public float i;
        public float j;
        public float k;

        public Quatf() {
            this.r = 0.0f;
            this.i = 0.0f;
            this.j = 0.0f;
            this.k = 0.0f;
        }

        public Quatf(float r, float i, float j, float k) {
            this.r = r;
            this.i = i;
            this.j = j;
            this.k = k;
        }

        public float len() {
            return (float)Math.Sqrt(this.r * this.r + this.i * this.i + this.j * this.j + this.k * this.k);
        }

        public float square() {
            return this.r * this.r + this.i * this.i + this.j * this.j + this.k * this.k;
        }

        public Mat4f toMat() {

            Mat4f res = new Mat4f();
            float wx, wy, wz, xx, yy, zz, yz, xy, xz, x2, y2, z2;

            x2 = this.i + this.i;
            y2 = this.j + this.j;
            z2 = this.k + this.k;

            xx = this.i * x2; xy = this.i * y2; xz = this.i * z2;
            yy = this.j * y2; yz = this.j * z2; zz = this.k * z2;
            wx = this.r * x2; wy = this.r * y2; wz = this.r * z2;

            res.data[0][0] = 1.0f - (yy + zz); res.data[0][1] = xy - wz;
            res.data[0][2] = xz + wy; res.data[0][3] = 0.0f;

            res.data[1][0] = xy + wz; res.data[1][1] = 1.0f - (xx + zz);
            res.data[1][2] = yz - wx; res.data[1][3] = 0.0f;

            res.data[2][0] = xz - wy; res.data[2][1] = yz + wx;
            res.data[2][2] = 1.0f - (xx + yy); res.data[2][3] = 0.0f;

            res.data[3][0] = 0.0f; res.data[3][1] = 0.0f;
            res.data[3][2] = 0.0f; res.data[3][3] = 1.0f;

            return res;
        }

        public void rotate(Vec3f v1) {
            Quatf q1 = new Quatf(0.0f, v1.x, v1.y, v1.z);
            Quatf q2 = new Quatf(this.r, this.i, this.j, this.k);
            Quatf res = q1 * q2;
            this.r = res.r;
            this.i = res.i;
            this.j = res.j;
            this.k = res.k;
        }

        public void normalize() {
            float d = this.r * this.r + this.i * this.i + this.j * this.j + this.k * this.k;
            float epsilon = 0.0001f + float.Epsilon;
            if (d < epsilon) {
                this.r = 1.0f;
                return;
            }

            d = 1.0f / (float)Math.Sqrt(d);

            this.r *= d;
            this.i *= d;
            this.j *= d;
            this.k *= d;
        }

        public float dot(Quatf other) {
            return (this.r * other.r + this.i * other.i + this.j * other.j + this.k * other.k);
        }

        public void conjugate() {
            this.i *= -1;
            this.j *= -1;
            this.k *= -1;
        }

        public void fromMat(Mat4f other) {

        }

        public bool Equals(Quatf other) {

            double difference_r = Math.Abs(this.r * 0.0001f + float.Epsilon);
            double difference_i = Math.Abs(this.i * 0.0001f + float.Epsilon);
            double difference_j = Math.Abs(this.j * 0.0001f + float.Epsilon);
            double difference_k = Math.Abs(this.k * 0.0001f + float.Epsilon);

            if (Math.Abs(this.r - other.r) <= difference_r && Math.Abs(this.i - other.i) <= difference_i && Math.Abs(this.j - other.j) <= difference_j && Math.Abs(this.k - other.k) <= difference_k) {
                return true;
            }
            else {
                return false;
            }
        }

        public float this[int i] {
            get {
                if (i == 0) return this.r;
                if (i == 1) return this.i;
                if (i == 2) return this.j;
                return this.k;
            }
            set {
                if (i == 0) this.r = value;
                if (i == 1) this.i = value;
                if (i == 2) this.j = value;
                this.k = value;
            }
        }

        public static Quatf operator *(Quatf q1, Quatf q2) {

            Quatf res = new Quatf();

            res.r = q1.r * q2.r - q1.i * q2.i - q1.j * q2.j - q1.k * q2.k;
            res.i = q1.r * q2.i + q1.i * q2.r + q1.j * q2.k - q1.k * q2.j;
            res.j = q1.r * q2.j + q1.j * q2.r + q1.k * q2.i - q1.i * q2.k;
            res.k = q1.r * q2.k + q1.k * q2.r + q1.i * q2.j - q1.j * q2.i;

            return res;
        }

        public static Quatf operator +(Quatf q1, Vec3f v1) {

            Quatf q = new Quatf(0.0f, v1.x, v1.y, v1.z);
            q = q * q1;

            Quatf res = new Quatf();
            res.r = q1.r;
            res.i = q1.i;
            res.j = q1.j;
            res.k = q1.k;

            res.r += q.r * 0.5f;
            res.i += q.i * 0.5f;
            res.j += q.j * 0.5f;
            res.k += q.k * 0.5f;

            return res;
        }
    }
}
