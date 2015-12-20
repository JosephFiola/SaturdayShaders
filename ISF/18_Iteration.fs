// SaturdayShader Week 18 : Iteration
// by Joseph Fiola (http://www.joefiola.com)
// 2015-12-19


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
			"DEFAULT": 0.03,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "radius",
			"TYPE": "float",
			"DEFAULT": 10.0,
			"MIN": 1e-4,
			"MAX": 50.0
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
			"NAME": "offsetPos",
			"TYPE": "point2D",
			"DEFAULT": [
				0.0,
				0.0
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
			"NAME": "offset",
			"TYPE": "float",
			"DEFAULT": 0.2,
			"MIN": 1e-4,
			"MAX": 1.0
		},
		{
			"NAME": "shapeSelect",
			"TYPE": "long",
			"VALUES": [
				0,
				1
			],
			"LABELS": [
				"circle",
				"square"
			],
			"DEFAULT": 0
		}
	]
}*/

float circle(in vec2 _st, in float _radius){
    vec2 l = _st;
    return 1.-smoothstep(_radius-(_radius*gradient),
                         _radius+(_radius*gradient),
                         dot(l,l)*radius);
}

float box(vec2 _st, vec2 _size, float _smoothEdges){
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
		
		vec3 color = vec3(0.0);		
		
		for (float i = 0.0; i<50.0; i++){
			vec3 shape = vec3(0.0);
			 if (shapeSelect == 0) shape = vec3(circle(st-=vec2(offsetPos), (offset * i)));
			 if (shapeSelect == 1) shape = vec3(box(st-=vec2(offsetPos),vec2(radius*0.009*offset,radius*0.009*offset),gradient*0.05));

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