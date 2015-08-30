//#SaturdayShader
//2015-01 StroboSphere
//Based on code from http://patriciogonzalezvivo.com/2015/thebookofshaders/07/


/*{
	"CREDIT": "by vjzef",
	"DESCRIPTION": "Stroboscopic sphere",
	"CATEGORIES": [
			"Generator"						
	],
	"INPUTS": [
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [
				0.5,
				0.5
			]
		},
		{
			"NAME": "speed",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": -3.0,
			"MAX": 10.0
		},
		{
			"NAME": "scale1",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 2.0
		},
		{
			"NAME": "scale2",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "outline",
			"TYPE": "float",
			"DEFAULT": 0.99,
			"MIN": 0.0,
			"MAX": 0.99
		},	
		{
			"NAME": "roundness",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		}
	]
}*/



void main(){
  vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
  st.x *= RENDERSIZE.x/RENDERSIZE.y;
  
  //not sure how to go about centering this to match the point2D XY, (Y is not perfectly centered)
  st.y+= 0.5;
  st -= pos/RENDERSIZE;
  
  
  //st.x += -0.5;
  vec3 color = vec3(0.0);
  float d = 0.0;

  // Remap the space to -1. to 1.
  st = st * 2.-1.;


  // Make the distance field
   d = length( max(abs(st)-roundness,0.) );
   
   d = smoothstep(scale1,scale2,d);
   
   color = vec3(fract(d*sin(d+TIME*speed)));
   
   if (outline > 0.0)
   color = step(outline, color);
   

  // Visualize the distance field
  gl_FragColor = vec4(color,1.0);

}