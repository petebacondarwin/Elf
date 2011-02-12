using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Castle.DynamicProxy;

namespace Elf.Tests.Persistence {
    [TestFixture]
    public class ProxyTests {
        [Test]
        public void TestSimpleProxy() {
            var square = new AShape("Square");
            var red = new AColor("Red");
            IColoredShape coloredShape = CreateColoredShapeMixin(square, red);
            Assert.That(coloredShape.Color, Is.EqualTo("Red"));
            Assert.That(coloredShape.Shape, Is.EqualTo("Square"));
            Assert.That(coloredShape.Describe(), Is.EqualTo("A Red Square"));
        }

        public IColoredShape CreateColoredShapeMixin(IHasShape shape, IHasColor color) {
            var options = new ProxyGenerationOptions();
            options.AddMixinInstance(shape);
            options.AddMixinInstance(color);
            var generator = new ProxyGenerator();
            var proxy = generator.CreateClassProxy(typeof(object), new[] { typeof(IColoredShape) }, options) as IColoredShape;
            return proxy;
        }

    }
    public interface IHasShape {
        string Shape { get; }
    }

    public interface IHasColor {
        string Color { get; }
    }

    public class AShape : IHasShape {
        public AShape(string shape) { this.Shape = shape; }
        public string Shape { get; private set; }
    }
    public class AColor : IHasColor {
        public AColor(string color) { this.Color = color; }
        public string Color { get; private set; }
    }

    public interface IColoredShape : IHasShape, IHasColor {
    }

    public static class ColoredShapeHelper {
        public static string Describe(this IColoredShape thing) {
            return "A " + thing.Color + " " + thing.Shape;
        }
    }
}