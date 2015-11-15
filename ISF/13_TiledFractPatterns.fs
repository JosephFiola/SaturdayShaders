// SaturdayShader Week 13 : TiledFractPatterns
// by Joseph Fiola (http://www.joefiola.com)
// 2015-11-14

// Based on Patricio Gonzalez Vivo's "2D Random" example on http://patriciogonzalezvivo.com/2015/thebookofshaders/10/ @patriciogv ( patriciogonzalezvivo.com ) - 2015
// with elements from last week's "Truchet Tiles" added in for the tiled patterns and rotations.

/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Tiles", "Random", "Noise"
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
			"NAME": "ScalePosition",
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
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "Random1",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -100.0,
			"MAX": 100.0
		},
		{
			"NAME": "TileZoom",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.5,
			"MAX": 10.0
		},
		{
			"NAME": "Tiles",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.5,
			"MAX": 10.0
		},
		{
			"NAME": "Random2",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -100.0,
			"MAX": 100.0
		},
		{
			"NAME": "Multiplyer",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -100.0,
			"MAX": 100.0
		},
		{
			"NAME": "Repeat",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -10.0,
			"MAX": 10.0
		},
		{
			"NAME": "Rotate",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -1.0,
			"MAX": 1.0
		}
	]
}*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265358979323846


float random (vec2 st) { 
    return fract(sin(dot(st.xy*Repeat,
                         vec2(Random1,Random2)))* 
        Multiplyer);
}

vec2 tile (vec2 _st, float _zoom) {
    _st *= _zoom;
    return fract(_st);
}

vec2 rotate2D (vec2 _st, float _angle) {
   // _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
   // _st += 0.5;
    return _st;
}


vec2 rotateTilePattern(vec2 _st){

    //  Scale the coordinate system by 2x2 
    _st -=ScalePosition;
    _st *= TileZoom;
	_st += ScalePosition;

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

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
	

	st += tile(st,Tiles);
    st = rotateTilePattern(st);

	st = rotate2D(st,-PI*Rotate*2.);

    float rnd = random( st );
    
    
     

    
     // invert colors
    if (invert)
    {rnd = rnd *-1.0 + 1.0;}

    gl_FragColor = vec4(vec3(rnd),1.0);
}