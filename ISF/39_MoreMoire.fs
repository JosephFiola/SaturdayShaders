// SaturdayShader Week 39 : More Moiré
// by Joseph Fiola (http://www.joefiola.com)
// 2016-05-14

// Based on "Moiré Bounce" Shadertoy by echophon
// https://www.shadertoy.com/view/XlfGDB




/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
	{
		"NAME": "invert",
		"TYPE": "bool"
	},
	{
		"NAME" : "zoom",
		"TYPE" : "float",
		"DEFAULT": 10.0,
		"MIN": 1e-4,
		"MAX": 20.0
	},
		{
		"NAME" : "moire_x",
		"TYPE" : "float",
		"DEFAULT": 10.0,
		"MIN": 1e-4,
		"MAX": 10.0
	},
	{
		"NAME" : "moire_y",
		"TYPE" : "float",
		"DEFAULT": 4.0,
		"MIN": 1e-4,
		"MAX": 10.0
	},
	{
		"NAME" : "shape_x",
		"TYPE" : "float",
		"DEFAULT": 1.005,
		"MIN": 0.0,
		"MAX": 1.0
	},
		{
		"NAME" : "shape_y",
		"TYPE" : "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME" : "mult_x",
		"TYPE" : "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME" : "mult_y",
		"TYPE" : "float",
		"DEFAULT": 2.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 1000.0
	},
		{
		"NAME": "timeOffset",
		"TYPE": "float",
		"DEFAULT": 80.0,
		"MIN": 0.0,
		"MAX": 1000.0
	},
	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "pos",
		"TYPE": "point2D",
		"DEFAULT": [0.5,0.5],
		"MIN":[0.0,0.0],
		"MAX":[1.0,1.0]
		}
	]
}*/

#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718


// Rotate
mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv *= zoom;
	uv = rotate2d(rotate*-TWO_PI) * uv;
	
	float _time = TIME + timeOffset;
	
	float distance = sqrt( pow(abs(uv[0]), shape_x * mult_x) + pow(abs(uv[1]), shape_y * mult_y) + speed);
    float color = sin(distance * _time * moire_x) * cos(distance * _time * moire_y);
 
    if  (invert) color = color *-1.0 + 1.0;
    gl_FragColor = vec4(color, color, color,1.0);}