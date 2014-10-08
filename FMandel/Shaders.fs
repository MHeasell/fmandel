module Shaders

open System.Numerics

open Util
open Color
open Easing

let cycleColorArray colors x : RgbColor =
    let len = Array.length colors
    let normX = wrap x (float len)
    let idx = (int normX)
    let frac = normX - (floor normX)

    let col1 = colors.[idx]
    let col2 = colors.[(idx + 1) % len]
    let diff = col2 - col1
    let easeFunc = (fun a c -> easeInOutLinear a c frac)
    RgbColor.map2 easeFunc col1 diff

let mandelbrotShade insideColor outsideColorFunc radius maxIterations (x, y) : RgbColor =
    let iters = Mandelbrot.mandelEscapeBoth radius maxIterations (Complex(x, y))
    match iters with
    | Some(x) -> outsideColorFunc x
    | None -> insideColor

let orangeArr = [|
    RgbColor.fromHex("#07005d")
    RgbColor.fromHex("#111987")
    RgbColor.fromHex("#1e4aac")
    RgbColor.fromHex("#4376cd")
    RgbColor.fromHex("#86afe1")
    RgbColor.fromHex("#d0e8f7")

    RgbColor.fromHex("#ede7be")
    RgbColor.fromHex("#f5c95a")
    RgbColor.fromHex("#fda801")
    RgbColor.fromHex("#c88101")
    RgbColor.fromHex("#945400")
    RgbColor.fromHex("#643101")
    RgbColor.fromHex("#421206")
    RgbColor.fromHex("#0e030e")
    RgbColor.fromHex("#050026")
    RgbColor.fromHex("#050047")
|]

let cycleMandelbrotSmooth =
    let colorFunc =
        Mandelbrot.renormalizeCount
        >> (*) (1.0 / 6.0)
        >> cycleColorArray orangeArr

    mandelbrotShade RgbColor.black colorFunc
