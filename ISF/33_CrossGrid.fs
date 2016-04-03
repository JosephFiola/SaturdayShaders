// SaturdayShader Week 33 : CrossGrid
// Joseph Fiola (http://www.joefiola.com)
// 2016-04-02

// Based on http://thebookofshaders.com/edit.html#09/cross.frag
// by Patricio Gonzalez Vivo - www.patriciogonzalezvivo.com

/*{
	"CREDIT": "",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 8.0,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 15.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "spin",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "centerOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "verticalHeight",
			"TYPE": "float",
			"DEFAULT": 0.125,
			"MIN": -0.1,
			"MAX": 0.5
		},
				{
			"NAME": "verticalWidth",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -0.1,
			"MAX": 0.5
		},
		{
			"NAME": "horizontalHeight",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -0.1,
			"MAX": 0.5
		},
				{
			"NAME": "horizontalWidth",
			"TYPE": "float",
			"DEFAULT": 0.125,
			"MIN": -0.1,
			"MAX": 0.5
		},
		{
			"NAME": "grid",
			"TYPE": "float",
			"DEFAULT": 5.0,
			"MIN": 0.0,
			"MAX": 100.0
		},
		{
			"NAME": "gridRotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "linesThickness",
			"TYPE": "float",
			"DEFAULT": 0.99,
			"MIN": 0.25,
			"MAX": 1.0
		},
		{
			"NAME": "invert",
			"TYPE": "bool"
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


mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

vec2 tile(vec2 _st, float _zoom){
  _st *= _zoom;
  return fract(_st);
}

float box(in vec2 _st, in vec2 _size){
    _size = vec2(0.5) - _size * 0.5;
    vec2 uv = smoothstep(_size,
                        _size+vec2(0.001),
                        _st);
    uv *= smoothstep(_size,
                    _size+vec2(0.001),
                    vec2(1.0)-_st);
    return uv.x*uv.y;
}

float cross(in vec2 _st, float _size){
    return  box(_st, vec2(_size*verticalWidth,_size*verticalHeight)) + 
            box(_st, vec2(_size*horizontalWidth,_size*horizontalHeight));
}



void main(){
	
	//crosses
	vec2 st = gl_FragCoord.xy / RENDERSIZE.xy;
	st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
	st = rotate2d(rotate*-TWO_PI) * st;
	st = tile(st,zoom);
	st -= 0.5;
	st = rotate2d(spin*-TWO_PI) * st;
	st += centerOffset;
 	vec3 color = vec3( clamp(cross(fract(st),scale),0.0,1.0) );
 	
 	//square grids
 	st = gl_FragCoord.xy / RENDERSIZE.xy;
 	st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
	st = rotate2d(gridRotate*-TWO_PI/2.0) * st;
	st += centerOffset;
	st = tile(st,grid);

	color += vec3(box(st,vec2(linesThickness))) * -1.0 + 1.0;
	
 	if  (invert) color = color *-1.0 + 1.0;


  	gl_FragColor = vec4(color,1.0);
}
