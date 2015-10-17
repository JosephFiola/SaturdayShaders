// SaturdayShader Week 9 : RotatingCirclePatterns
// by Joseph Fiola (http://www.joefiola.com)
// 2015-10-17
//
// Based on a combination of Patricio Gonzalez Vivo's Patterns examples on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/
// and Matrices examples http://patriciogonzalezvivo.com/2015/thebookofshaders/08/ @patriciogv ( patriciogonzalezvivo.com ) - 2015



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Circles"
			],
	"INPUTS": [
		
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "circulateCanvas",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "circulateCanvasAmplitude",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "circulateTile",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "circulateTileAmplitude",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "tiles",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 1.0,
			"MAX": 20.0
		},
		{
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 0.03,
			"MIN": 0.0,
			"MAX": 50.0
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

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform float u_time;

float circle(in vec2 _st, in float _radius){
    vec2 l = _st-vec2(0.5);
    return 1.-smoothstep(_radius-(_radius*gradient),
                         _radius+(_radius*gradient),
                         dot(l,l)*radius);
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	st -= vec2(pos);						// move the origin - vec2(0.5) to center
	st.x *= RENDERSIZE.x/RENDERSIZE.y;		// make things 1:1 ratio regardless of canvas ratio

    vec3 color = vec3(0.0);					// instantiate color variable
  
    
    //translate canvas
    vec2 translate = vec2(cos(circulateCanvas*PI),sin(circulateCanvas*PI));
    st += translate*circulateCanvasAmplitude;
    
    //rotate canvas
 	st = rotate2d(rotate*PI ) * st;
 
    //tile pattern
    st *= tiles;      // Scale up the space by 3
    st = fract(st); // Wrap arround 1.0
    
    // translate within tile
    translate = vec2(cos(circulateTile),sin(circulateTile));
    st += translate*circulateTileAmplitude;
    

    color = vec3(st,0.0);
    color = vec3(circle(st,0.5));
    
    // invert colors
    if (invert)
    color = vec3(color *-1.0 + 1.0);
    

	gl_FragColor = vec4(color,1.0);
}