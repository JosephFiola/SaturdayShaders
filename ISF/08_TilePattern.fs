// #SaturdayShader Week 7 : GradientEdges
// by Joseph Fiola (http://www.joefiola.com)
// 2015-10-03
// Based on Apply Matrices Inside Patterns example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/
// Author @patriciogv ( patriciogonzalezvivo.com ) - 2015



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Squares"
			],
	"INPUTS": [
		{
			"NAME": "Rotate",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 2.0
		},
				{
			"NAME": "Offset",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "Tiles",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": -20.0,
			"MAX": 20.0
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



#define PI 3.14159265358979323846

vec2 rotate2D(vec2 _st, float _angle){
    _st -= pos;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += Offset;
    return _st;
}

vec2 tile(vec2 _st, float _zoom){
    _st *= _zoom;
    return fract(_st);
}

float box(vec2 _st, vec2 _size, float _smoothEdges){
    _size = vec2(0.5)-_size*0.5;
    vec2 aa = vec2(_smoothEdges*0.5);
    vec2 uv = smoothstep(_size,_size+aa,_st);
    uv *= smoothstep(_size,_size+aa,vec2(2.0)-_st);
    return uv.x*uv.y;
}

void main(void){
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    vec3 color = vec3(0.0);

    // Divide the space in 4
    st = tile(st,Tiles);   

    // Use a matrix to rotate the space 45 degrees
    st = rotate2D(st,PI*Rotate);

    // Draw a square
    color = vec3(box(st,vec2(0.7),0.01));
    // color = vec3(st,0.0);

    gl_FragColor = vec4(color,1.0);    
}