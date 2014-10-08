module Color

open Util

let linearToGamma x =
    x ** (1.0/2.2)

let gammaToLinear x =
    x ** 2.2

let floatColorToScreen = linearToGamma >> (*) 255.0 >> clamp 0.0 255.0

let floatColorToEightBit =
    linearToGamma >> (*) 255.0 >> round >> int >> max 0 >> min 255

let EightBitToFloatColor =
    float >> ((*) (1.0/255.0)) >> gammaToLinear >> max 0.0 >> min 1.0

type RgbColor =
    struct
        val r:float
        val g:float
        val b:float
        new(r, g, b) = { r = r; g = g; b = b; }

        static member Zero = RgbColor(0.0, 0.0, 0.0)

        static member white = RgbColor(1.0, 1.0, 1.0)
        static member black = RgbColor(0.0, 0.0, 0.0)
        static member red = RgbColor(1.0, 0.0, 0.0)
        static member green = RgbColor(0.0, 1.0, 0.0)
        static member blue = RgbColor(0.0, 0.0, 1.0)
        static member yellow = RgbColor(1.0, 1.0, 0.0)
        static member magenta = RgbColor(1.0, 0.0, 1.0)
        static member cyan = RgbColor(0.0, 1.0, 1.0)

        static member create (r, g, b) =
            RgbColor(r, g, b)
        static member map f (c:RgbColor) =
                RgbColor(f c.r, f c.g, f c.b)
        static member map2 f (a:RgbColor) (b:RgbColor) =
            RgbColor(f a.r b.r, f a.g b.g, f a.b b.b)
        static member (+) (a, b) = RgbColor.map2 (+) a b
        static member (-) (a, b) = RgbColor.map2 (-) a b
        static member (*) (c, s) = RgbColor.map ((*) s) c
        static member DivideByInt (v:RgbColor, i) : RgbColor =
            let fi = float i
            RgbColor.map ((*) (1.0 / fi)) v
        static member RawToGdiColor (c:RgbColor) =
            System.Drawing.Color.FromArgb(
                int c.r,
                int c.g,
                int c.b)
        static member ToGdiColor (c:RgbColor) : System.Drawing.Color =
            System.Drawing.Color.FromArgb(
                floatColorToEightBit c.r,
                floatColorToEightBit c.g,
                floatColorToEightBit c.b)

        static member FromGdiColor (c:System.Drawing.Color) : RgbColor =
            RgbColor(
                EightBitToFloatColor c.R,
                EightBitToFloatColor c.G,
                EightBitToFloatColor c.B)
        static member fromHex =
            System.Drawing.ColorTranslator.FromHtml
            >> RgbColor.FromGdiColor
    end

type HsbColor =
    struct
        val h:float
        val s:float
        val b:float
        new(h, s, b) = { h = h; s = s; b = b; }

        static member create (h, s, b) = HsbColor(h, s, b)
        static member toRgb (color:HsbColor) : RgbColor =
            let c = color.b * color.s
            let h' = ((wrap color.h 1.0) * 360.0) / 60.0
            let x = c * (1.0 - (abs ((h' % 2.0) - 1.0)))

            let (r', g', b') =
                if h' < 1.0 then (c, x, 0.0)
                else if h' < 2.0 then (x, c, 0.0)
                else if h' < 3.0 then (0.0, c, x)
                else if h' < 4.0 then (0.0, x, c)
                else if h' < 5.0 then (x, 0.0, c)
                else (c, 0.0, x)

            let m = color.b - c

            RgbColor(r' + m, g' + m, b' + m)
    end
