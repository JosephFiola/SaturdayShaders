// SaturdayShader Week 28 : Turbulent Shapes
// by Joseph Fiola (http://www.joefiola.com)
// 2016-02-27

// This is a remix of Patricio Gonzalez Vivo's
// VectorField - http://patriciogonzalezvivo.github.io/glslEditor/?log=160224011512
// and @kyndinfo's Distance Field Transitions http://thebookofshaders.com/edit.html?log=160131053646

/*{
	"CREDIT": "Joseph Fiola, based on work by Patricio Gonzalez and @kyndinfo",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generator"
	],
	"INPUTS": [
		{
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 20.0,
			"MIN": 1.0,
			"MAX": 60.0
		},
		{
			"NAME": "scale",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": -4.0,
			"MAX": 4.0
		},
		{
			"NAME": "spin",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 10.0
		},
		{
			"NAME": "turbulanceSpeed",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "turbulanceZoom",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "shape",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 8.0
		},
		{
			"NAME": "rotateCanvas",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "centerTile",
			"TYPE": "bool",
			"DEFAULT": true
		},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [0.5,0.5],
			"MIN":[0.0,0.0],
			"MAX":[1.0,1.0]
		},
		{
			"NAME": "posOffset",
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
#define TWO_PI 6.28318530718

float smoothedge(float v) {
    return smoothstep(0.0, 1.0 / RENDERSIZE.x, v) * -1.0 +1.0;
}

float circle(vec2 p, float radius) {
  return length(p) - radius;
}

float ring(vec2 p, float radius, float width) {
  return abs(length(p) - radius) - width;
}

//http://thndl.com/square-shaped-shaders.html
float polygon(vec2 p, int vertices, float size) {
    float a = atan(p.x, p.y) + 0.2;
    float b = 6.28319 / float(vertices);
    return cos(floor(0.5 + a / b) * b - a) * length(p) - size;
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

vec3 mod289(vec3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
vec2 mod289(vec2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
vec3 permute(vec3 x) { return mod289(((x*34.0)+1.0)*x); }

float snoise(vec2 v) {
    const vec4 C = vec4(0.211324865405187,  // (3.0-sqrt(3.0))/6.0
                        0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
                        -0.577350269189626,  // -1.0 + 2.0 * C.x
                        0.024390243902439); // 1.0 / 41.0
    vec2 i  = floor(v + dot(v, C.yy) );
    vec2 x0 = v -   i + dot(i, C.xx);
    vec2 i1;
    i1 = (x0.x > x0.y) ? vec2(1.0, 0.0) : vec2(0.0, 1.0);
    vec4 x12 = x0.xyxy + C.xxzz;
    x12.xy -= i1;
    i = mod289(i); // Avoid truncation effects in permutation
    vec3 p = permute( permute( i.y + vec3(0.0, i1.y, 1.0 ))
        + i.x + vec3(0.0, i1.x, 1.0 ));

    vec3 m = max(0.5 - vec3(dot(x0,x0), dot(x12.xy,x12.xy), dot(x12.zw,x12.zw)), 0.0);
    m = m*m ;
    m = m*m ;
    vec3 x = 3.0 * fract(p * C.www) - 1.0;
    vec3 h = abs(x) - 0.5;
    vec3 ox = floor(x + 0.5);
    vec3 a0 = x - ox;
    m *= 1.79284291400159 - 0.85373472095314 * ( a0*a0 + h*h );
    vec3 g;
    g.x  = a0.x  * x0.x  + h.x  * x0.y;
    g.yz = a0.yz * x12.xz + h.yz * x12.yw;
    return 130.0 * dot(m, g);
}

float getShape(vec2 fpos, int i, float scale) {
    if (i == 0) {
        return polygon(fpos, 2, scale); //rectangle
    } else if (i == 1) {
        return polygon(fpos, 1, scale); //line
    } else if (i == 2) {
        return polygon(fpos, 3, scale); //triangle
    } else if (i == 3) {
        return polygon(fpos, 4, scale); //square
    } else if (i == 4) {
        return polygon(fpos, 6, scale); //hexagon
    } else if (i == 5) {
        return circle(fpos, scale);		//circle
    } else if (i == 6) {
        return ring(fpos, scale,scale*0.25); //ring1
    } else if (i ==7) {
        return ring(fpos, scale * 2.0 ,scale); //ring2
	} else {
        return polygon(fpos, 4, scale); //square
    }
}

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
   	st -= vec2(pos);
	st.x *= RENDERSIZE.x/RENDERSIZE.y;

    st = rotate2d(rotateCanvas*-TWO_PI) * st; 


    st *= zoom; // Scale the coordinate system by 10
    
   	if (centerTile) st+=0.5; // centers tile

    vec2 ipos = floor(st);  // get the integer coords
    vec2 fpos = fract(st);  // get the fractional coords
    
    float angle = snoise(ipos*turbulanceZoom+TIME*turbulanceSpeed)*spin;
    float scale = snoise(ipos*turbulanceZoom+TIME*turbulanceSpeed)*scale;
    
    fpos -= posOffset;
    fpos = rotate2d(angle*PI)* fpos;

    float t0 = mod(shape, 9.0);
    float t1 = mod(shape + 1.0, 9.0);
    int i0 = int(floor(t0));
    int i1 = int(floor(t1));
    float f = fract(t0);
    
    vec3 color = vec3(smoothedge(mix(getShape(fpos, i0, scale), getShape(fpos, i1, scale), f)));

    gl_FragColor = vec4(color,1.0);
}