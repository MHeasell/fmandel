module Mandelbrot

open System.Numerics

let juliaIterate c z = (z * z) + c

let rec juliaEscapeBoth2 (radius:float) (maxIterations:int) (c:Complex) (z:Complex) curIteration =
    if z.Magnitude > radius then Some (curIteration, z)
    else if curIteration >= maxIterations then None
    else juliaEscapeBoth2 radius maxIterations c (z*z+c) (curIteration + 1)

let mandelEscapeBoth radius maxIterations z =
    juliaEscapeBoth2 radius maxIterations z z 0

let renormalizeCount (n:int, zn:Complex) =
    let mu = (log ((log zn.Magnitude) / (log 2.0))) / (log 2.0)
    (float n) + 1.0 - mu

let mandelEscapeIterationsFraction radius maxIterations z =
    let renormalize (n, nz) =
        renormalizeCount (n + 2, nz |> juliaIterate z |> juliaIterate z)

    mandelEscapeBoth radius maxIterations z |> Option.map renormalize
