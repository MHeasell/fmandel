// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open System.Drawing
open Color
open Util

let render (f:int*int->RgbColor) (width:int, height:int) =
    let bmp = new Bitmap(width, height)
    let coords = seq {
        for i in 0..height-1 do
            for j in 0..width-1 do
                yield (j,i)
    }

    let coordFunc = indexToCoord width

    let indexShader = coordFunc >> f >> RgbColor.map floatColorToScreen

    let pixels =
        Array.Parallel.init (width * height) indexShader
        |> Dithering.dither width

    let writeFunc idx color =
        let (x, y) = coordFunc idx
        bmp.SetPixel(x, y, (RgbColor.RawToGdiColor color))

    Array.iteri writeFunc pixels
    bmp

let subSample cells i =
    let interval = 1.0 / (float cells)
    let halfInterval = interval/2.0
    seq { (i - 0.5 + halfInterval)..interval..(i + 0.5 - halfInterval) }

let wrapAntiAliasing cells f (x, y) =
    let samp = subSample cells

    let cols = seq {
        for dy in (samp y) do
            for dx in (samp x) do
                yield f (dx, dy) }

    Seq.average cols

let inverseProject (screenWidth, screenHeight) =
    let aspectRatio = screenWidth / screenHeight

    Vector2.mul (1.0/screenWidth, 1.0/screenHeight)
    >> Vector2.add (-0.5, -0.5)
    >> Vector2.mul (2.0, -2.0)
    >> Vector2.mul (aspectRatio, 1.0)

let inverseView cameraPosition rotation scale =
    Vector2.scale scale >> Vector2.rotate rotation >> Vector2.add cameraPosition

[<EntryPoint>]
let main argv =

    let options = Options.ProgramOptions()
    let success = CommandLine.Parser.Default.ParseArguments(argv, options)

    if not success then 1
    else
        try
            printfn "Setting up pipeline..."

            let fname = if options.Items.Count > 0 then options.Items.[0] else "out.png"

            if options.Width < 1 || options.Height < 1 then
                failwith "Screen dimensions invalid."

            let screenSize = (options.Width, options.Height)

            let cameraPosition = (options.CameraX, options.CameraY)

            let cameraRotation = degToRad options.RotationAngle
            let cameraScale = options.FrameHeight

            if options.SubSamples < 1 then
                failwith "Subsample level invalid."

            let subsampleCells = options.SubSamples

            if options.EscapeRadius <= 0.0 then
                failwith "Invalid escape radius."
            let escapeRadius = options.EscapeRadius

            if options.Iterations < 0 then
                failwith "Invalid number of iterations."
            let maxIterations = options.Iterations

            let inverseViewProj =
                inverseProject (toFloatTuple2 screenSize)
                >> inverseView cameraPosition cameraRotation cameraScale

            let coreShader = Shaders.cycleMandelbrotSmooth escapeRadius maxIterations
            let screenShader = inverseViewProj >> coreShader

            let aaShader = wrapAntiAliasing subsampleCells screenShader

            let finalShader =
                toFloatTuple2
                >> Vector2.add (0.5, 0.5)
                >> aaShader

            printfn "Rendering frame..."
            let stopWatch = System.Diagnostics.Stopwatch.StartNew()
            let bmp = render finalShader screenSize
            stopWatch.Stop()
            printfn "Render complete."
            printfn "Elapsed time: %f seconds." stopWatch.Elapsed.TotalSeconds

            printfn "Saving to: %s" fname
            bmp.Save(fname)
            printfn "Render saved."

            0 // return an integer exit code
        with
            | Failure(msg) -> printfn "%s" msg; 1
