// SaturdayShader Week 38 : SimpleShapeFunctions
// by Joseph Fiola (http://www.joefiola.com)
// 2016-05-07

// Simple shape function based on the first examples in the Book of Shaders by Patricio Gonzalez Vivo
// http://thebookofshaders.com/05/




/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
		"NAME": "function",
		"TYPE": "long",
		"VALUES": [
			0,1,2,3,4
			
		],
		"LABELS": [
			"tan",
			"atan",
			"sin",
			"asin",
			"mod"
			
		],
		"DEFAULT": 0
	},
	{
		"NAME": "invert",
		"TYPE": "bool"
	},
	{
		"NAME" : "zoom",
		"TYPE" : "float",
		"DEFAULT": 10.0,
		"MIN": 1e-4,
		"MAX": 20.0
	},
	{
		"NAME" : "thickness",
		"TYPE" : "float",
		"DEFAULT": 2.0,
		"MIN": 0.0,
		"MAX": 10.0
	},
		{
		"NAME" : "compress",
		"TYPE" : "float",
		"DEFAULT": 1.0,
		"MIN": 1e-4,
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




// Plot a line on Y using a value between 0.0-1.0
float plot(vec2 st, float pct){
  return  smoothstep( pct-thickness, pct, st.y) - 
          smoothstep( pct, pct+thickness, st.y);
}

// Rotate
mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
	st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
	st *= zoom;
	st = rotate2d(rotate*-TWO_PI) * st;

    float p = 0.0;

  	if(function == 0) p = tan(st.x);
   	if(function == 1) p = atan(st.x);
   	if(function == 2) p = sin(st.x);
   	if(function == 3) p = asin(st.x);
   	if(function == 4) p = mod(st.x, 1.0);


    // Plot line
    float pct = plot(st,p*compress);
    
    if  (invert) pct = pct *-1.0 + 1.0;
   
	gl_FragColor = vec4(vec3(pct),1.0);
}