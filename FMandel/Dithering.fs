module Dithering

open Color
open Util

let closestColor = RgbColor.map round

let dither (width:int) (pixels:RgbColor[]) =
    let indexFunc = indexToCoord width
    let coordFunc = coordToIndex width
    let height = pixels.Length / width

    for i in 0..pixels.Length-1 do
        let (x, y) = indexFunc i
        let oldPixel = pixels.[i]
        let newPixel = closestColor oldPixel
        pixels.[i] <- newPixel
        let quantError = oldPixel - newPixel

        if x+1 < width then
            pixels.[coordFunc (x+1)  y   ] <- pixels.[coordFunc (x+1)  y   ] + quantError * (7.0/16.0)

        if y+1 < height then
            if x > 0 then
                pixels.[coordFunc (x-1) (y+1)] <- pixels.[coordFunc (x-1) (y+1)] + quantError * (3.0/16.0)

            pixels.[coordFunc  x    (y+1)] <- pixels.[coordFunc  x    (y+1)] + quantError * (5.0/16.0)

            if x+1 < width then
                pixels.[coordFunc (x+1) (y+1)] <- pixels.[coordFunc (x+1) (y+1)] + quantError * (1.0/16.0)

    pixels