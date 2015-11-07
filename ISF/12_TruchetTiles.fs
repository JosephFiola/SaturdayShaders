// SaturdayShader Week 12 : Truchet Tiles
// by Joseph Fiola (http://www.joefiola.com)
// 2015-11-07

// Based on Patricio Gonzalez Vivo's "Truchet Tiles" example
// on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/ @patriciogv ( patriciogonzalezvivo.com ) - 2015

/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Tiles"
			],
	"INPUTS": [
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
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "patternZoom",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "scalePos",
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
		},
		{
			"NAME": "tiles",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 20.0
		}
		
	]
}*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265358979323846

vec2 rotate2D (vec2 _st, float _angle) {
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}

vec2 tile (vec2 _st, float _zoom) {
    _st *= _zoom;
    return fract(_st);
}

vec2 rotateTilePattern(vec2 _st){

    //  Scale the coordinate system by 2x2 
    _st -=scalePos;
    _st *= scale;
	_st += scalePos;

    // Give each cell an index number
    // according to its position
    float index = 0.0;    
    index += step(1., mod(_st.x,2.0));
    index += step(1., mod(_st.y,2.0))*2.0;

    // Make each cell between 0.0 - 1.0
    _st = fract(_st);

    // Rotate each cell according to the index
    if(index == 1.0){
        //  Rotate cell 1 by 90 degrees
        _st = rotate2D(_st,PI*0.5);
    } else if(index == 2.0){
        //  Rotate cell 2 by -90 degrees
        _st = rotate2D(_st,PI*-0.5);
    } else if(index == 3.0){
        //  Rotate cell 3 by 180 degrees
        _st = rotate2D(_st,PI);
    }

    return _st;
}

void main (void) {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    
    st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;

    st = tile(st,tiles);
    st = rotateTilePattern(st);

    // Make more interesting combinations
     st = rotate2D(st,-PI*rotate*2.);
     st = rotateTilePattern(st*patternZoom);

    gl_FragColor = vec4(vec3(step(st.x,st.y)),1.0);
}