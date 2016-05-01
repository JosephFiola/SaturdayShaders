// SaturdayShader Week 37 : CircleSpiral
// by Joseph Fiola (http://www.joefiola.com)
// 2016-04-30

// Based on "CircleSpiral" Shadertoy by mmalex
// https://www.shadertoy.com/view/4sBGRh

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
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 0.5,
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
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 15.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 3.0
		},
		{
			"NAME": "lineThickness",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [0.5,0.5],
			"MIN":[0.0,0.0],
			"MAX":[1.0,1.0]
		},
		{
			"NAME": "mode",
			"TYPE": "long",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"spiral",
				"wave"
			],
			"DEFAULT": 0
		}
	]
}*/


#define TWO_PI 6.28318530718

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main()
{
	float res=min(RENDERSIZE.x,RENDERSIZE.y);
	float pixel=1.0/res;
	vec2 p = (gl_FragCoord.xy-RENDERSIZE.xy * vec2(pos)) * pixel;
	p = rotate2d(rotate* -TWO_PI) * p;
	p *=zoom;

	float ink=0.0,theta=0.0;
	float rr=res;
	float ofs=0.0001*(TIME + 10.)  * offset + pixel * 0.25;

	for (int iter=0;iter<100;++iter) {
		ink +=  max(0.0, lineThickness - abs(length(p) - radius) * rr);
		rr /= 1.1; // center glow
		p *= 1.1;
		p.x += ofs * sin(theta);
		if (mode == 0) p.y += ofs * cos(theta);		
		if (mode == 1) p.y += ofs * sin(theta);
		
		theta += TIME * speed;
	}
	
	ink=sqrt(ink)*0.5; // line blur
	
	if  (invert) ink = ink *-1.0 + 1.0;

	gl_FragColor = vec4(vec3(ink),1.0);

}