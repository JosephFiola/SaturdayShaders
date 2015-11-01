// SaturdayShader Week 11 : Bricked
// by Joseph Fiola (http://www.joefiola.com)
// 2015-10-31

// Based on Patricio Gonzalez Vivo's "Apply Matrices Inside Patterns" example
// on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/ @patriciogv ( patriciogonzalezvivo.com ) - 2015




/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Squares", "Warp"
			],
	"INPUTS": [
		
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "tile",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "strokeWidth",
			"TYPE": "float",
			"DEFAULT": 0.9,
			"MIN": 0.0,
			"MAX": 0.99
		},
		{
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 1e-4,
			"MIN": 1e-4,
			"MAX": 0.5
		},
		{
			"NAME": "evenRows",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "oddRows",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 5.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "warp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 2.0
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
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}


vec2 brickTile(vec2 _st, float _zoom){
    _st *= _zoom;

    // Here is where the offset is happening
    _st.x += step(evenRows, mod(_st.y,oddRows)) * offset;

    return fract(_st);
}

float box(vec2 _st, vec2 _size){
    _size = vec2(0.5)-_size*0.5;
    vec2 uv = smoothstep(_size,_size+vec2(gradient),_st);
    uv *= smoothstep(_size,_size+vec2(gradient),vec2(1.0)-_st);
    return uv.x*uv.y;
}

void main(void){
    vec2 st = gl_FragCoord.xy/RENDERSIZE;
    vec3 color = vec3(0.0);
    
 	st *= rotate2D(st,PI*warp);
    
    st -= vec2(pos);						// move the origin - vec2(0.5) will center it
	st.x *= RENDERSIZE.x/RENDERSIZE.y;		// make things 1:1 ratio regardless of canvas ratio

    // Apply the brick tiling
    st = brickTile(st,tile);
    
    st = rotate2D(st,PI*rotate);
    
    color = vec3(box(st,vec2(strokeWidth)));
    
    // invert colors
    if (invert)
    {color = vec3(color *-1.0 + 1.0);}

    gl_FragColor = vec4(color,1.0);    
}