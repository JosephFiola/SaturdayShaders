// #SaturdayShader Week 4 : SmoothStep
// by Joseph Fiola (http://www.joefiola.com)
// 2015-09-12
// Based on the "SmoothStep" example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/05/

/*{
	"CREDIT": "Joseph Fiola",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator", "Lines", "Gradients"
	],
	"INPUTS": [
		{
			"NAME": "invertColors",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "left",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "right",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "peak",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "topGradient",
			"TYPE": "float",
			"DEFAULT": 0.01,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "bottomGradient",
			"TYPE": "float",
			"DEFAULT": 0.1,
			"MIN": 0.0,
			"MAX": 1.0
		}
		]
}
*/

#ifdef GL_ES
precision mediump float;
#endif


float plot(vec2 st, float pct){
  return  smoothstep( pct-bottomGradient, pct, st.y) - 
          smoothstep( pct, pct+topGradient, st.y);
}

void main() {
    
    vec2 st = gl_FragCoord.xy/RENDERSIZE;

    // Smooth interpolation between 0.1 and 0.9
    float y = smoothstep(left,0.0+peak,st.x) - smoothstep(1.0-peak,right,st.x);
    
    vec3 color = vec3(0.0 + invertColors); // background color
    float pct = plot(st,y); //plot the line
    color = (1.0-pct)*color+pct*vec3(1.0-invertColors);

    gl_FragColor = vec4(color,1.0);
}