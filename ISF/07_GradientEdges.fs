// #SaturdayShader Week 7 : GradientEdges
// by Joseph Fiola (http://www.joefiola.com)
// 2015-10-03
// Based on Combining Powers example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/07/




/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Gradients", "lines", "Shapes"
	],
	"INPUTS": [
		{
			"NAME": "sides",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
				{
			"NAME": "gradientRadius",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "teeth",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "warpX",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "warpY",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "warpPosition",
			"TYPE": "point2D",
			"DEFAULT": [
				0.0,
				0.0
			],
			"MIN": [
				-1.0,
				-1.0
			],
			"MAX": [
				1.0,
				1.0
			]
		},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [
				1.0,
				1.0
			],
			"MIN": [
				0.0,
				0.0
			],
			"MAX": [
				2.0,
				2.0
			]
		}
	]
}*/

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359
#define TWO_PI 6.28318530718


// Reference to
// http://thndl.com/square-shaped-shaders.html

void main(){
  vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
//  st.x *= RENDERSIZE.x/RENDERSIZE.y;
  vec3 color = vec3(0.0);
  float d = 0.0;

  // Remap the space to -1. to 1.
  st = st *2.-pos;

  // Number of sides of your shape
  int N = int(sides);

  // Angle and radius from the current pixel
  float a = atan(st.x*warpX,st.y*warpY)+PI;
  float r = TWO_PI/float(N);
  

  // Shaping function that modulate the distance
  d = cos(floor(teeth+a/r)*r-a)*length(st+warpPosition *-1.);

  color = vec3(1.0-smoothstep(-gradient+gradientRadius,gradient+gradientRadius,d));
  // color = vec3(d);

  gl_FragColor = vec4(color,1.0);
}