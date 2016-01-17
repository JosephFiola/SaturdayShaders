// SaturdayShader Week 22 : Difference Strokes
// by Joseph Fiola (http://www.joefiola.com)
// 2016-01-16

/*{
    "CREDIT": "Joseph Fiola",
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
			"NAME": "difference",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "shape",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3
			],
			"LABELS": [
				"circle solid",
				"circle outlines",
				"rect solid",
				"rect outlines"
			],
			"DEFAULT": 0
		},
		{
			"NAME": "dotSize",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.25,
			"MAX": 4.0
		},
		{
			"NAME": "iteration",
			"TYPE": "float",
			"DEFAULT": 25.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "xAmp",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -0.5,
			"MAX": 0.5
		},
		{
			"NAME": "yAmp",
			"TYPE": "float",
			"DEFAULT": -0.2,
			"MIN": -0.5,
			"MAX": 0.5
		},
		{
			"NAME": "xFactor",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 10.0
		},		
		{
			"NAME": "yFactor",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 0.1
		},
		{
			"NAME": "rotateCanvas",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "rotateParticles",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
			{
			"NAME": "rotateMultiplier",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 0.01,
			"MAX": 10
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


#define PI 3.14159265358979323846
#define TWO_PI 6.28318530718



//rotation function
vec2 rot(vec2 uv,float a){
	return vec2(uv.x*cos(a)-uv.y*sin(a),uv.y*cos(a)+uv.x*sin(a));
}

// circle function by jonobr1 from https://www.shadertoy.com/view/XsjGDt
vec3 circle(vec2 uv, vec2 pos, float rad) {
	float d = length(pos - uv) - rad;
	float t = clamp(d, 0.0, 1.0);
	return vec3(vec3(1.0-t));
}

vec3 rectangle(vec2 uv, vec2 pos, float width, float height) {
	float t = 0.0;
	if ((uv.x > pos.x - width / 2.0) && (uv.x < pos.x + width / 2.0)
		&& (uv.y > pos.y - height / 2.0) && (uv.y < pos.y + height / 2.0)) {
		t = 1.0;
	}
	return vec3(t);
}

 vec3 invertColor(vec3 color) {
    return vec3(color *-1.0 + 1.0);
 }



void main(){

	vec2 uv = gl_FragCoord.xy;
	uv -= vec2(pos * RENDERSIZE);
	
	uv *= zoom;
	
	//rotate canvas
	uv=rot(uv,rotateCanvas * PI);
	
	vec3 color = vec3(0.0);
	
	// prevents background from flashing when +/- iteration value
	if (difference) {
		if (mod(iteration, 2.0) < 1.0) color = invertColor(color);
	}


	float radius = dotSize * RENDERSIZE.x;
	
	for (float i = 0.0; i<=50.0; i++){
		
		// set max number of iterations
		if (iteration < i) break;
		
		vec2 offset = pos;
		offset +=	vec2(	cos( i * xFactor * (TIME * speed)) * (xAmp * RENDERSIZE.x),
							sin( i * yFactor * (TIME * speed)) * (yAmp * RENDERSIZE.y));
						
		radius += i * 0.002;

		//DRAW SHAPES
		//draw circle solid
		if (shape == 0 || shape == 1) color += circle(uv, offset, radius);
		
		//draw circle outline
		if (shape == 1){
			if (difference)		color -= circle(uv, offset, radius + 2.0);
			if (!difference)	color -= circle(uv, offset, radius - 2.0);
		}
		
		//draw rectangle solid
		if (shape == 2 || shape == 3) color += rectangle(uv, offset, radius*2.0, radius*2.0);
	
		//draw rect outline
		if (shape == 3) {
			if (difference)		color -= rectangle(uv, offset, radius*2.0 + 4.0, radius*2.0 + 4.0);
			if (!difference)	color -= rectangle(uv, offset, radius*2.0 - 4.0, radius*2.0 - 4.0);
		}


	if (difference) color = invertColor(color);
		
		//rotate particles
		uv=rot(uv,rotateParticles * PI * rotateMultiplier);

		}
		
	//invert colors
	if (invert) color = invertColor(color);
	
	gl_FragColor = vec4(color, 1.0);
}