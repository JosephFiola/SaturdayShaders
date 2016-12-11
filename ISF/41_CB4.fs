// SaturdayShader #41 : CB4
// Joseph Fiola (http://www.joefiola.com)
// 2016-12-10

// Based on "4 cells DF" example by Patricio Gonzalez Vivo from the Book of Shaders' chapter on Cellular Noise
// http://thebookofshaders.com/12/

/*{
	"CREDIT": "",
	"ISFVSN": "2",
	"DESCRIPTION": "",
	"CATEGORIES": [
		"Generators"
	],
		"INPUTS": [
		    {
	      "NAME": "invert",
	      "TYPE": "bool",
	      "DEFAULT":1
	    },
		{
		"NAME" : "spread",
		"TYPE" : "float",
		"DEFAULT": 1.0,
		"MIN": 0.0,
		"MAX": 1.0
		},
		{
		"NAME" : "rings",
		"TYPE" : "float",
		"DEFAULT": 100.0,
		"MIN": 0.0,
		"MAX": 400.0
		},
		{
		"NAME" : "thickness",
		"TYPE" : "float",
		"DEFAULT": 0.5,
		"MIN": 0.001,
		"MAX": 0.995
		},
		{
		"NAME" : "fade",
		"TYPE" : "float",
		"DEFAULT": 1.5,
		"MIN": -2.0,
		"MAX": 2.0
		},
		{      
		"NAME": "point1pos",
		"TYPE": "point2D",
		"DEFAULT":[0,1],
		"MAX": [1,1],
      	"MIN": [0,0]
		},
		{
		"NAME": "point2pos",
		"TYPE": "point2D",
		"DEFAULT": [1,1],
		"MAX": [1,1],
		"MIN": [0,0]
		},
		{
		"NAME": "point3pos",
		"TYPE": "point2D",
		"DEFAULT": [0,0],
		"MAX": [1,1],
      	"MIN": [0,0]
		},
		{
		"NAME": "point4pos",
		"TYPE": "point2D",
		"DEFAULT": [1,0],
		"MAX": [1,1],
    	"MIN": [0,0]
		},
		{
		"NAME" : "canvas_xPos",
		"TYPE" : "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.0
		},
		{
		"NAME" : "canvas_yPos",
		"TYPE" : "float",
		"DEFAULT": 0.5,
		"MIN": 0.0,
		"MAX": 1.0
		},
		{
		"NAME" : "zoom",
		"TYPE" : "float",
		"DEFAULT": 2.5,
		"MIN": 1.0,
		"MAX": 4.0
		},
		{
      	"NAME": "rotate",
      	"TYPE": "float",
      	"DEFAULT": 0,
      	"MIN": 0,
      	"MAX": 1
    }
	]
}*/

#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718


mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}



void main() {
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
   
    st -= vec2(canvas_xPos,canvas_yPos); //center uv to pos location
    st *=zoom;
    
    st.y *= RENDERSIZE.y/RENDERSIZE.x; // make comp square
    
 	st = rotate2d(rotate*-TWO_PI) * st;
 	
 	st +=0.5; // center
    


    vec3 color = vec3(0.0);

    // Cell positions
    vec2 point[4];
    point[0] = point1pos;
    point[1] = point2pos;
    point[2] = point3pos;
    point[3] = point4pos;
    
    float m_dist = spread;  // minimun distance

    // Iterate through the points positions
    for (int i = 0; i < 4; i++) {
        float dist = distance(st, point[i]);
        
        // Keep the closer distance
        m_dist = min(m_dist, dist);
    }
    
    // Draw the min distance (distance field)
    color += m_dist*fade;

    // Show isolines
 
   	color += smoothstep(thickness,thickness+(thickness*0.003*zoom)+(rings*0.003*zoom),(0.8,abs(sin(rings*m_dist)))); 
   
   //invert colors
    if  (invert) color = color *-1.0 + 1.0;

    gl_FragColor = vec4(color,1.0);
}