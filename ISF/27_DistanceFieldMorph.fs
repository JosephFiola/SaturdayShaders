// SaturdayShader Week 27 : Distance Field Morphs
// by Joseph Fiola (http://www.joefiola.com)
// 2016-02-20


// Based on @kyndinfo's Distance Field Shapes Transitions http://thebookofshaders.com/edit.html#examples/07/1454255754461.frag
// http://www.kynd.info

// Also used tilling techniques found on @patriciogv's Book of Shaders' patterns page
// http://thebookofshaders.com/09/

/*{
	"CREDIT": "",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 0.5,
			"MAX": 20.0
		},
		{
			"NAME": "centerTile",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "rotateCanvas",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "rotateElements",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "shape1",
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
				"circle",
				"ring",
				"round rectangle",
				"rectangle",
				"capsule",
				"ellipse",
				"triangle",
				"polygon",
				"hexagon"
			],
			"DEFAULT": 1
		},
				{
			"NAME": "shape2",
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
				"circle",
				"ring",
				"round rectangle",
				"rectangle",
				"capsule",
				"ellipse",
				"triangle",
				"polygon",
				"hexagon"
			],
			"DEFAULT": 8
		},
		{
			"NAME": "morph",
			"TYPE": "float",
			"DEFAULT": 1.5,
			"MIN": -2.0,
			"MAX": 3.0
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


#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}


float smoothedge(float v) {
    return smoothstep(0.0, zoom * 4.0 / RENDERSIZE.x, v);
}

float circle(vec2 p, float radius) {
  return length(p) - radius;
}

float rect(vec2 p, vec2 size) {  
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0));
}

float roundRect(vec2 p, vec2 size, float radius) {
  vec2 d = abs(p) - size;
  return min(max(d.x, d.y), 0.0) + length(max(d,0.0))- radius;
}

float ring(vec2 p, float radius, float width) {
  return abs(length(p) - radius) - width;
}

float hexagon(vec2 p, float radius) {
    vec2 q = abs(p);
    return max(abs(q.y), q.x * 0.866025 + q.y * 0.5) - radius;
}

float triangle(vec2 p, float size) {
    vec2 q = abs(p);
    return max(q.x * 0.866025 + p.y * 0.5, -p.y * 0.5) - size * 0.5;
}

float ellipse(vec2 p, vec2 r, float s) {
    return (length(p / r) - s);
}

float capsule(vec2 p, vec2 a, vec2 b, float r) {
    vec2 pa = p - a, ba = b - a;
    float h = clamp( dot(pa,ba)/dot(ba,ba), 0.0, 1.0 );
    return length( pa - ba*h ) - r;
}

//http://thndl.com/square-shaped-shaders.html
float polygon(vec2 p, int vertices, float size) {
    float a = atan(p.x, p.y) + 0.2;
    float b = 6.28319 / float(vertices);
    return cos(floor(0.5 + a / b) * b - a) * length(p) - size;
}

float getShape(vec2 st, int i) {
    if (i == 0) {
        return circle(st, 0.4);
    } else if (i == 1) {
        return ring(st, 0.36, 0.08);
    } else if (i == 2) {
        return roundRect(st, vec2(0.32, 0.24), 0.08);
    } else if (i == 3) {
        return rect(st, vec2(0.4, 0.4));
    } else if (i == 4) {
        return capsule(st, vec2(-0.25, -0.25), vec2(0.25, 0.25), 0.2);
    } else if (i == 5) {
        return ellipse(st, vec2(0.9, 1.2), 0.4);
    } else if (i == 6) {
        return triangle(st, 0.4);
    } else if (i == 7) {
        return polygon(st, 5, 0.4);
    } else {
        return hexagon(st, 0.4);
    }
}

void main() {
   	vec2 st = gl_FragCoord.xy/RENDERSIZE;
   	st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;
	
	st=rotate2d(rotateCanvas * -PI * 2.0) * st;

   	st *= zoom;
   	if (centerTile) st+=0.5; // centers tile
   	st = fract(st);
   	st -=0.5;

    st = rotate2d(rotateElements*-PI *2.0 ) * st;

	vec3 color = vec3(smoothedge(mix(getShape(st, shape1), getShape(st, shape2), morph)));
	
    gl_FragColor = vec4(color, 1.0);
}