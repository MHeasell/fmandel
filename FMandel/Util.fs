module Util

let wrap a b : float = ((a % b) + b) % b

let log2 x : float = (log x)/(log 2.0)

let logp p x = (log x)/(log p)

let clamp low high v = (min high (max low v))

let toFloatTuple2 (x, y) = (float x, float y)
let toFloatTuple3 (x, y, z) = (float x, float y, float z)

let mapTuple3 f (a, b, c) = f a, f b, f c

let indexToCoord width i =
    (i % width, i / width)

let coordToIndex width x y =
    (width * y) + x

let degToRad i =
    (i / 360.0) * (System.Math.PI * 2.0)
