using BasicRender.Engine;
using MathematicalEntities;
using PhysicsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace BasicRender {

    public struct OneLineCacheStruct {
        public long data1;
        public long data2;
        public long data3;
        public long data4;
        public long data5;
        public long data6;
        public long data7;
        public long data8;
    }

    public struct pLine {
        public pLine(int x0, int y0, int x1, int y1) {
            this.x0 = x0;
            this.y0 = y0;
            this.x1 = x1;
            this.y1 = y1;
        }
        public int x0;
        public int y0;
        public int x1;
        public int y1;
    }

    public struct pBGRA {
        public pBGRA(int blue, int green, int red, int alpha) {
            this.blue = blue;
            this.green = green;
            this.red = red;
            this.alpha = alpha;
        }
        public int blue;
        public int green;
        public int red;
        public int alpha;
    }

    public partial class MainWindow : Window {

        private const int MAX_RAY_DEPTH = 2;
        private const float INFINITY = 100000000.0f;
        private const float M_PI = 3.141592653589793f;

        private GameTimer _timer = new GameTimer();

        private WriteableBitmap _wbStat;
        private Int32Rect _rectStat;
        private byte[] _pixelsStat;
        private int _strideStat;
        private int _pixelWidthStat;
        private int _pixelHeightStat;

        private WriteableBitmap _wb;
        private Int32Rect _rect;
        private byte[] _pixels;
        private int _stride;
        private int _pixelWidth;
        private int _pixelHeight;

        public MainWindow() {
            InitializeComponent();
        }

        static void printLine(byte[] buf, pLine lineCoords, pBGRA color, int pixelWidth) {

            int stride = (pixelWidth * 32) / 8;
            int pixelHeight = buf.Length / stride;

            int x0 = lineCoords.x0;
            int y0 = lineCoords.y0;
            int x1 = lineCoords.x1;
            int y1 = lineCoords.y1;

            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;

            int dy = Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;

            int err = (dx > dy ? dx : -dy) / 2;
            int e2;

            for (; ; ) {

                if (!(x0 >= pixelWidth || y0 >= pixelHeight || x0 < 0 || y0 < 0))
                    printPixel(buf, x0, y0, color, pixelWidth);

                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;

                if (e2 > -dx) {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dy) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        static void printPixel(byte[] buf, int x, int y, pBGRA color, int pixelWidth) {

            int blue = color.blue;
            int green = color.green;
            int red = color.red;
            int alpha = color.alpha;

            int pixelOffset = (x + y * pixelWidth) * 32 / 8;
            buf[pixelOffset] = (byte)blue;
            buf[pixelOffset + 1] = (byte)green;
            buf[pixelOffset + 2] = (byte)red;
            buf[pixelOffset + 3] = (byte)alpha;
        }

        static void fillScreen(byte[] buf, pBGRA color, int pixelWidth) {

            int stride = (pixelWidth * 32) / 8;
            int pixelHeight = buf.Length / stride;

            for (int y = 0; y < pixelHeight; y++)
                for (int x = 0; x < pixelWidth; x++)
                    printPixel(buf, x, y, color, pixelWidth);
        }

        static void lmoveScreen(byte[] buf, pBGRA fillColor, int moveAmt, int pixelWidth) {

            int stride = (pixelWidth * 32) / 8;
            int pixelHeight = buf.Length / stride;

            for (int y = 0; y < pixelHeight; y++) {
                for (int x = 0; x < pixelWidth; x++) {

                    int nextPixel = x + moveAmt;
                    if (nextPixel < pixelWidth) {
                        int pixelOffset = (nextPixel + y * pixelWidth) * 32 / 8;
                        printPixel(buf, x, y, new pBGRA(buf[pixelOffset], buf[pixelOffset + 1], buf[pixelOffset + 2], buf[pixelOffset + 3]), pixelWidth);
                    }
                    else {
                        printPixel(buf, x, y, fillColor, pixelWidth);
                    }
                }
            }
        }

        List<Sphere> _spheres = new List<Sphere>();
        private ParticleWorld _world1 = new ParticleWorld(300, 16);
        private Particlef _particle1;
        private Particlef _particle2;
        private Particlef _particle3;
        private Particlef _particle4;
        private Particlef _particle5;

        private ParticleGravity _g = new ParticleGravity(new Vec3f(0.0f, -9.8f * 2.0f, 0.0f));
        private SphereContact _sphereContactGenerator = new SphereContact();

        private void Window_Initialized(object sender, EventArgs e) {

            _timer.reset();
            _timer.start();

            _pixelWidth = (int)img.Width;
            _pixelHeight = (int)img.Height;

            int stride = (_pixelWidth * 32) / 8;

            _wb = new WriteableBitmap(_pixelWidth, _pixelHeight, 96, 96, PixelFormats.Bgra32, null);
            _rect = new Int32Rect(0, 0, _pixelWidth, _pixelHeight);
            _pixels = new byte[_pixelWidth * _pixelHeight * _wb.Format.BitsPerPixel / 8];

            fillScreen(_pixels, new pBGRA(128, 128, 128, 255), _pixelWidth);
            printLine(_pixels, new pLine(0, 0, 64, 56), new pBGRA(195, 94, 65, 255), _pixelWidth);

            _stride = (_wb.PixelWidth * _wb.Format.BitsPerPixel) / 8;
            _wb.WritePixels(_rect, _pixels, _stride, 0);

            img.Source = _wb;

            // position, radius, surface color, reflectivity, transparency, emission color
            Sphere sphere1 = new Sphere(new Vec3f(0.0f, -10005, -20), 10000, new Vec3f(0.20f, 0.20f, 0.20f), 0, 0.0f);
            _spheres.Add(sphere1);
            _particle1 = new Particlef(sphere1, new Vec3f(0.0f), INFINITY);

            Sphere sphere2 = new Sphere(new Vec3f(0.0f, 2, -20), 6, new Vec3f(1.00f, 0.32f, 0.36f), 1, 0.9f);
            _spheres.Add(sphere2);
            _particle2 = new Particlef(sphere2, new Vec3f(0.0f), 10.0f);

            Sphere sphere3 = new Sphere(new Vec3f(5.0f, 0, -15), 2, new Vec3f(0.90f, 0.76f, 0.46f), 1, 0.9f);
            _spheres.Add(sphere3);
            _particle3 = new Particlef(sphere3, new Vec3f(0.0f), 10.0f);

            Sphere sphere4 = new Sphere(new Vec3f(5.0f, 1, -25), 3, new Vec3f(0.65f, 0.77f, 0.97f), 1, 0.9f);
            _spheres.Add(sphere4);
            _particle4 = new Particlef(sphere4, new Vec3f(0.0f), 10.0f);

            Sphere sphere5 = new Sphere(new Vec3f(-5.5f, 1, -15), 3, new Vec3f(0.90f, 0.90f, 0.90f), 1, 0.0f);
            _spheres.Add(sphere5);
            _particle5 = new Particlef(sphere5, new Vec3f(0.0f), 10.0f);

            _spheres.Add(new Sphere(new Vec3f(20.0f, 30, -40), 3, new Vec3f(0.00f, 0.00f, 0.00f), 0, 0.0f, new Vec3f(3.0f, 3.0f, 3.0f)));

            _world1.getParticles().Add(_particle1);
            _world1.getParticles().Add(_particle2);
            _world1.getParticles().Add(_particle3);
            _world1.getParticles().Add(_particle4);
            _world1.getParticles().Add(_particle5);

            _sphereContactGenerator.init(_world1.getParticles());

            _world1.getForceRegistry().add(_particle1, _g);
            _world1.getForceRegistry().add(_particle2, _g);
            _world1.getForceRegistry().add(_particle3, _g);
            _world1.getForceRegistry().add(_particle4, _g);
            _world1.getForceRegistry().add(_particle5, _g);

            _world1.getContactGenerators().Add(_sphereContactGenerator);

            InitializeStats();

            CompositionTarget.Rendering += UpdateChildren;
        }

        private void InitializeStats() {

            _pixelWidthStat = (int)statImg.Width;
            _pixelHeightStat = (int)statImg.Height;

            _wbStat = new WriteableBitmap(_pixelWidthStat, _pixelHeightStat, 96, 96, PixelFormats.Bgra32, null);
            _rectStat = new Int32Rect(0, 0, _pixelWidthStat, _pixelHeightStat);
            _pixelsStat = new byte[_pixelWidthStat * _pixelHeightStat * _wbStat.Format.BitsPerPixel / 8];

            fillScreen(_pixelsStat, new pBGRA(32, 32, 32, 255), _pixelWidthStat);

            _strideStat = (_wbStat.PixelWidth * _wbStat.Format.BitsPerPixel) / 8;
            _wbStat.WritePixels(_rectStat, _pixelsStat, _strideStat, 0);

            statImg.Source = _wbStat;
        }

        private float _tt = 0.0f;
        private float _tt2 = 0.0f;
        Random _rnd = new Random();

        protected void UpdateChildren(object sender, EventArgs e) {

            RenderingEventArgs renderingArgs = e as RenderingEventArgs;
            _timer.tick();

            float value1 = (float)((_rnd.Next() % 10) - 5);

            float duration = _timer.deltaTime();
            float totalTime = _timer.gameTime();

            _tt += duration;
            if (_tt > 5.0f)
                _tt = 0.0f;

            _tt2 += duration;
            if (_tt2 > 2.0f) {
                _tt2 = 0.0f;

                _particle2.Position = new Vec3f(0.0f, 5.0f + value1, -20);
                _particle2.Velocity = new Vec3f(0.0f);

                _particle3.Position = new Vec3f(5.0f, 10.0f - value1, -15);
                _particle3.Velocity = new Vec3f(0.0f);

                _particle4.Position = new Vec3f(5.0f, 10.0f + value1, -25);
                _particle4.Velocity = new Vec3f(0.0f);

                _particle5.Position = new Vec3f(-5.5f, 10.0f - value1, -15);
                _particle5.Velocity = new Vec3f(0.0f);
            }

            _world1.startFrame();
            _world1.runPhysics(duration);

            _spheres[0].center = _particle1.Position;
            _spheres[1].center = _particle2.Position;
            _spheres[2].center = _particle3.Position;
            _spheres[3].center = _particle4.Position;
            _spheres[4].center = _particle5.Position;

            Fx1.ScreenResolution = new Point(800.0f, 600.0f);
            Fx1.Fov = 90.0f - _tt * 2;
            Fx1.CameraOrigin = new Point3D(0.0f, 0.0f, 0.0f - _tt * 2.0f);

            Fx1.SphereOrigin1 = new Point3D(_spheres[1].center.x, _spheres[1].center.y, _spheres[1].center.z);
            Fx1.SphereRadius1 = _spheres[1].radius;

            Fx1.SphereOrigin2 = new Point3D(_spheres[2].center.x, _spheres[2].center.y, _spheres[2].center.z);
            Fx1.SphereRadius2 = _spheres[2].radius;

            Fx1.SphereOrigin3 = new Point3D(_spheres[3].center.x, _spheres[3].center.y, _spheres[3].center.z);
            Fx1.SphereRadius3 = _spheres[3].radius;

            Fx1.SphereOrigin4 = new Point3D(_spheres[4].center.x, _spheres[4].center.y, _spheres[4].center.z);
            Fx1.SphereRadius4 = _spheres[4].radius;

            updateStats();
        }

        private void updateStats() {

            float duration = _timer.deltaTime();
            float totalTime = _timer.gameTime();
            int iduration = (int)(duration * 1000.0f);

            statsText.Text = $"RenderDuration: {duration * 1000.0f:F2}ms; FPS: {1.0f / duration:F0}; TotalTime: {totalTime:F3}sec";

            lmoveScreen(_pixelsStat, new pBGRA(32, 32, 32, 255), 1, _pixelWidthStat);
            if (iduration < 32)
                printPixel(_pixelsStat, 319, iduration, new pBGRA(0, 255, 0, 255), _pixelWidthStat);
            _wbStat.WritePixels(_rectStat, _pixelsStat, _strideStat, 0);
        }
    }
}
