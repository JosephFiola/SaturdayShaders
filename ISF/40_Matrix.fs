// SaturdayShader Week 40 : Matrix
// by Joseph Fiola (http://www.joefiola.com)
// 2016-07-09

// Based on "Matrix" Patricio Gonzalez Vivo from the Book of Shaders' Generative Designs Examples
// https://thebookofshaders.com/examples/?chapter=10



/*{
	"CREDIT": "",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
	{
		"NAME": "invert",
		"TYPE": "bool"
	},
	{
		"NAME" : "zoom",
		"TYPE" : "float",
		"DEFAULT": 1.0,
		"MIN": 1e-4,
		"MAX": 40.0
	},
	{
		"NAME" : "grid",
		"TYPE" : "float",
		"DEFAULT": 5.0,
		"MIN": 0.1,
		"MAX": 20.0
	},
	{
		"NAME" : "dotSize",
		"TYPE" : "float",
		"DEFAULT": 0.1,
		"MIN": 0.0,
		"MAX": 0.5
	},
	{
		"NAME" : "xScale",
		"TYPE" : "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 0.49
	},
	{
		"NAME" : "yScale",
		"TYPE" : "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 0.49
	},
	{
		"NAME" : "xRandom",
		"TYPE" : "float",
		"DEFAULT": 12.9898,
		"MIN": 1e-4,
		"MAX": 12.9898
	},
	{
		"NAME" : "yRandom",
		"TYPE" : "float",
		"DEFAULT": 78.233,
		"MIN": 1e-4,
		"MAX": 78.233
	},
	{
		"NAME" : "randomMultiplier",
		"TYPE" : "float",
		"DEFAULT": 43758.5453,
		"MIN": 1e-4,
		"MAX": 43758.5453
	},
	{
		"NAME": "speed",
		"TYPE": "float",
		"DEFAULT": 20.0,
		"MIN": 0.0,
		"MAX": 40.0
	},
	{
		"NAME": "rotate",
		"TYPE": "float",
		"DEFAULT": 0.0,
		"MIN": 0.0,
		"MAX": 1.0
	},
	{
		"NAME": "pos",
		"TYPE": "point2D",
		"DEFAULT": [0.0,0.5],
		"MIN":[0.0,0.0],
		"MAX":[1.0,1.0]
		}
	]
}*/

#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718
                

float random(in float x){ return fract(sin(x)*randomMultiplier); } // original value was 43758.5453
float random(in vec2 st){ return fract(sin(dot(st.xy,vec2(xRandom,yRandom))) * randomMultiplier); }  // 12.9898,	78.233,  & 43758.5453

float randomChar(vec2 outer,vec2 inner){
    float grid = grid;
    vec2 margin = vec2(xScale,yScale);
    vec2 borders = step(margin,inner)*step(margin,1.-inner);
    vec2 ipos = floor(inner*grid);
    vec2 fpos = fract(inner*grid);
    return step(.5,random(outer*64.+ipos)) * borders.x * borders.y * step(dotSize,fpos.x) * step(dotSize,fpos.y);
}

// Rotate
mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main(){
    vec2 st = gl_FragCoord.st/RENDERSIZE.xy;
    st -= vec2(pos); //center uv to pos location
    st.y *= RENDERSIZE.y/RENDERSIZE.x;
   
    st *= zoom;
    st = rotate2d(rotate*-TWO_PI) * st;
    
    vec3 color = vec3(0.0);
   
    vec2 ipos = floor(st);
    vec2 fpos = fract(st);
    
    ipos += vec2(0.,floor(TIME*speed*random(ipos.x+1.)));

    float pct = 1.0;
    pct *= randomChar(ipos,fpos);
    //pct *= random(ipos);

    color = vec3(pct);
    
    if  (invert) color = color *-1.0 + 1.0; //invert colors
    
    gl_FragColor = vec4( color , 1.0);
}