// #SaturdayShader Week 5 : PetalBlob
// by Joseph Fiola (http://www.joefiola.com)
// 2015-09-12
// Based on Polar Shapes example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/07/


/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Blobs", "Shapes"
	],
	"INPUTS": [
		
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
				{
			"NAME": "inner",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "outer",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
				{
			"NAME": "sinValue",
			"TYPE": "float",
			"DEFAULT": -50.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"NAME": "cosValue",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"NAME": "fade",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "location",
			"TYPE": "point2D",
			"DEFAULT": [
				0,
				0
			],
			"MIN": [
				0,
				0
			],
			"MAX": [
				1,
				1
			]
		},
				{
			"NAME": "pinchPoint",
			"TYPE": "point2D",
			"DEFAULT": [
				0,
				0
			],
			"MIN": [
				-1,
				-1
			],
			"MAX": [
				1,
				1
			]
		}
		]
}
*/

#ifdef GL_ES
precision mediump float;
#endif


void main(){
    vec2 st = gl_FragCoord.xy/RENDERSIZE;
    vec3 color = vec3(0.0);
    
    vec2 pos = location - st; ///RENDERSIZE-st;

    float r = length(pos)*(radius); // radius
    float a = atan(pos.y+pinchPoint.y,pos.x+pinchPoint.x); 
    
    
    float mSpeed = speed * TIME;

    float f = abs(cos(a*cosValue + mSpeed)*sin(a*sinValue+mSpeed)) *outer+inner;
 

    color = vec3( 1.-smoothstep(f,f+fade,r) );
    gl_FragColor = vec4(color, 1.0);
}