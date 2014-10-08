module Options

open CommandLine
open System.Collections.Generic

type ProgramOptions() =
    [<Option('x', "camera-x", DefaultValue=0.0, HelpText="Set the camera X position.")>]
    member val CameraX = 0.0 with get, set

    [<Option('y', "camera-y", DefaultValue=0.0, HelpText="Set the camera Y position.")>]
    member val CameraY = 0.0 with get, set

    [<Option('f', "frame-height", DefaultValue=1.0, HelpText="Set the height of the frame from the centre to the top or bottom.")>]
    member val FrameHeight = 1.0 with get, set

    [<Option('r', "rotation-angle", DefaultValue=0.0, HelpText="Set the camera rotation angle in degrees. The camera rotates anti-clockwise, causing the image to rotate clockwise.")>]
    member val RotationAngle = 0.0 with get, set

    [<Option('s', "sample-level", DefaultValue=1, HelpText="Set the sample grid density. A value of 2 would take 2x2 (4) samples per pixel.")>]
    member val SubSamples = 1 with get, set

    [<Option('i', "max-iterations", DefaultValue=100, HelpText="Set the maximum number of Mandelbrot iterations per pixel. A higher value increases detail.")>]
    member val Iterations = 100 with get, set

    [<Option('w', "width", DefaultValue=512, HelpText="Set the width of the image in pixels.")>]
    member val Width = 512 with get, set

    [<Option('h', "height", DefaultValue=512, HelpText="Set the height of the image in pixels.")>]
    member val Height = 512 with get, set

    [<Option('d', "escape-radius", DefaultValue=2000.0, HelpText="Set the Mandelbrot escape radius. Points leaving this radius are considered to have \"escaped\" from the set.")>]
    member val EscapeRadius = 2000.0 with get, set

    [<ValueList(typeof<List<string>>)>]
    member val Items : IList<string> = null with get, set

    [<HelpOption>]
    member this.GetUsage() : string =
        let helpText = Text.HelpText()
        helpText.AddDashesToOption <- true

        helpText.Heading <- Text.HeadingInfo("FMandel", AssemblyInfo.InfoVersion).ToString()

        helpText.AddPreOptionsLine("FMandel is a Mandelbrot set renderer written in F# by Michael Heasell.")
        helpText.AddPreOptionsLine("")
        helpText.AddPreOptionsLine("Usage: FMandel [OPTIONS...] [filename]")
        helpText.AddPreOptionsLine("")
        helpText.AddPreOptionsLine("If no filename is specified, the image is output to out.png in the current working directory.")
        helpText.AdditionalNewLineAfterOption <- true
        helpText.AddOptions(this)
        helpText.ToString()
