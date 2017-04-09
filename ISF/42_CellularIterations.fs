// SaturdayShader #42 : Cellular Iterations
// Joseph Fiola (http://www.joefiola.com)
// 2017-04-08

// Based on the tiling and iteration example by Patricio Gonzalez Vivo from the Book of Shaders' chapter on Cellular Noise
// http://thebookofshaders.com/12/

/*{
	"CREDIT": "",
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
			"NAME": "zoom",
			"TYPE": "float",
			"DEFAULT": 4.0,
			"MIN": 0.0,
			"MAX": 50.0
		},
		{
		"NAME" : "rings",
		"TYPE" : "float",
		"DEFAULT": 20.0,
		"MIN": 0.0,
		"MAX": 200.0
		},
		{
		"NAME" : "thickness",
		"TYPE" : "float",
		"DEFAULT": 0.5,
		"MIN": 0.001,
		"MAX": 0.995
		},
		{
			"NAME": "grid",
			"TYPE": "float",
			"DEFAULT": 0.025,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
	      "NAME": "gridColor",
	      "TYPE": "bool",
	      "DEFAULT":1
	    },
		{
      	"NAME": "rotate",
      	"TYPE": "float",
      	"DEFAULT": 0.0,
      	"MIN": 0,
      	"MAX": 1
    	},
		{
			"NAME": "pos",
			"TYPE": "point2D",
			"DEFAULT": [0.5,0.5],
			"MIN":[0.0,0.0],
			"MAX":[1.0,1.0]
		},
		{
			"NAME": "time",
			"TYPE": "float",
			"DEFAULT": 0.0,
			"MIN": 0.0,
			"MAX": 1.0
		},
		{
			"NAME": "amplitude",
			"TYPE": "float",
			"DEFAULT": 0.5,
			"MIN": 0.0,
			"MAX": 0.5
		},
		{
			"NAME": "offsets",
			"TYPE": "point2D",
			"DEFAULT": [0.0,0.0],
			"MIN":[-2.0,-2.0],
			"MAX":[2.0,2.0]
		}

	]
}*/


#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718

vec2 random2( vec2 p ) {
    return fract(sin(vec2(dot(p,vec2(127.1,311.7)),dot(p,vec2(269.5,183.3))))*43758.5453);
}

mat2 rotate2d(float _angle){
    return mat2(cos(_angle),-sin(_angle),
                sin(_angle),cos(_angle));
}

void main() {
	float animate = time * TWO_PI;
    vec2 st = gl_FragCoord.xy/RENDERSIZE.xy;
    
    st -= vec2(pos.x,pos.y); //center uv to pos location

    
    st.x *= RENDERSIZE.x/RENDERSIZE.y; // 1:1 ratio
    
        st = rotate2d(rotate*-TWO_PI) * st;
    
    float color = 0.0;
    
    // Scale 
    st *= zoom;
    
    // Tile the space
    vec2 i_st = floor(st);
    vec2 f_st = fract(st);

    float m_dist = 1.;  // minimun distance
    
    for (int y= -1; y <= 1; y++) {
        for (int x= -1; x <= 1; x++) {
            // Neighbor place in the grid
            vec2 neighbor = vec2(float(x),float(y));
            
            // Random position from current + neighbor place in the grid
            vec2 point = random2(i_st + neighbor);

			// Animate the point
            //point = 0.5 + 0.5*sin(animate + 6.2831*point);
            point = 0.5 + amplitude*sin(animate + TWO_PI * point)+(offsets);
            
			// Vector between the pixel and the point
            vec2 diff = neighbor + point - f_st;
            
            // Distance to the point
            float dist = length(diff);

            // Keep the closer distance
            m_dist = min(m_dist, dist);
        }
    }

    // Draw the min distance (distance field)
    color += m_dist;

    // Draw cell center
    //color += 1.-step(.02, m_dist);
    
    // Draw grid
    if(grid > 0.0) {
    	float thick = 1.0 - grid;
    	if (gridColor) color = color *-1.0 + 1.0;
    	color -= step(thick, f_st.x) + step(thick, f_st.y);
    	if (gridColor) color = color *-1.0 + 1.0;
    }
    
    // Show isolines
    if(rings > 0.0) color += smoothstep(thickness,thickness+(thickness*0.002*zoom)+(rings*0.002*zoom),(0.8,abs(sin(rings*m_dist)))); 
    
     //invert colors
    if  (invert) color = color *-1.0 + 1.0;

    
    gl_FragColor = vec4(vec3(color),1.0);
}