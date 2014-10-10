FMandel
=======

A mandelbrot set renderer written in F#.

FMandel outputs images of the Mandelbrot set
similar to those seen on Wikipedia.
It uses a fixed colour palette
and renders the set with smooth "renormalized" shading.

FMandel supports rendering with arbitrary position, rotation and level of detail
and supports anti-aliasing using grid-based sub-sampling.
FMandel also applies Floyd-Steinberg dithering
to remove banding effects on subtle gradients.

Usage
-----

    fmandel [OPTIONS...] [filename]

If no filenmae is specified, the image is output to out.png
in the current working directory.

A number of options are supported:

Short | Long             | Default | Description
------|------------------|---------|------------
-x,   | --camera-x       | 0       | Set the camera X position.
-y,   | --camera-y       | 0       | Set the camera Y position.
-f,   | --frame-height   | 1       | Set the height of the frame from the centre to the top or bottom.
-r,   | --rotation-angle | 0       | Set the camera rotation angle in degrees. The camera rotates anti-clockwise, causing the image to rotate clockwise.
-s,   | --sample-level   | 1       | Set the sample grid density. A value of 2 would take 2x2 (4) samples per pixel.
-i,   | --max-iterations | 100     | Set the maximum number of Mandelbrot iterations per pixel. A higher value increases detail.
-w,   | --width          | 512     | Set the width of the image in pixels.
-h,   | --height         | 512     | Set the height of the image in pixels.
-d,   | --escape-radius  | 2000    | Set the Mandelbrot escape radius. Points leaving this radius are considered to have "escaped" from the set.
      | --help           |         | Display the help screen.

Examples
--------

    fmandel -x -0.7 -f 1.15 -w 800 -h 600 -s 4 -i 1000

![example-1](examples/1.png?raw=true)

    fmandel -x -0.65 -y 0.45 -f 0.01 -w 800 -h 600 -s 4 -i 1000

![example-2](examples/2.png?raw=true)

    fmandel -x -0.65 -y 0.45 -f 0.00005 -w 800 -h 600 -s 4 -i 10000

![example-3](examples/3.png?raw=true)
