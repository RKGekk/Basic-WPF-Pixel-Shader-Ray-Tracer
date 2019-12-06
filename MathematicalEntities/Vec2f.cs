using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Vec2f : IEquatable<Vec2f> {

        public float x;
        public float y;

        public Vec2f() {
            this.x = 0.0f;
            this.y = 0.0f;
        }

        public Vec2f(float x, float y) {
            this.x = x;
            this.y = y;
        }

        public float dot(Vec2f other) => this.x * other.x + this.y * other.y;

        public float len() => (float)Math.Sqrt(this.x * this.x + this.y * this.y);

        public float square() => this.x * this.x + this.y * this.y;

        public Vec2f normal() {
            float lenght = len();
            Vec2f newVec = new Vec2f(this.x, this.y);
            if (lenght > 0.0f) {
                float invLenght = 1.0f / lenght;
                newVec.x *= invLenght;
                newVec.y *= invLenght;
            }
            return newVec;
        }

        public void normalize() {
            float lenght = len();
            if (lenght > 0.0f) {
                float invLenght = 1.0f / lenght;
                this.x *= invLenght;
                this.y *= invLenght;
            }
        }

        public Vec2f unit() {
            Vec2f result = new Vec2f();
            result.x = this.x;
            result.y = this.y;
            result.normalize();
            return result;
        }

        public Vec2f lcross() {
            Vec2f result = new Vec2f();
            result.x = -this.y;
            result.y = this.x;
            return result;
        }

        public Vec2f rcross() {
            Vec2f result = new Vec2f();
            result.x = this.y;
            result.y = -this.x;
            return result;
        }

        public Vec2f reflect(Vec2f planeNormal) => 2.0f * ((this.negate().dot(planeNormal)) * planeNormal) + this;

        public Vec2f negate() => new Vec2f(-1.0f * this.x, -1.0f * this.y);

        public bool Equals(Vec2f other) {

            double difference_x = Math.Abs(this.x * .0001 + float.Epsilon);
            double difference_y = Math.Abs(this.y * .0001 + float.Epsilon);

            if (Math.Abs(this.x - other.x) <= difference_x && Math.Abs(this.y - other.y) <= difference_y) {
                return true;
            }
            else {
                return false;
            }
        }

        public float this[int i] {
            get {
                if (i == 0) return this.x;
                return this.y;
            }
            set {
                if (i == 0) this.x = value;
                this.y = value;
            }
        }

        public static Vec2f operator +(Vec2f v1, Vec2f v2) => new Vec2f(v1.x + v2.x, v1.y + v2.y);

        public static Vec2f operator -(Vec2f v1, Vec2f v2) => new Vec2f(v1.x - v2.x, v1.y - v2.y);

        public static Vec2f operator *(Vec2f v1, float s1) => new Vec2f(v1.x * s1, v1.y * s1);

        public static Vec2f operator *(float s1, Vec2f v1) => new Vec2f(v1.x * s1, v1.y * s1);
    }
}
