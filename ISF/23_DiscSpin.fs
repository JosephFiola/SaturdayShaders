// SaturdayShader Week 23 : Discspin
// by Joseph Fiola (http://www.joefiola.com)
// 2016-01-23

// Based on "The Power of Sin" by antonOTI - https://www.shadertoy.com/view/XdlSzB



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator"
	],
	"INPUTS": [
			{
			"NAME": "mirror",
			"TYPE": "bool"
		},
		{
			"NAME": "pattern",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "iteration",
			"TYPE": "float",
			"DEFAULT": 35.0,
			"MIN": 0.0,
			"MAX": 35.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -10.0,
			"MAX": 10.0
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 0.8,
			"MIN": 0.0,
			"MAX": 2.0
		},
			{
			"NAME": "centerRadius",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "lineThickness",
			"TYPE": "float",
			"DEFAULT": 0.07,
			"MIN": 0.01,
			"MAX": 1.0
		},
		{
			"NAME": "smoothEdge",
			"TYPE": "float",
			"DEFAULT": 0.03,
			"MIN": 0.01,
			"MAX": 1.0
		},
		{
			"NAME": "yOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "xOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "startValue",
			"TYPE": "float",
			"DEFAULT": -1.5,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "endValue",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": -2.0,
			"MAX": 2.0
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


#define NB 35.
#define MODE1
#define PI 3.14159265358979323846

float circle(vec2 center , float radius,float thickness,float la,float ha)
{
	float f = length(center);
	
	float a = atan(center.y,center.x) ;
	return(smoothstep(f,f+0.01,radius) * smoothstep(radius - thickness,radius - thickness+0.01,f) * step(la,a) *smoothstep(a-smoothEdge,a+smoothEdge,ha));
}

float cable(vec2 p,float dx,float dy,float r,float thick,float la,float ha)
{
	p.x-= dx;
	p.y -= dy;
	return (circle(p,r,thick,la,ha));
}

//rotation function
vec2 rot(vec2 uv,float a){
	return vec2(uv.x*cos(a)-uv.y*sin(a),uv.y*cos(a)+uv.x*sin(a));
}

void main()
{
	
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos - 0.5);

	vec2 p = -1. + 2. * uv;
	p.x*=RENDERSIZE.x/RENDERSIZE.y;
	
	p=rot(p,rotate * PI);

	
	
	vec2 ap = vec2(0.0);
	if (mirror){
		ap = p * vec2(1.,-1.);
	} else {
		ap = p * vec2(-1.,-1.);
	}



	float f = 0.;
	for(float i = 0.; i < NB; ++i)
	{
		if (i > iteration) break;
		
		if (pattern) f *= -1.; // invert values every iteration to create interesting patterns when line thickness overlaps
		
		float divi = i/iteration * centerRadius;
		f += cable(p,xOffset,yOffset,radius - divi,lineThickness,0.,(sin(TIME * speed - divi*5.)*startValue+endValue) * 3.14);
		f += cable(ap,xOffset,yOffset,radius - divi,lineThickness,0.,(sin(TIME * speed - divi*5.)*startValue+endValue) * 3.14);
	}
	vec3 col = mix(vec3(0.,0.,0.),vec3(1.,1.,1.),f);
	
	gl_FragColor = vec4(col,1.0);
}

