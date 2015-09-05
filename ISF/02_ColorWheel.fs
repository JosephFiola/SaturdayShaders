//#SaturdayShader
//2015-02 ColorWheel
//Based on code from http://patriciogonzalezvivo.com/2015/thebookofshaders/06/
//and from Iñigo Quiles https://www.shadertoy.com/view/MsS3Wc


/*{
	"CREDIT": "by vjzef",
	"DESCRIPTION": "",
	"CATEGORIES": [
			"Generator"
	],
	"INPUTS": [
		{
			"NAME": "rotate",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "multiply",
			"TYPE": "float",
			"DEFAULT": 6.0,
			"MIN": 0.0,
			"MAX": 2000.0
		},
		{
			"NAME": "sharpenLines",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": 1.0,
			"MAX": 1000.0
		},
		{
			"NAME": "invert",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -20.0,
			"MAX": 20.0
		},
		{
			"NAME": "warp",
			"TYPE": "float",
			"DEFAULT": 1.0,
			"MIN": -200.0,
			"MAX": 200.0
		},
		{
			"NAME": "mRadius",
			"TYPE": "float",
			"DEFAULT": 2.0,
			"MIN": -50.0,
			"MAX": 50.0
		},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [
				0.0,
				0.0
			],
			"MIN": [
				-0.5,
				-0.5
			],
			"MAX": [
				0.5,
				0.5
			]
		}
	]
}*/

// multiply 0 - 2000, 6.0 is default

#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718


//  Function from Iñigo Quiles 
//  https://www.shadertoy.com/view/MsS3Wc
vec3 hsb2rgb( in vec3 c ){
    vec3 rgb = clamp(abs(mod(c.x*multiply+vec3(0.0,4.0,2.0),
                             6.0)-3.0)-1.0, 
                     0.0, 
                     1.0 );
    rgb = rgb*rgb*(3.0-2.0*rgb);
    return c.z * mix( vec3(invert), rgb, c.y)*warp;
}

void main(){
    vec2 st = gl_FragCoord.xy/RENDERSIZE;
    vec3 color = vec3(0.0);
    st.x -=pos.x;
    st.y -=pos.y;

    // Use polar coordinates instead of cartesian
    vec2 toCenter = vec2(0.5)-st;
    float angle = atan(toCenter.y,toCenter.x);
    float radius = length(toCenter)*mRadius;
  
    // Map the angle (-PI to PI) to the Hue (from 0 to 1)
    // and the Saturation to the radius
    color = hsb2rgb(vec3((angle/TWO_PI)+rotate,radius*sharpenLines,1.0));

    gl_FragColor = vec4(color,1.0);
}