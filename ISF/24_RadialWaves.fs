// SaturdayShader Week 24 : RadialWaves
// by Joseph Fiola (http://www.joefiola.com)
// 2016-01-30

// Based on the animation found at https://media.giphy.com/media/xT9GEDBNxB9PDgicco/giphy.gif



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
			"DEFAULT": true
		},
		{
			"NAME": "shapes",
			"TYPE": "long",
			"VALUES": [
				0,
				1,
				2

			],
			"LABELS": [
				"Circles",
				"Dots",
				"Both"
		
			],
			"DEFAULT": 2
		},
		{
			"NAME": "iteration",
			"TYPE": "float",
			"DEFAULT": 15.0,
			"MIN": 1.0,
			"MAX": 20.0
		},
		{
			"NAME": "dotRadius",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 0.1
		},
		{
			"NAME": "dotThickness",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 0.1
		},
				{
			"NAME": "orbitDistance",
			"TYPE": "float",
			"DEFAULT": 0.9,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "circleRadius",
			"TYPE": "float",
			"DEFAULT": 0.05,
			"MIN": 0.0,
			"MAX": 0.1
		},
		{
			"NAME": "lineThickness",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.05
		},
		{
			"NAME": "gridDistance",
			"TYPE": "float",
			"DEFAULT": 0.055,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": -3.0,
			"MIN": -10.0,
			"MAX": 10.0
		},
				{
			"NAME": "freq",
			"TYPE": "float",
			"DEFAULT": 0.4,
			"MIN": -10.0,
			"MAX": 10.0
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

float circle(vec2 center , float radius,float thickness)
{
	float f = length(center);
	return(smoothstep(f,f+0.002,radius*1.1) * smoothstep(radius - thickness,radius - thickness+0.002,f));
}

float invertColor(float color) {
    return float(color *-1.0 + 1.0);
}


void main()
{
	
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv.x*=RENDERSIZE.x/RENDERSIZE.y;
	
	
	float center = gridDistance;
	
	uv -= vec2(pos - center * ((iteration) * -0.5));
	
	
	float c = 0.0;
	
	// offset to keep grid centered
	float offset = center * iteration; 	

    for(int row = 0; row < 20; row++) {
        if (row > int(iteration)) break;
        
        for(int col = 0; col < 20; col++) {
        if (col > int(iteration)) break;
        
        //draw circles
        if (shapes == 0 || shapes == 2){
        	c += circle(uv+offset	-(vec2(row,col)* gridDistance),circleRadius,lineThickness);
        }

        //draw oscilating dots
        if (shapes == 1 || shapes == 2) {

        	c += circle(uv+offset	-(vec2(
        				sin(
        					(float(-row)*freq) + (float(col)*freq) + TIME*speed) * orbitDistance	+ float(row),
        				-cos(
        					(float(-row)*freq) + (float(col)*freq) + TIME*speed) * orbitDistance	+ float(col)
        				) * gridDistance),
        				dotRadius, dotThickness);
        	}
        }
	}
	
	if (invert) { 
		c = invertColor(c);
	}

	gl_FragColor = vec4(vec3(c), 1.0);
}

