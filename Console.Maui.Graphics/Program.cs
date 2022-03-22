using System;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using ClassLibrary1;
using GraphicsTester.Scenarios;

string outputFolder = System.IO.Path.GetFullPath("TestImages");
if (!Directory.Exists(outputFolder))
    Directory.CreateDirectory(outputFolder);

foreach (AbstractScenario scenario in ScenarioList.Scenarios)
{
    using BitmapExportContext bmp = new SkiaBitmapExportContext((int)scenario.Width, (int)scenario.Height, 1f);
    bmp.Canvas.FillColor = Colors.White;
    bmp.Canvas.FillRectangle(0, 0, scenario.Width, scenario.Height);

    scenario.Draw(bmp.Canvas);

    string fileName = GetSafeFilename(scenario.ToString()) + ".png";
    string filePath = Path.Combine(outputFolder, fileName);
    bmp.WriteToFile(filePath);
    Console.WriteLine(filePath);
}


static string GetSafeFilename(string text)
{
    char[] allowedSpecialChars = { '_', '-' };
    char[] chars = text.ToCharArray();
    for (int i = 0; i < chars.Length; i++)
    {
        if (allowedSpecialChars.Contains(chars[i]))
            continue;
        else if (char.IsLetterOrDigit(chars[i]))
            chars[i] = char.ToLowerInvariant(chars[i]);
        else
            chars[i] = '-';
    }

    string safe = new string(chars);
    while (safe.Contains("--"))
        safe = safe.Replace("--", "-");
    return safe.Trim('-');
}

namespace GraphicsTester.Scenarios
{
    public static class ScenarioList
    {
        private static List<AbstractScenario> _scenarios;

        public static List<AbstractScenario> Scenarios
        {
            get
            {
                if (_scenarios == null)
                {
                    _scenarios = new List<AbstractScenario>()
                    {
                        new DrawLines()
                    };
                }

                return _scenarios;
            }
        }
    }
    public class DrawLines : AbstractScenario
    {
        public DrawLines() : base(720, 1024)
        {
        }

        public override void Draw(ICanvas canvas)
        {
            canvas.DrawLine(50, 20.5f, 200, 20.5f);

            canvas.SaveState();

            DrawLinesOfDifferentSizesAndColors(canvas);
            DrawDashedLinesOfDifferentSizes(canvas);
            DrawLinesWithLineCaps(canvas);
            DrawLinesWithAlpha(canvas);
            DrawShadowedLine(canvas);

            canvas.RestoreState();

            canvas.DrawLine(50, 30.5f, 200, 30.5f);
        }

        private static void DrawShadowedLine(ICanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 10;
            canvas.SetShadow(CanvasDefaults.DefaultShadowOffset, CanvasDefaults.DefaultShadowBlur, CanvasDefaults.DefaultShadowColor);
            canvas.DrawLine(50, 400, 200, 400);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = Colors.Salmon;
            canvas.StrokeSize = 10;
            canvas.SetShadow(CanvasDefaults.DefaultShadowOffset, CanvasDefaults.DefaultShadowBlur, CanvasDefaults.DefaultShadowColor);
            canvas.DrawLine(50, 450, 200, 450);
            canvas.RestoreState();
        }

        private static void DrawLinesWithLineCaps(ICanvas canvas)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 20;
            canvas.StrokeDashPattern = null;
            canvas.StrokeLineCap = LineCap.Butt;
            canvas.DrawLine(50, 250, 200, 250);
            canvas.StrokeLineCap = LineCap.Round;
            canvas.DrawLine(50, 300, 200, 300);
            canvas.StrokeLineCap = LineCap.Square;
            canvas.DrawLine(50, 350, 200, 350);

            canvas.StrokeColor = Colors.Blue;
            canvas.StrokeSize = 1;
            canvas.StrokeLineCap = LineCap.Butt;
            canvas.DrawLine(50, 250, 200, 250);
            canvas.DrawLine(50, 300, 200, 300);
            canvas.DrawLine(50, 350, 200, 350);
        }

