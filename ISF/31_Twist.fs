//Based on "Twist" Shadertoy by fb39ca4 - https://www.shadertoy.com/view/XsXXDH
//Inspired by Matthew DiVito's gifs
//http://cargocollective.com/matthewdivito/Animated-Gifs-02


/*{
	"CREDIT": "Joseph Fiola, Matthew DiVito, Shadertoy user fb39ca4",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "iterations",
			"TYPE": "float",
			"DEFAULT": 30.0,
			"MIN": 0.0,
			"MAX": 60.0
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.125,
			"MAX": 3.0
		},
		{
			"NAME": "twist",
			"TYPE": "float",
			"DEFAULT": 0.51,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "rotation",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "linesOffset",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -10.0,
			"MAX": 10.0
		},
		{
			"NAME": "fade",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "stripes",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": false
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

const float PI = 3.14159265;

vec2 rotate(vec2 v, float a) {
	float sinA = sin(a);
	float cosA = cos(a);
	return vec2(v.x * cosA - v.y * sinA, v.y * cosA + v.x * sinA); 	
}

float square(vec2 uv, float d) {
	return max(abs(uv.x), abs(uv.y)) - d;	
}



void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
		uv -= vec2(pos);

	uv.x *= RENDERSIZE.x / RENDERSIZE.y;
	uv *= zoom;
	
	uv = rotate(uv, rotation * PI);
	
    float blurAmount = -5.0 / RENDERSIZE.y * (zoom * 0.5);
    
	float time = twist;
	
	gl_FragColor = vec4(0.0, 0.0, 0.0, 1.0);
	for (int i = 0; i < 60; i++) {
		
		float n = float(i);
		float size = 1.0 - n / iterations;
		float rotateAmount = (n * 0.5 + 0.25) * PI * 2.0; 
		gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(1.0), smoothstep(0.0, blurAmount, square(rotate(uv, -rotateAmount * time), size)));
		
		float blackOffset = mix(linesOffset / 4.0, linesOffset / 2.0, n / (iterations * offset)) / (iterations * offset);
		gl_FragColor.rgb = mix(gl_FragColor.rgb, vec3(0.0), smoothstep(0.0, blurAmount, square(rotate(uv, -(rotateAmount + PI / 2.0) * time), size - blackOffset)));
    	
    	
    	
    	if (stripes) {
    		gl_FragColor.rgb = vec3(gl_FragColor.rgb * -1.0 + 1.0) * fade;
       	} else {
       		gl_FragColor.rgb = gl_FragColor.rgb * fade;
       	}
	}
	
	if (invert) gl_FragColor.rgb = vec3(gl_FragColor.rgb *-1.0 + 1.0);

}