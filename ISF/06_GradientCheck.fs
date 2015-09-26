// #SaturdayShader Week 6 : GradientCheck
// by Joseph Fiola (http://www.joefiola.com)
// 2015-09-26



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Gradients", "lines", "Shapes"
	],
	"INPUTS": [
			{
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "gradientPosition",
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
			"NAME": "whiteframeTopRight",
			"TYPE": "point2D",
			"DEFAULT": [
				0.8,
				0.8
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
			"NAME": "whiteframeBottomLeft",
			"TYPE": "point2D",
			"DEFAULT": [
				0.2,
				0.2
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
			"NAME": "blackframeTopRight",
			"TYPE": "point2D",
			"DEFAULT": [
				0.9,
				0.9
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
			"NAME": "blackframeBottomLeft",
			"TYPE": "point2D",
			"DEFAULT": [
				0.1,
				0.1
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
			"NAME": "checkerframeTopRight",
			"TYPE": "point2D",
			"DEFAULT": [
				0.7,
				0.7
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
			"NAME": "checkerframeBottomLeft",
			"TYPE": "point2D",
			"DEFAULT": [
				0.3,
				0.3
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
			"NAME": "checkerColorLeft",
			"TYPE": "float",
			"DEFAULT": 0.8,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "checkerColorRight",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		}
	]
}*/


#ifdef GL_ES
precision mediump float;
#endif



void main(){
	vec2 st = gl_FragCoord.xy/RENDERSIZE;
    float pct = 0.0;
    
   
    
    // draw gradient based on distance
    pct = distance(st,vec2(gradientPosition)) + gradient;
    

    
    //draw inner white frame
    //bottom-left
    vec2 bl = step(vec2(whiteframeBottomLeft),st);
    pct /= bl.x * bl.y;
    // top-right 
    vec2 tr = step(vec2(whiteframeTopRight * -1.0 + 1.0),1.0-st);
    pct /= tr.x * tr.y;
    
    //draw black frame
    //bottom-left
    bl = step(vec2(blackframeBottomLeft),st);
    pct *= bl.x * bl.y;
    // top-right 
    tr = step(vec2(blackframeTopRight * -1.0 + 1.0),1.0-st);
    pct *= tr.x * tr.y;
    
    
    //draw checker frame
    //bottom-left
    bl = step(vec2(checkerframeBottomLeft),st);
    bl += checkerColorLeft;
    pct *= bl.x * bl.y;
    // top-right 
    tr = step(vec2(checkerframeTopRight * -1.0 + 1.0),1.0-st);
    tr += checkerColorRight;
    pct *= tr.x * tr.y;
    
    

    vec3 color = vec3(pct);
    
    // invert colors
    if (invert)
    {color = vec3(pct *-1.0 + 1.0);}

	gl_FragColor = vec4( color, 1.0 );
}