// SaturdayShader Week 19 : Truchet Iteration
// by Joseph Fiola (http://www.joefiola.com)
// 2015-12-26

// This shader is a mix of Patricio Gonzalez Vivo's "Truchet Tiles" example
// found at http://patriciogonzalezvivo.com/2015/thebookofshaders/09/ @patriciogv ( patriciogonzalezvivo.com ) - 2015
// as well as last week's "Iteration" shader found at http://www.interactiveshaderformat.com/sketches/746

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
			"NAME": "gradient",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "xScale",
			"TYPE": "float",
			"DEFAULT": 0.49,
			"MIN": 1e-4,
			"MAX": 0.5
		},
		{
			"NAME": "yScale",
			"TYPE": "float",
			"DEFAULT": 0.3,
			"MIN": 1e-4,
			"MAX": 0.5
		},
		{
			"NAME": "tiles",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": 0.0,
			"MAX": 20.0
		},
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.15,
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
		},
		{
			"NAME": "offset",
			"TYPE": "point2D",
			"DEFAULT": [
				0.05,
				0.05
			],
			"MIN": [
				-0.1,
				-0.1
			],
			"MAX": [
				0.1,
				0.1
			]
		},
		{
			"NAME": "scalePos",
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

vec2 rotate2D (vec2 _st, float _angle) {
    _st -= 0.5;
    _st =  mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle)) * _st;
    _st += 0.5;
    return _st;
}

vec2 tile (vec2 _st, float _zoom) {
    _st *= _zoom;
    return fract(_st);
}

vec2 rotateTilePattern(vec2 _st){

    //  Scale the coordinate system by 2x2 
    _st -=scalePos;
    _st *= xScale + yScale  * 0.5;
	//_st += scalePos;

    // Give each cell an index number
    // according to its position
    float index = 0.0;    
    index += step(1., mod(_st.x,2.0));
    index += step(1., mod(_st.y,2.0))*2.0;

    // Make each cell between 0.0 - 1.0
    _st = fract(_st);

    // Rotate each cell according to the index
    if(index == 1.0){
        //  Rotate cell 1 by 90 degrees
        _st = rotate2D(_st,PI*0.5);
    } else if(index == 2.0){
        //  Rotate cell 2 by -90 degrees
        _st = rotate2D(_st,PI*-0.5);
    } else if(index == 3.0){
        //  Rotate cell 3 by 180 degrees
        _st = rotate2D(_st,PI);
    }

    return _st;
}


float box(vec2 _st, vec2 _size, float _smoothEdges){
    //_size = vec2(0.0);
    _st += 0.5;
    vec2 aa = vec2(_smoothEdges*0.5);
    vec2 uv = smoothstep(_size,_size+aa,_st);
    uv *= smoothstep(_size,_size+aa,vec2(1.0)-_st);
    return uv.x*uv.y;
}


vec3 invertColor(vec3 color) {
    return vec3(color *-1.0 + 1.0);
}


void main() {

		vec2 st = gl_FragCoord.xy/RENDERSIZE;
		st -= vec2(pos);						
		st.x *= RENDERSIZE.x/RENDERSIZE.y;	
		
	    st = tile(st,tiles);
        st = rotateTilePattern(st);
        
            // Make more interesting combinations
     st = rotate2D(st,-PI*rotate*2.);
     st = rotateTilePattern(st);


		
		vec3 color = vec3(0.0);

		
		for (float i = 0.0; i<50.0; i++){
			vec3 shape = vec3(0.0);
			shape = vec3(box(st-=vec2(offset), vec2(xScale, yScale), gradient*0.05));
			shape *= invertColor(shape);
			color += shape;
		}
		
		
	float finalColor = 0.0;
	if (invert) { 
		color = invertColor(color);
		finalColor = color.x + color.y - color.z;
	} else {
		finalColor = color.x + color.y + color.z;
	}
	
	gl_FragColor = vec4(vec3(finalColor),1.0);
}