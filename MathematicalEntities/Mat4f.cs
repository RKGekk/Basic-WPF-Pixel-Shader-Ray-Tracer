using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Mat4f {

        public Vec4f[] data;

        public Mat4f() {
            data = new Vec4f[4];

            data[0] = new Vec4f(1.0f, 0.0f, 0.0f, 0.0f);
            data[1] = new Vec4f(0.0f, 1.0f, 0.0f, 0.0f);
            data[2] = new Vec4f(0.0f, 0.0f, 1.0f, 0.0f);
            data[3] = new Vec4f(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public Mat4f(float s) {
            data = new Vec4f[4];

            data[0] = new Vec4f(s);
            data[1] = new Vec4f(s);
            data[2] = new Vec4f(s);
            data[3] = new Vec4f(s);
        }

        public Mat4f(Vec4f row1, Vec4f row2, Vec4f row3, Vec4f row4) {
            data = new Vec4f[4];

            data[0] = row1;
            data[1] = row2;
            data[2] = row3;
            data[3] = row4;
        }

        public Quatf toQuat() {

            Quatf res = new Quatf();

            float tr;
            float s;
            float[] q = new float[4];
            int i, j, k;

            int[] nxt = new int[3] { 1, 2, 3 };

            tr = this.data[0][0] + this.data[1][1] + this.data[2][2];

            if (tr > 0.0f) {
                s = (float)Math.Sqrt(tr + 1.0f);
                res.r = s / 2.0f;
                s = 0.5f / s;
                res.i = (this.data[2][1] - data[1][2]) * s;
                res.j = (this.data[0][2] - data[2][0]) * s;
                res.k = (this.data[1][0] - data[0][1]) * s;
            }
            else {

                i = 0;
                if (this.data[1][1] > this.data[0][0]) i = 1;
                if (this.data[2][2] > this.data[i][i]) i = 2;
                j = nxt[i];
                k = nxt[j];

                s = (float)Math.Sqrt((this.data[i][i] - (this.data[j][j] + this.data[k][k])) + 1.0f);

                q[i] = s * 0.5f;

                if (s != 0.0f) s = 0.5f / s;

                q[3] = (this.data[k][j] - this.data[j][k]) * s;
                q[j] = (this.data[i][j] + this.data[j][i]) * s;
                q[k] = (this.data[i][k] + this.data[k][i]) * s;

                res.i = q[0];
                res.j = q[1];
                res.k = q[2];
                res.r = q[3];
            }

            return res;
        }

        public Mat4f transpose() {

            Mat4f transpMat = new Mat4f();

            transpMat.data[0][0] = data[0][0];
            transpMat.data[0][1] = data[1][0];
            transpMat.data[0][2] = data[2][0];
            transpMat.data[0][3] = data[3][0];

            transpMat.data[1][0] = data[0][1];
            transpMat.data[1][1] = data[1][1];
            transpMat.data[1][2] = data[2][1];
            transpMat.data[1][3] = data[3][1];

            transpMat.data[2][0] = data[0][2];
            transpMat.data[2][1] = data[1][2];
            transpMat.data[2][2] = data[2][2];
            transpMat.data[2][3] = data[3][2];

            transpMat.data[3][0] = data[0][3];
            transpMat.data[3][1] = data[1][3];
            transpMat.data[3][2] = data[2][3];
            transpMat.data[3][3] = data[3][3];

            return transpMat;
        }

        public float determinant() {

            float SubFactor00 = data[2][2] * data[3][3] - data[3][2] * data[2][3];
            float SubFactor01 = data[2][1] * data[3][3] - data[3][1] * data[2][3];
            float SubFactor02 = data[2][1] * data[3][2] - data[3][1] * data[2][2];
            float SubFactor03 = data[2][0] * data[3][3] - data[3][0] * data[2][3];
            float SubFactor04 = data[2][0] * data[3][2] - data[3][0] * data[2][2];
            float SubFactor05 = data[2][0] * data[3][1] - data[3][0] * data[2][1];

            Vec4f DetCof = new Vec4f(
                +(data[1][1] * SubFactor00 - data[1][2] * SubFactor01 + data[1][3] * SubFactor02),
                -(data[1][0] * SubFactor00 - data[1][2] * SubFactor03 + data[1][3] * SubFactor04),
                +(data[1][0] * SubFactor01 - data[1][1] * SubFactor03 + data[1][3] * SubFactor05),
                -(data[1][0] * SubFactor02 - data[1][1] * SubFactor04 + data[1][2] * SubFactor05)
            );

            return
                data[0][0] * DetCof[0] + data[0][1] * DetCof[1] +
                data[0][2] * DetCof[2] + data[0][3] * DetCof[3];
        }

        public Mat4f inverse() {

            float Coef00 = data[2][2] * data[3][3] - data[3][2] * data[2][3];
            float Coef02 = data[1][2] * data[3][3] - data[3][2] * data[1][3];
            float Coef03 = data[1][2] * data[2][3] - data[2][2] * data[1][3];

            float Coef04 = data[2][1] * data[3][3] - data[3][1] * data[2][3];
            float Coef06 = data[1][1] * data[3][3] - data[3][1] * data[1][3];
            float Coef07 = data[1][1] * data[2][3] - data[2][1] * data[1][3];

            float Coef08 = data[2][1] * data[3][2] - data[3][1] * data[2][2];
            float Coef10 = data[1][1] * data[3][2] - data[3][1] * data[1][2];
            float Coef11 = data[1][1] * data[2][2] - data[2][1] * data[1][2];

            float Coef12 = data[2][0] * data[3][3] - data[3][0] * data[2][3];
            float Coef14 = data[1][0] * data[3][3] - data[3][0] * data[1][3];
            float Coef15 = data[1][0] * data[2][3] - data[2][0] * data[1][3];

            float Coef16 = data[2][0] * data[3][2] - data[3][0] * data[2][2];
            float Coef18 = data[1][0] * data[3][2] - data[3][0] * data[1][2];
            float Coef19 = data[1][0] * data[2][2] - data[2][0] * data[1][2];

            float Coef20 = data[2][0] * data[3][1] - data[3][0] * data[2][1];
            float Coef22 = data[1][0] * data[3][1] - data[3][0] * data[1][1];
            float Coef23 = data[1][0] * data[2][1] - data[2][0] * data[1][1];

            Vec4f Fac0 = new Vec4f(Coef00, Coef00, Coef02, Coef03);
            Vec4f Fac1 = new Vec4f(Coef04, Coef04, Coef06, Coef07);
            Vec4f Fac2 = new Vec4f(Coef08, Coef08, Coef10, Coef11);
            Vec4f Fac3 = new Vec4f(Coef12, Coef12, Coef14, Coef15);
            Vec4f Fac4 = new Vec4f(Coef16, Coef16, Coef18, Coef19);
            Vec4f Fac5 = new Vec4f(Coef20, Coef20, Coef22, Coef23);

            Vec4f Vec0 = new Vec4f(data[1][0], data[0][0], data[0][0], data[0][0]);
            Vec4f Vec1 = new Vec4f(data[1][1], data[0][1], data[0][1], data[0][1]);
            Vec4f Vec2 = new Vec4f(data[1][2], data[0][2], data[0][2], data[0][2]);
            Vec4f Vec3 = new Vec4f(data[1][3], data[0][3], data[0][3], data[0][3]);

            Vec4f Inv0 = new Vec4f(Vec1 * Fac0 - Vec2 * Fac1 + Vec3 * Fac2);
            Vec4f Inv1 = new Vec4f(Vec0 * Fac0 - Vec2 * Fac3 + Vec3 * Fac4);
            Vec4f Inv2 = new Vec4f(Vec0 * Fac1 - Vec1 * Fac3 + Vec3 * Fac5);
            Vec4f Inv3 = new Vec4f(Vec0 * Fac2 - Vec1 * Fac4 + Vec2 * Fac5);

            Vec4f SignA = new Vec4f(+1, -1, +1, -1);
            Vec4f SignB = new Vec4f(-1, +1, -1, +1);
            Mat4f Inverse = new Mat4f(Inv0 * SignA, Inv1 * SignB, Inv2 * SignA, Inv3 * SignB);

            Vec4f Row0 = new Vec4f(Inverse.data[0][0], Inverse.data[1][0], Inverse.data[2][0], Inverse.data[3][0]);

            Vec4f Dot0 = new Vec4f(data[0] * Row0);
            float Dot1 = (Dot0.x + Dot0.y) + (Dot0.z + Dot0.w);

            float OneOverDeterminant = 1.0f / Dot1;

            return Inverse * OneOverDeterminant;
        }

        public float this[int i] {
            get {
                if (i == 0)
                    return this.data[0][0];
                if (i == 1)
                    return this.data[0][1];
                if (i == 2)
                    return this.data[0][2];
                if (i == 3)
                    return this.data[0][3];
                if (i == 4)
                    return this.data[1][0];
                if (i == 5)
                    return this.data[1][1];
                if (i == 6)
                    return this.data[1][2];
                if (i == 7)
                    return this.data[1][3];
                if (i == 8)
                    return this.data[2][0];
                if (i == 9)
                    return this.data[2][1];
                if (i == 10)
                    return this.data[2][2];
                if (i == 11)
                    return this.data[2][3];
                if (i == 12)
                    return this.data[3][0];
                if (i == 13)
                    return this.data[3][1];
                if (i == 14)
                    return this.data[3][2];
                return this.data[3][3];
            }
            set {
                if (i == 0)
                    this.data[0][0] = value;
                if (i == 1)
                    this.data[0][1] = value;
                if (i == 2)
                    this.data[0][2] = value;
                if (i == 3)
                    this.data[0][3] = value;
                if (i == 4)
                    this.data[1][0] = value;
                if (i == 5)
                    this.data[1][1] = value;
                if (i == 6)
                    this.data[1][2] = value;
                if (i == 7)
                    this.data[1][3] = value;
                if (i == 8)
                    this.data[2][0] = value;
                if (i == 9)
                    this.data[2][1] = value;
                if (i == 10)
                    this.data[2][2] = value;
                if (i == 11)
                    this.data[2][3] = value;
                if (i == 12)
                    this.data[3][0] = value;
                if (i == 13)
                    this.data[3][1] = value;
                if (i == 14)
                    this.data[3][2] = value;
                this.data[3][3] = value;
            }
        }

        public static Mat4f operator *(Mat4f m1, Mat4f m2) {

            Vec4f ap1 = m1.data[0];
            Vec4f ap2 = m1.data[1];
            Vec4f ap3 = m1.data[2];
            Vec4f ap4 = m1.data[3];

            Vec4f bp1 = m2.data[0];
            Vec4f bp2 = m2.data[1];
            Vec4f bp3 = m2.data[2];
            Vec4f bp4 = m2.data[3];

            Vec4f cp1 = new Vec4f();
            Vec4f cp2 = new Vec4f();
            Vec4f cp3 = new Vec4f();
            Vec4f cp4 = new Vec4f();

            float a0, a1, a2, a3;

            a0 = ap1.x;
            a1 = ap1.y;
            a2 = ap1.z;
            a3 = ap1.w;

            cp1.x = a0 * bp1.x + a1 * bp2.x + a2 * bp3.x + a3 * bp4.x;
            cp1.y = a0 * bp1.y + a1 * bp2.y + a2 * bp3.y + a3 * bp4.y;
            cp1.z = a0 * bp1.z + a1 * bp2.z + a2 * bp3.z + a3 * bp4.z;
            cp1.w = a0 * bp1.w + a1 * bp2.w + a2 * bp3.w + a3 * bp4.w;

            a0 = ap2.x;
            a1 = ap2.y;
            a2 = ap2.z;
            a3 = ap2.w;

            cp2.x = a0 * bp1.x + a1 * bp2.x + a2 * bp3.x + a3 * bp4.x;
            cp2.y = a0 * bp1.y + a1 * bp2.y + a2 * bp3.y + a3 * bp4.y;
            cp2.z = a0 * bp1.z + a1 * bp2.z + a2 * bp3.z + a3 * bp4.z;
            cp2.w = a0 * bp1.w + a1 * bp2.w + a2 * bp3.w + a3 * bp4.w;

            a0 = ap3.x;
            a1 = ap3.y;
            a2 = ap3.z;
            a3 = ap3.w;

            cp3.x = a0 * bp1.x + a1 * bp2.x + a2 * bp3.x + a3 * bp4.x;
            cp3.y = a0 * bp1.y + a1 * bp2.y + a2 * bp3.y + a3 * bp4.y;
            cp3.z = a0 * bp1.z + a1 * bp2.z + a2 * bp3.z + a3 * bp4.z;
            cp3.w = a0 * bp1.w + a1 * bp2.w + a2 * bp3.w + a3 * bp4.w;

            a0 = ap4.x;
            a1 = ap4.y;
            a2 = ap4.z;
            a3 = ap4.w;

            cp4.x = a0 * bp1.x + a1 * bp2.x + a2 * bp3.x + a3 * bp4.x;
            cp4.y = a0 * bp1.y + a1 * bp2.y + a2 * bp3.y + a3 * bp4.y;
            cp4.z = a0 * bp1.z + a1 * bp2.z + a2 * bp3.z + a3 * bp4.z;
            cp4.w = a0 * bp1.w + a1 * bp2.w + a2 * bp3.w + a3 * bp4.w;

            return new Mat4f(cp1, cp2, cp3, cp4);
        }

        public static Vec4f operator *(Mat4f m1, Vec4f v1) {

            Vec4f outRes = new Vec4f();

            outRes.x = v1.x * m1.data[0].x + v1.y * m1.data[1].x + v1.z * m1.data[2].x + m1.data[3].x;
            outRes.y = v1.x * m1.data[0].y + v1.y * m1.data[1].y + v1.z * m1.data[2].y + m1.data[3].y;
            outRes.z = v1.x * m1.data[0].z + v1.y * m1.data[1].z + v1.z * m1.data[2].z + m1.data[3].z;
            float w = v1.x * m1.data[0].w + v1.y * m1.data[1].w + v1.z * m1.data[2].w + m1.data[3].w;

            if (w != 1.0f && w != 0.0f) {
                outRes.x /= w;
                outRes.y /= w;
                outRes.z /= w;
                outRes.w /= w;
            }

            return outRes;
        }

        public static Vec4f operator *(Mat4f m1, Vec3f v1) {
            return m1 * new Vec4f(v1);
        }

        public static Mat4f operator *(Mat4f m1, float s1) {
            return new Mat4f(
                new Vec4f(m1.data[0] * s1),
                new Vec4f(m1.data[1] * s1),
                new Vec4f(m1.data[2] * s1),
                new Vec4f(m1.data[3] * s1)
            );
        }

        public static Mat4f operator +(Mat4f m1, Mat4f m2) {

            Mat4f res = new Mat4f();
            res.data[0] = m1.data[0] + m2.data[0];
            res.data[1] = m1.data[1] + m2.data[1];
            res.data[2] = m1.data[2] + m2.data[2];
            res.data[3] = m1.data[3] + m2.data[3];

            return res;
        }

        public static Mat4f ProjectionMatrix4(float angleOfView, float near, float far) {
            float scale = 1.0f / (float)(Math.Tan(angleOfView * 0.5f * (float)(Math.PI) / 180.0f));
            Vec4f row1 = new Vec4f(scale, 0.0f, 0.0f, 0.0f);
            Vec4f row2 = new Vec4f(0.0f, scale, 0.0f, 0.0f);
            Vec4f row3 = new Vec4f(0.0f, 0.0f, -far / (far - near), -1.0f);
            Vec4f row4 = new Vec4f(0.0f, 0.0f, -far * near / (far - near), 0.0f);

            return new Mat4f(row1, row2, row3, row4);
        }

        public void Rotate(float angle, Vec3f v) {

            float a = angle;
            float c = (float)Math.Cos(a);
            float s = (float)Math.Sin(a);

            Vec3f axis = v.unit();
            Vec3f temp = new Vec3f((1.0f - c) * axis);

            Mat4f Rotate = new Mat4f(0.0f);
            Rotate.data[0][0] = c + temp[0] * axis[0];
            Rotate.data[0][1] = temp[0] * axis[1] + s * axis[2];
            Rotate.data[0][2] = temp[0] * axis[2] - s * axis[1];

            Rotate.data[1][0] = temp[1] * axis[0] - s * axis[2];
            Rotate.data[1][1] = c + temp[1] * axis[1];
            Rotate.data[1][2] = temp[1] * axis[2] + s * axis[0];

            Rotate.data[2][0] = temp[2] * axis[0] + s * axis[1];
            Rotate.data[2][1] = temp[2] * axis[1] - s * axis[0];
            Rotate.data[2][2] = c + temp[2] * axis[2];

            data[0] = data[0] * Rotate.data[0][0] + data[1] * Rotate.data[0][1] + data[2] * Rotate.data[0][2];
            data[1] = data[0] * Rotate.data[1][0] + data[1] * Rotate.data[1][1] + data[2] * Rotate.data[1][2];
            data[2] = data[0] * Rotate.data[2][0] + data[1] * Rotate.data[2][1] + data[2] * Rotate.data[2][2];
        }

        public static Mat4f RotationMatrix(Mat4f m, float angle, Vec3f v) {

            float a = angle;
            float c = (float)Math.Cos(a);
            float s = (float)Math.Sin(a);

            Vec3f axis = v.unit();
            Vec3f temp = new Vec3f((1.0f - c) * axis);

            Mat4f Rotate = new Mat4f(0.0f);
            Rotate.data[0][0] = c + temp[0] * axis[0];
            Rotate.data[0][1] = temp[0] * axis[1] + s * axis[2];
            Rotate.data[0][2] = temp[0] * axis[2] - s * axis[1];

            Rotate.data[1][0] = temp[1] * axis[0] - s * axis[2];
            Rotate.data[1][1] = c + temp[1] * axis[1];
            Rotate.data[1][2] = temp[1] * axis[2] + s * axis[0];

            Rotate.data[2][0] = temp[2] * axis[0] + s * axis[1];
            Rotate.data[2][1] = temp[2] * axis[1] - s * axis[0];
            Rotate.data[2][2] = c + temp[2] * axis[2];

            Mat4f Result = new Mat4f(0.0f);
            Result[0] = m[0] * Rotate.data[0][0] + m[1] * Rotate.data[0][1] + m[2] * Rotate.data[0][2];
            Result[1] = m[0] * Rotate.data[1][0] + m[1] * Rotate.data[1][1] + m[2] * Rotate.data[1][2];
            Result[2] = m[0] * Rotate.data[2][0] + m[1] * Rotate.data[2][1] + m[2] * Rotate.data[2][2];
            Result[3] = m[3];
            return Result;
        }

        public static Mat4f RotationZMatrix(float angle) {

            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            Vec4f row1 = new Vec4f(cosTheta, -sinTheta, 0.0f, 0.0f);
            Vec4f row2 = new Vec4f(sinTheta, cosTheta, 0.0f, 0.0f);
            Vec4f row3 = new Vec4f(0.0f, 0.0f, 1.0f, 0.0f);
            Vec4f row4 = new Vec4f(0.0f, 0.0f, 0.0f, 1.0f);

            return new Mat4f(row1, row2, row3, row4);
        }

        public static Mat4f RotationYMatrix(float angle) {

            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            Vec4f row1 = new Vec4f(cosTheta, 0.0f, sinTheta, 0.0f);
            Vec4f row2 = new Vec4f(0.0f, 1.0f, 0.0f, 0.0f);
            Vec4f row3 = new Vec4f(-sinTheta, 0.0f, cosTheta, 0.0f);
            Vec4f row4 = new Vec4f(0.0f, 0.0f, 0.0f, 1.0f);

            return new Mat4f(row1, row2, row3, row4);
        }

        public static Mat4f RotationXMatrix(float angle) {

            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);

            Vec4f row1 = new Vec4f(1.0f, 0.0f, 0.0f, 0.0f);
            Vec4f row2 = new Vec4f(0.0f, cosTheta, -sinTheta, 0.0f);
            Vec4f row3 = new Vec4f(0.0f, sinTheta, cosTheta, 0.0f);
            Vec4f row4 = new Vec4f(0.0f, 0.0f, 0.0f, 1.0f);

            return new Mat4f(row1, row2, row3, row4);
        }

        public static Mat4f lookAtRH(Vec3f eye, Vec3f center, Vec3f up) {
            Vec3f f = (center - eye).normal();
            Vec3f s = (f * up).normal();
            Vec3f u = s * f;

            Mat4f Result = new Mat4f();
            Result.data[0][0] = s.x;
            Result.data[1][0] = s.y;
            Result.data[2][0] = s.z;
            Result.data[0][1] = u.x;
            Result.data[1][1] = u.y;
            Result.data[2][1] = u.z;
            Result.data[0][2] = -f.x;
            Result.data[1][2] = -f.y;
            Result.data[2][2] = -f.z;
            Result.data[3][0] = -(s.dot(eye));
            Result.data[3][1] = -(u.dot(eye));
            Result.data[3][2] = f.dot(eye);
            return Result;
        }

        public static Mat4f lookAtLH(Vec3f eye, Vec3f center, Vec3f up) {
            Vec3f f = (center - eye).normal();
            Vec3f s = (f * up).normal();
            Vec3f u = s * f;

            Mat4f Result = new Mat4f();
            Result.data[0][0] = s.x;
            Result.data[1][0] = s.y;
            Result.data[2][0] = s.z;
            Result.data[0][1] = u.x;
            Result.data[1][1] = u.y;
            Result.data[2][1] = u.z;
            Result.data[0][2] = f.x;
            Result.data[1][2] = f.y;
            Result.data[2][2] = f.z;
            Result.data[3][0] = -(s.dot(eye));
            Result.data[3][1] = -(u.dot(eye));
            Result.data[3][2] = -(f.dot(eye));
            return Result;
        }
    }
}
