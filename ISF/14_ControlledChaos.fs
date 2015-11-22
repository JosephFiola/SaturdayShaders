// SaturdayShader Week 14 : ControlledChaos
// by Joseph Fiola (http://www.joefiola.com)
// 2015-11-21
// Based on Patricio Gonzalez Vivo's "Using the Chaos" example on http://patriciogonzalezvivo.com/2015/thebookofshaders/10/ @patriciogv ( patriciogonzalezvivo.com ) - 2015


/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Random", "Noise"
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
			"NAME": "invert",
			"TYPE": "bool"
		},
		{
			"NAME": "function",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2,
				3,
				4,
				5,
				6,
				7,
				8
			],
			"LABELS": [
				"floor",
				"fract",
				"abs",
				"tan",
				"atan",
				"sin",
				"mod",
				"mod grid",
				"clamp"
			],
			"DEFAULT": 8
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "multiplier",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "grid",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1e-4,
			"MAX": 20.0
		},
		{
			"NAME": "detail",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 1e-4,
			"MAX": 0.1
		},

		{
			"NAME": "contrast",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "contrastShift",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.5,
			"MAX": 0.5
		},
		{
			"NAME": "mode",
			"TYPE": "long",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"clean",
				"noisy"
			],
			"DEFAULT": 0
		}
	]
}*/



#ifdef GL_ES
precision mediump float;
#endif



float random (vec2 st) { 
	if (mode == 0) 			return sin(dot(st.xy + TIME*speed, vec2(12.9898, 78.233 * 2. *multiplier))) * 40.* 1.* detail;
	else if (mode == 1)  	return fract(sin(dot(st.xy + TIME*speed *0.0001, vec2(12.9898,78.233*2.*multiplier)))* 43758.5453123*1.* detail);
}


 vec3 invertColor(vec3 color) {
    return vec3(color *-1.0 + 1.0);
 	
 }
 

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y; // 1:1 ratio
	

    st *= grid; // Scale the coordinate system
    
    
    vec2 ipos = st;
	if (function == 0) ipos = floor(st);
	else if (function == 1) ipos = fract(st);
	else if (function == 2) ipos = abs(st);
	else if (function == 3) ipos = tan(st);
	else if (function == 4) ipos = atan(st);
	else if (function == 5) ipos = sin(st);
	else if (function == 6) ipos = mod(st.xy, st.yx);
	else if (function == 7) ipos = mod(st.xy, st.xy);
	else if (function == 8) ipos = clamp(st.xy, vec2(-2.0 + multiplier), vec2(2.0 - multiplier));

	
	
    vec2 fpos = fract(st);  // get the fractional coords

    // Assign a random value based on the integer coord
    vec3 color = vec3(random(ipos));


	//adjust contrast
	color = smoothstep(0.0+contrast+contrastShift,1.0-contrast+contrastShift, color);
	
	//invert colors
	if (invert) color = invertColor(color);

    gl_FragColor = vec4(color,1.0);
}

