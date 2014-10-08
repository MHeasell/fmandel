module Easing

let easeInOutLinear (a:float) (c:float) (frac:float) : float =
    a + (frac * c)

let easeInOutStep (a:float) (c:float) (frac:float) : float =
    if frac < 0.5 then a else a + c

let easeInOutSine (a:float) (c:float) (frac:float) : float =
    -c / 2.0 * ((cos (System.Math.PI * frac)) - 1.0) + a

let easeInOutQuad (a:float) (c:float) (frac:float) : float =
    let t = 2.0 * frac
    if t < 1.0 then
        a + (c / 2.0 * t * t)
    else
        let t' = t - 1.0
        a + (-c/2.0 * (t'*(t'-2.0)-1.0))
