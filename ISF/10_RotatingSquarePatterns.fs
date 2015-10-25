// SaturdayShader Week 10 : RotatingSquarePatterns
// by Joseph Fiola (http://www.joefiola.com)
// 2015-10-24

// Based on Patricio Gonzalez Vivo's "Apply Matrices Inside Patterns" example
// on http://patriciogonzalezvivo.com/2015/thebookofshaders/09/ @patriciogv ( patriciogonzalezvivo.com ) - 2015



/*{
    "CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Patterns", "Boxes"
			],
	"INPUTS": [
		
		{
			"NAME": "invert",
			"TYPE": "bool",
			"DEFAULT": 0.0
		},
		{
			"NAME": "blur1",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.01,
			"MAX": 0.75
		},
		{
			"NAME": "blur2",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.01,
			"MAX": 3.0
		},
		{
			"NAME": "rotate1",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "rotate2",
			"TYPE": "float",
			"DEFAULT": 0.25,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "tile1",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 50.0
		},		
		{
			"NAME": "tile2",
			"TYPE": "float",
			"DEFAULT": 3.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
			"NAME": "xScale1",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},	
		{
			"NAME": "yScale1",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "xScale2",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},	
		{
			"NAME": "yScale2",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
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

#ifdef GL_ES
precision mediump float;
#endif


#define PI 3.14159265358979323846

vec2 rotate2D(vec2 _st, float _angle){
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}

vec2 tile(vec2 _st, float _zoom){
    _st *= _zoom;
    return fract(_st);
}

float box(vec2 _st, vec2 _size, float _smoothEdges){
    _size = vec2(0.5)-_size*0.5;
    vec2 aa = vec2(_smoothEdges*0.5);
    vec2 uv = smoothstep(_size,_size+aa,_st);
    uv *= smoothstep(_size,_size+aa,vec2(1.0)-_st);
    return uv.x*uv.y;
}


void main(void){
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
	st -= vec2(pos);						// move the origin - vec2(0.5) will center it
	st.x *= RENDERSIZE.x/RENDERSIZE.y;		// make things 1:1 ratio regardless of canvas ratio
	vec3 color = vec3(0.0);					// instantiate color variable

    // Divide the space
    st = tile(st,tile1);   

    // Use a matrix to rotate the space 45 degrees
    st = rotate2D(st,PI*rotate1);

    // Draw a square
    color = vec3(box(st,vec2(xScale1,yScale1),blur1));
	
   	//Second set of squares
    // Center second set of squares
    st -= vec2(0.5);
    
    st = tile(st,tile2);
  
    st = rotate2D(st,PI*rotate2);
    color += vec3(box(st,vec2(xScale2, yScale2),blur2));
   
   
    // invert colors
    if (invert)
    {color = vec3(color *-1.0 + 1.0);}

    gl_FragColor = vec4(color,1.0);    
}