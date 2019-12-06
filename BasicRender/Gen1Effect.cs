using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;

namespace BasicRender {

    public class Gen1Effect : ShaderEffect {

        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(Gen1Effect), 0);
        public static readonly DependencyProperty ScreenResolutionProperty = DependencyProperty.Register("ScreenResolution", typeof(Point), typeof(Gen1Effect), new UIPropertyMetadata(new Point(0D, 0D), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty FovProperty = DependencyProperty.Register("Fov", typeof(double), typeof(Gen1Effect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty CameraOriginProperty = DependencyProperty.Register("CameraOrigin", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty CameraAngleProperty = DependencyProperty.Register("CameraAngle", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(3)));

        public static readonly DependencyProperty SphereOrigin1Property = DependencyProperty.Register("SphereOrigin1", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(4)));
        public static readonly DependencyProperty SphereRadius1Property = DependencyProperty.Register("SphereRadius1", typeof(double), typeof(Gen1Effect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(5)));
        public static readonly DependencyProperty SphereOrigin2Property = DependencyProperty.Register("SphereOrigin2", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(6)));
        public static readonly DependencyProperty SphereRadius2Property = DependencyProperty.Register("SphereRadius2", typeof(double), typeof(Gen1Effect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(7)));
        public static readonly DependencyProperty SphereOrigin3Property = DependencyProperty.Register("SphereOrigin3", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(8)));
        public static readonly DependencyProperty SphereRadius3Property = DependencyProperty.Register("SphereRadius3", typeof(double), typeof(Gen1Effect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(9)));
        public static readonly DependencyProperty SphereOrigin4Property = DependencyProperty.Register("SphereOrigin4", typeof(Point3D), typeof(Gen1Effect), new UIPropertyMetadata(new Point3D(0D, 0D, 0D), PixelShaderConstantCallback(10)));
        public static readonly DependencyProperty SphereRadius4Property = DependencyProperty.Register("SphereRadius4", typeof(double), typeof(Gen1Effect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(11)));

        public Gen1Effect() {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/BasicRender;component/rr.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(ScreenResolutionProperty);
            this.UpdateShaderValue(FovProperty);
            this.UpdateShaderValue(CameraOriginProperty);
            this.UpdateShaderValue(CameraAngleProperty);

            this.UpdateShaderValue(SphereOrigin1Property);
            this.UpdateShaderValue(SphereRadius1Property);
            this.UpdateShaderValue(SphereOrigin2Property);
            this.UpdateShaderValue(SphereRadius2Property);
            this.UpdateShaderValue(SphereOrigin3Property);
            this.UpdateShaderValue(SphereRadius3Property);
            this.UpdateShaderValue(SphereOrigin4Property);
            this.UpdateShaderValue(SphereRadius4Property);
        }
        public Brush Input {
            get {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set {
                this.SetValue(InputProperty, value);
            }
        }
        public Point ScreenResolution {
            get {
                return ((Point)(this.GetValue(ScreenResolutionProperty)));
            }
            set {
                this.SetValue(ScreenResolutionProperty, value);
            }
        }
        public double Fov {
            get {
                return ((double)(this.GetValue(FovProperty)));
            }
            set {
                this.SetValue(FovProperty, value);
            }
        }
        public Point3D CameraOrigin {
            get {
                return ((Point3D)(this.GetValue(CameraOriginProperty)));
            }
            set {
                this.SetValue(CameraOriginProperty, value);
            }
        }
        public Point3D CameraAngle {
            get {
                return ((Point3D)(this.GetValue(CameraAngleProperty)));
            }
            set {
                this.SetValue(CameraAngleProperty, value);
            }
        }
        public Point3D SphereOrigin1 {
            get {
                return ((Point3D)(this.GetValue(SphereOrigin1Property)));
            }
            set {
                this.SetValue(SphereOrigin1Property, value);
            }
        }
        public double SphereRadius1 {
            get {
                return ((double)(this.GetValue(SphereRadius1Property)));
            }
            set {
                this.SetValue(SphereRadius1Property, value);
            }
        }
        public Point3D SphereOrigin2 {
            get {
                return ((Point3D)(this.GetValue(SphereOrigin2Property)));
            }
            set {
                this.SetValue(SphereOrigin2Property, value);
            }
        }
        public double SphereRadius2 {
            get {
                return ((double)(this.GetValue(SphereRadius2Property)));
            }
            set {
                this.SetValue(SphereRadius2Property, value);
            }
        }
        public Point3D SphereOrigin3 {
            get {
                return ((Point3D)(this.GetValue(SphereOrigin3Property)));
            }
            set {
                this.SetValue(SphereOrigin3Property, value);
            }
        }
        public double SphereRadius3 {
            get {
                return ((double)(this.GetValue(SphereRadius3Property)));
            }
            set {
                this.SetValue(SphereRadius3Property, value);
            }
        }
        public Point3D SphereOrigin4 {
            get {
                return ((Point3D)(this.GetValue(SphereOrigin4Property)));
            }
            set {
                this.SetValue(SphereOrigin4Property, value);
            }
        }
        public double SphereRadius4 {
            get {
                return ((double)(this.GetValue(SphereRadius4Property)));
            }
            set {
                this.SetValue(SphereRadius4Property, value);
            }
        }
    }
}
