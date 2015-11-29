// SaturdayShader Week 15 : GOTO 10
// by Joseph Fiola (http://www.joefiola.com)
// 2015-11-28
// Based on Patricio Gonzalez Vivo's "GOTO 10 Maze" example on http://patriciogonzalezvivo.com/2015/thebookofshaders/10/ @patriciogv ( patriciogonzalezvivo.com ) - 2015


/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [ "Generator" ],
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
			"NAME": "invert",
			"TYPE": "bool"
		},
		{
			"NAME": "pattern",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2

			],
			"LABELS": [
				"Maze",
				"Circle",
				"Both"
		
			],
			"DEFAULT": 0
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.5,
			"MAX": 100.0
		},
		{
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.01,
			"MAX": 2.0
		},
		{
			"NAME": "gradientOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "truchetPositionOffset",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 1e-4,
			"MAX": 2.0
		},
		{
			"NAME": "truchetIndex",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "contrast",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "contrastShift",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.5,
			"MAX": 0.5
		},
		{
			"NAME": "randomXspeed",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "randomYspeed",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		}
	]
}*/


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265358979323846


float random (in vec2 _st) { 
    return fract(sin(dot(_st.xy,
                         vec2((TIME*randomXspeed),(TIME*randomYspeed)))));
}

vec2 truchetPattern(in vec2 _st, in float _index){
    _index = fract(((_index-0.5)*truchetIndex));
    if (_index > 0.75*truchetIndex) {
        _st = vec2(truchetPositionOffset) - _st;
    } else if (_index > 0.5 * truchetIndex) {
        _st = vec2(truchetPositionOffset-_st.x,_st.y);
    } else if (_index > 0.25 * truchetIndex) {
        _st = 1.-vec2(truchetPositionOffset-_st.x,_st.y);
    } else if (_index >= 0.0) {
    	_st += truchetPositionOffset;
    }
    return _st;
}

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y; // 1:1 ratio
    
    st *= zoom;

    vec2 ipos = floor(st);  // integer
    vec2 fpos = fract(st);  // fraction

    vec2 tile = truchetPattern(fpos, random( ipos ));

    float color = 0.0;

    // Maze
    if (pattern == 0 || pattern == 2){
    color += smoothstep(tile.x-gradient+gradientOffset,tile.x,tile.y)-
            smoothstep(tile.x,tile.x+gradient+gradientOffset,tile.y);
    }

    // Circles
    if (pattern == 1 || pattern == 2){
     color += (step(length(tile),0.6) -
             step(length(tile),0.4) ) +
             (step(length(tile-vec2(1.)),0.6) -
              step(length(tile-vec2(1.)),0.4) );
    }
    
    //adjust contrast
	color += smoothstep(0.0+contrast+contrastShift,1.0-contrast+contrastShift, color);
	
	//invert colors
	if (invert) color = color *-1.0 + 1.0;

    gl_FragColor = vec4(vec3(color),1.0);
}



