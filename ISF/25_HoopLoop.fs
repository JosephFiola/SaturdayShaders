// SaturdayShader Week 25 : HoopLoop
// by Joseph Fiola (http://www.joefiola.com)
// 2016-02-06


/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
			{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "animate",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": -3.14159265358979323846,
			"MAX": 3.14159265358979323846
		},
		{
			"NAME": "size",
			"TYPE": "float",
			"DEFAULT": 0.35,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "thickness",
			"TYPE": "float",
			"DEFAULT": 0.001,
			"MIN": 0.001,
			"MAX": 0.5
		},
		{
			"NAME": "lineEffect",
			"TYPE": "float",
			"DEFAULT": 0.001,
			"MIN": 0.0,
			"MAX": 0.2
		},
		{
			"NAME": "patternOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "rSin",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -5.0,
			"MAX": 5.0
		},		
		{
			"NAME": "xCos",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "ySin",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -5.0,
			"MAX": 5.0
		},
		{
			"NAME": "blur",
			"TYPE": "float",
			"DEFAULT": 0.005,
			"MIN": 0.001,
			"MAX": 0.5
		},
		{
			"NAME": "function",
			"TYPE": "long",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"abs",
				"fract"
			],
			"DEFAULT": 0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
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


const int   NUM_CIRCLES 	= 50;

#define PI 3.14159265358979323846
#define TWO_PI 6.28318530718

vec3 drawCircle(vec2 p, vec2 center, float radius, float edgeWidth, vec3 color)
{
    float dist = length(p - center);
    vec3 ret;
    
    float look;
    if (function == 0) look = abs(dist -size);
    else if (function == 1) look = fract(dist -size);

	ret = color * (1.0 - lineEffect - smoothstep(radius, (radius+edgeWidth),  look  ));
   
    return ret;
} 

vec3 invertColor(vec3 color) {
    return vec3(color *-1.0 + 1.0);
}

//rotation function
vec2 rot(vec2 uv,float a){
	return vec2(uv.x*cos(a)-uv.y*sin(a),uv.y*cos(a)+uv.x*sin(a));
}


void main()
{

	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos);
	uv.x*=RENDERSIZE.x/RENDERSIZE.y;
	uv *= zoom;
	
	uv=rot(uv,rotate * PI);
	
    
	vec3 color = vec3(0.0);
	float angleIncrement = TWO_PI / float(NUM_CIRCLES);


	for (int i = 0; i < NUM_CIRCLES; ++i) {
		float t = angleIncrement*(float(i));
		float r = sin(rSin * t + animate);
		vec2 p = vec2(r*cos(t*xCos), r*sin(t*ySin));
		
		uv=rot(uv,patternOffset * PI);
		
        if (lineEffect >= 0.2) color = invertColor(color);
       	
       	color += drawCircle(uv, p, thickness, blur, vec3(1.0));
	}
	
	if (invert) color = invertColor(color);

	gl_FragColor = vec4(color,1.0);
}