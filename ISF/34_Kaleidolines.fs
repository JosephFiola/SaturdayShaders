// SaturdayShader Week 34 : Kaleidolines
// Joseph Fiola (http://www.joefiola.com)
// 2016-04-09

// Based on Shadertoy created by Vinicius Graciano Santos - vgs/2014
// https://www.shadertoy.com/view/lsBSDz


/*{
	"CREDIT": "",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT" : "1"
		},
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.25,
			"MAX": 20.0
		},
		{
			"NAME": "rotateCanvas",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "rotateLines",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "lineThickness",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.1,
			"MAX": 20.0
		},
		
		{
			"NAME": "lineLength",
			"TYPE": "float",
			"DEFAULT": 1.25,
			"MIN": 0.05,
			"MAX": 10.0
		},
		{
			"NAME": "lines1",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1.0,
			"MAX": 10.0
		},
		{
			"NAME": "lines2",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1.0,
			"MAX": 10.0
		},
		{
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": -2.0,
			"MAX": 2.0
		},
		{
			"NAME": "motion",
			"TYPE": "float",
			"DEFAULT": 0.25,
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


#define TAU 6.28318530718

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

vec2 tile(vec2 _st, float _zoom){
  _st *= _zoom;
  return fract(_st);
}

float segment(vec2 p, vec2 a, vec2 b) {
    vec2 ab = b - a;
    vec2 ap = p - a;
    float k = clamp(dot(ap, ab)/dot(ab, ab), 0.0, lineLength);
    return smoothstep(0.0, lineThickness/RENDERSIZE.y, length(ap - k*ab) - (0.001 * lineThickness * 10. ));
}

float shape(vec2 p, float angle) {
    float d = 100.0;
    vec2 a = vec2(1.0, 0.0), b;
    vec2 rot = vec2(cos(angle), sin(angle));
    
    for (int i = 0; i < 10; ++i) {
    	        a = a + offset;
    	if (i >= int(lines1)) break;
        b = a;
        for (int j = 0; j < 10; ++j) {
        	if (j >= int(lines2)) break;
        	b = vec2(b.x*rot.x - b.y*rot.y, b.x*rot.y + b.y*rot.x);
        	p = rotate2d(rotateLines* -TAU) * p;
        	d = min(d, segment(p,  a, b));
        }
        a = vec2(a.x*rot.x - a.y*rot.y, a.x*rot.y + a.y*rot.x);

    }
    return d;
}

void main() {

    vec2 uv = gl_FragCoord.xy / RENDERSIZE.xy;
    uv -= vec2(pos);
	uv.x *= RENDERSIZE.x/RENDERSIZE.y;
	uv = rotate2d(rotateCanvas *-TAU) * uv;
	uv *= zoom;
        
    float col = shape(abs(uv), cos((motion * TAU)));
    
    if  (invert) col = col *-1.0 + 1.0;
    
    gl_FragColor = vec4(vec3(col), 1.0);
}