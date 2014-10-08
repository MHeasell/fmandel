module Vector2

let vMap f (x:float, y:float) =
    (f x, f y)

let vMap2 f (x1:float, y1:float) (x2:float, y2:float) =
    (f x1 x2, f y1 y2)

let vReduce f (x:float, y:float) =
    f x y

let add = vMap2 (+)
let sub = vMap2 (-)
let mul = vMap2 (*)
let div = vMap2 (/)
let scale s = vMap ((*) s)

let sum = vReduce (+)

let rotate rads v =
    let cosTheta = cos rads
    let sinTheta = sin rads

    let x' = sum (mul (cosTheta, -sinTheta) v)
    let y' = sum (mul (sinTheta, cosTheta) v)
    (x', y')