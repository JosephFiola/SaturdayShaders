// SaturdayShader Week 20 : Fractal Dots
// by Joseph Fiola (http://www.joefiola.com)
// 2016-01-02

// Based on "Basic Fractal" by @paulofalcao found at https://www.shadertoy.com/view/Mss3Wf



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator"
	],
	"INPUTS": [
	
	
		{
			"NAME": "iteration",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
				{
			"NAME": "complexity",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 4.0
		},

		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 40.0,
			"MIN": 0.0,
			"MAX": 200.0
		},
		{
			"NAME": "pattern",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "spacing",
			"TYPE": "float",
			"DEFAULT": 15.0,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "rotate1",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.57079632679
		},
		{
			"NAME": "rotate2",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.57079632679
		},
		{
			"NAME": "dotSize",
			"TYPE": "float",
			"DEFAULT": 500.0,
			"MIN": 0.0,
			"MAX": 4000.0
		},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			],
			"MIN": [
				0.0,
				0.0
			],
			"MAX": [
				1.0,
				1.0
			]
		}
	]
}*/

#define HALF_PI 1.57079632679

const int maxIterations=10; //a nice value for fullscreen is 8

float circleSize=dotSize/(3.0*pow(2.0,float(maxIterations)));

//generic rotation formula
vec2 rot(vec2 uv,float a){

	return vec2(uv.x*cos(a)-uv.y*sin(a),uv.y*cos(a)+uv.x*sin(a));

}

void main(){

vec2 uv = gl_FragCoord.xy/RENDERSIZE;
uv -= vec2(pos);						
uv.x *= RENDERSIZE.x/RENDERSIZE.y;


	//global rotation and zoom
	uv=rot(uv,rotate1);
	uv *= zoom;

	
	//mirror, rotate and scale 6 times...
	float s=spacing;
	for(int i=0;i<maxIterations;i++){
		//uv=floor(abs(uv)-s);
		uv=abs(uv)-s;
		uv -= complexity;
		uv=rot(uv,rotate2);
		//uv += 0.5;
		s=s/pattern;
		if (int(iteration) < i) 
		break;
	}
	
	//draw a circle
	float c=length(uv)>circleSize?0.0:1.0;	

	gl_FragColor = vec4(c,c,c,1.0);
}