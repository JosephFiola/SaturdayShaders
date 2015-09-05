// #SaturdayShader Week 3 : SquareStep
// 2015-09-05
// Based on the "Step" example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/05/

/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Geometry"
			
	],
	"INPUTS": [
		{
			"NAME": "left",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "right",
			"TYPE": "float",
			"DEFAULT": 0.9,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "top",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "bottom",
			"TYPE": "float",
			"DEFAULT": 0.9,
			"MIN": 0.0,
			"MAX": 1.0
		}
		]
}
*/

#ifdef GL_ES
precision mediump float;
#endif

void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE;
    float y;
    if(right > left){
		y = step(st.x,right) * step(left, st.x);
    } else if(right < left){
    	y = step(st.x,left) * step(right, st.x);	
    }
    float x;
    if(top>bottom){
    	x = step(st.y, top) * step(bottom, st.y);
    } else if(top < bottom){
    	x = step(st.y, bottom) * step(top, st.y);
    	
    }

    vec3 color = vec3(y * x);

    gl_FragColor = vec4(color,1.0);
}