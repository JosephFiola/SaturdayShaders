// SaturdayShader Week 17 : HypnoCircles
// by Joseph Fiola (http://www.joefiola.com)
// 2015-12-12
// Based on "Circles" by Michael Feldstein - http://www.interactiveshaderformat.com/sketches/519

/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
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
			"NAME": "zoom",
			"TYPE": "float",
			"MIN": 0.1,
			"MAX": 25,
			"DEFAULT": 6
		},
				{
			"NAME": "rotate",
			"TYPE": "float",
			"MIN": 0.0,
			"MAX": 2,
			"DEFAULT": 0.25
		},
		{
			"NAME": "lineSpeed",
			"TYPE": "float",
			"MIN": -50,
			"MAX": 50,
			"DEFAULT": 1.0
		},
		{
			"NAME": "lineAmount",
			"TYPE": "float",
			"MIN": 1.0,
			"MAX": 1000,
			"DEFAULT": 50.0
		},
		{
			"NAME": "lineThickness",
			"TYPE": "float",
			"MIN": 0.01,
			"MAX": 1.999,
			"DEFAULT": 1.0
		}
	]
}*/



#define PI 3.14159265358979323846




//rotate function taken from Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/
vec2 rotate2D(vec2 _st, float _angle){
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    return _st;
}

void main() {

		vec2 st = gl_FragCoord.xy / RENDERSIZE;
		st -= vec2(pos);
		st.x *= RENDERSIZE.x/RENDERSIZE.y; // 1:1 ratio
		
		st = rotate2D(st,PI*-rotate);

		
		st *= zoom;
		st = fract(st);
		float v = distance(st, vec2(0.5));
		v = floor(sin(v * lineAmount + TIME * -lineSpeed) + lineThickness);
		gl_FragColor = vec4(v);
	
	
}