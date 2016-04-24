// SaturdayShader Week 36 : SpiralCone
// by Joseph Fiola (http://www.joefiola.com)
// 2016-04-23

// Based on "Flailing" Shadertoy by okro
//https://www.shadertoy.com/view/MsjSWw

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
			"DEFAULT": 2.0,
			"MIN": 0.25,
			"MAX": 10.0
		},
		{
			"NAME": "rings",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.01,
			"MAX": 1.0
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.001,
			"MAX": 2.0
		},
		{
			"NAME": "xAmp",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.1,
			"MAX": 0.1
		},
		{
			"NAME": "xOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "xOffsetSpeed",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.1
		},
				{
			"NAME": "yAmp",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -0.1,
			"MAX": 0.1
		},
		{
			"NAME": "yOffset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -1.0,
			"MAX": 1.0
		},
		{
			"NAME": "yOffsetSpeed",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 0.1
		},
		{
			"NAME": "startPoint",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 300.0
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


#define TWO_PI 6.28318530718

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main()
{
	vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
	uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv = rotate2d(rotate*-TWO_PI) * uv;
   	uv *= zoom;

    vec3 col = vec3(0.);
    vec2 loc = uv * 0.0;

    float radius = radius+rings;


    for (int i = 0; i < 100; ++i) {
        float r = smoothstep(radius, radius+.004, distance(uv,loc));
      
        col = 1.0 - col * r;

        //move circles
        float dx = xAmp;
       	dx += cos((startPoint + TIME) * xOffsetSpeed * float(i)) * xOffset;
		
       	float dy = yAmp;
        dy += sin((startPoint + TIME) * yOffsetSpeed * float(i)) * yOffset;
        
		loc += vec2(dx, dy);
        
        
        //make smaller
        radius -= rings;
    }
    
    if  (invert) col = col *-1.0 + 1.0;
	gl_FragColor = vec4(col, 1.0);
}