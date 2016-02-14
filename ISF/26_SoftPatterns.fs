// SaturdayShader Week 26 : Soft Patterns
// by Joseph Fiola (http://www.joefiola.com)
// 2016-02-13

// Based on Interferance, Color Waves by @gabrieldunne
// https://twitter.com/gabrieldunne/status/671398225593561090
// http://glslsandbox.com/e#29006.1



/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "iterations",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "contrast",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -20.0,
			"MAX": 20.0
		},
		{
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "pattern",
			"TYPE": "float",
			"DEFAULT": 1.1,
			"MIN": 1e-4,
			"MAX": 10.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
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



#define PI 3.14159
#define TWO_PI (PI*2.0)


vec2 rot(vec2 uv,float a){
	return vec2(uv.x*cos(a) -uv.y*sin(a),uv.y*cos(a)+uv.x*sin(a));
}


void main() 
{
	vec2 center = (gl_FragCoord.xy);
	
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv *= zoom;
	uv=rot(uv,rotate * PI);

	float col = contrast;

	for(float i = 0.0; i < 50.0; i++) 
	{
	  	float a = i * (TWO_PI / pattern);
		col += cos(TWO_PI*(uv.y * cos(a) + uv.x * sin(a) + offset));
		
		if (i >= iterations) break;
		
	}
	
	gl_FragColor = vec4(col*1.0, col*1.0,col*1.0, 1.0);
}