        private static void DrawDashedLinesOfDifferentSizes(ICanvas canvas)
        {
            canvas.StrokeColor = Colors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = DASHED;
                canvas.DrawLine(50, 100 + i * 10, 200, 100 + i * 10);
                canvas.DrawLine(250, 100.5f + i * 10, 400, 100.5f + i * 10);
            }
        }

        private static void DrawLinesOfDifferentSizesAndColors(ICanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawLine(50, 50 + i * 10, 200, 50 + i * 10);
                canvas.DrawLine(250, 50.5f + i * 10, 400, 50.5f + i * 10);
            }

            canvas.StrokeColor = Colors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawLine(450, 50.5f + i * 10, 600, 50.5f + i * 10);
            }
        }

        private static void DrawLinesWithAlpha(ICanvas canvas)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float)i / 10f;
                canvas.DrawLine(250, 250f + i * 10, 400, 250f + i * 10);
            }

            canvas.Alpha = 1;
        }
    }
    public abstract class AbstractScenario : IPicture, IDrawable
    {
        public static readonly float[] SOLID = null;
        public static readonly float[] DOT_DOT = { 1, 1 };
        public static readonly float[] DOTTED = { 2, 2 };
        public static readonly float[] DASHED = { 4, 4 };
        public static readonly float[] LONG_DASHES = { 8, 4 };
        public static readonly float[] EXTRA_LONG_DASHES = { 16, 4 };
        public static readonly float[] DASHED_DOT = { 4, 4, 1, 4 };
        public static readonly float[] DASHED_DOT_DOT = { 4, 4, 1, 4, 1, 4 };
        public static readonly float[] LONG_DASHES_DOT = { 8, 4, 2, 4 };
        public static readonly float[] EXTRA_LONG_DASHES_DOT = { 16, 4, 8, 4 };

        private float x;
        private float y;
        private float width;
        private float height;
        private string hash;

        public float X
        {
            get => x;
            set => x = value;
        }

        public float Y
        {
            get => y;
            set => y = value;
        }

        public float Width
        {
            get => width;
            set => width = value;
        }

        public float Height
        {
            get => height;
            set => height = value;
        }

        public AbstractScenario(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public AbstractScenario(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Draw(ICanvas canvas)
        {
            // Do nothing by default
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            Draw(canvas);
        }

        public string Hash
        {
            get => hash;
            set => hash = value;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public IImage ToImage(int width, int height, float scale = 1)
        {
            throw new System.NotImplementedException();
        }
    }
}


//// Use Skia to create a Maui graphics context and canvas
//BitmapExportContext bmpContext =SkiaCanvas.  SkiaGraphicsService.Instance.CreateBitmapExportContext(1000, 1000);
//SizeF bmpSize = new(bmpContext.Width, bmpContext.Height);
//ICanvas canvas = bmpContext.Canvas;

//// Draw on the canvas with abstract methods that are agnostic to the renderer
//ClearBackground(canvas, bmpSize, Colors.Navy);
//DrawRandomLines(canvas, bmpSize, 1000);
//DrawBigTextWithShadow(canvas, "测试 This is Maui.Graphics with Skia");
//SaveFig(bmpContext, Path.GetFullPath("quickstart.jpg"));

//Console.WriteLine("-------------------------------");
//foreach (var item in new Library())
//{
//Console.WriteLine($"{item}");
//}


//static void ClearBackground(ICanvas canvas, SizeF bmpSize, Color bgColor)
//{
//    canvas.FillColor = Colors.Navy;
//    canvas.FillRectangle(0, 0, bmpSize.Width, bmpSize.Height);
//}

//static void DrawRandomLines(ICanvas canvas, SizeF bmpSize, int count = 1000)
//{
//    Random rand = new();
//    for (int i = 0; i < count; i++)
//    {
//        canvas.StrokeSize = (float)rand.NextDouble() * 10;

//        canvas.StrokeColor = new Color(
//            red: (float)rand.NextDouble(),
//            green: (float)rand.NextDouble(),
//            blue: (float)rand.NextDouble(),
//            alpha: .2f);

//        canvas.DrawLine(
//            x1: (float)rand.NextDouble() * bmpSize.Width,
//            y1: (float)rand.NextDouble() * bmpSize.Height,
//            x2: (float)rand.NextDouble() * bmpSize.Width,
//            y2: (float)rand.NextDouble() * bmpSize.Height);
//    }
//}

//static void DrawBigTextWithShadow(ICanvas canvas, string text)
//{
//    canvas.FontName = "Courier";
//    canvas.FontSize = 36;
//    canvas.FontColor = Colors.White;
//    canvas.SetShadow(offset: new SizeF(2, 2), blur: 1, color: Colors.Black);
//    canvas.DrawString(text, 20, 50, HorizontalAlignment.Left);
//}

//static void SaveFig(BitmapExportContext bmp, string filePath)
//{
//    bmp.WriteToFile(filePath);
//    Console.WriteLine($"WROTE: {filePath}");
//}



namespace ClassLibrary1
{
    public class Library : IEnumerable<string>
    {
        public IEnumerator<string> GetEnumerator()
        {
            yield return $"{Environment.CurrentDirectory}";
            yield return $"{Directory.GetCurrentDirectory()}";
            yield return $"{GetType().Assembly.Location}";
            yield return $"{Process.GetCurrentProcess().MainModule.FileName}";
            yield return $"{AppDomain.CurrentDomain.BaseDirectory}";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
