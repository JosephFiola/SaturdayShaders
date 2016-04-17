// SaturdayShader Week 35 : Moirée
// by Joseph Fiola (http://www.joefiola.com)
// 2016-04-16

//Based on "Hyper-lightweight2 XOR ..." Shadertoy https://www.shadertoy.com/view/4slXWn by Fabrice NEYRET @FabriceNEYRET 

/*{
	"CREDIT": "",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
			{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT" : "0"
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 0.25,
			"MAX": 10.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "details",
			"TYPE": "float",
			"DEFAULT": 50.0,
			"MIN": 1.0,
			"MAX": 500.0
		},
		{
			"NAME": "amp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
		},
		{
			"NAME": "frequency",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.02,
			"MAX": 50.0
		},
		{
			"NAME": "sharpen",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 4.0
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

#define TWO_PI 6.28318530718

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

float f(float a, float b, vec2 uv) {
    return sin(length( uv -vec2(cos(a) * amp,sin(b) *amp) * frequency) *details) *sharpen;
}

void main() {
	
	float t= TIME * speed;

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
       
   	uv = rotate2d(rotate*-TWO_PI) * uv;
   	uv *= zoom;

  
    float offset = 0.7;
    vec4 color = vec4(f(t,t,uv)*f(t * (offset * 2.0),t * offset, uv));
    
    if  (invert) color = color *-1.0 + 1.0;

    gl_FragColor = vec4(color);
}