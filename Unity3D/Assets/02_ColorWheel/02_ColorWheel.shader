//#SaturdayShader week 2
//2015-02_ColorWheel
//by VJZEF (Joseph Fiola) - visit VJZEF.com & JoeFiola.com

//Shader code based on http://patriciogonzalezvivo.com/2015/thebookofshaders/07/
//GLSL to Unity 3D template found on https://en.wikibooks.org/wiki/GLSL_Programming/Unity/Minimal_Shader


Shader "SaturdayShaders/2015/02_ColorWheel" {


 Properties {
      _Rotate		("Rotate", Range (0.0, 1.0)) = 0.0
      _Multiply		("Multiply", Range (0.0, 2000.0)) = 6.0
      _SharpenLines	("SharpenLines", Range (1.0, 100.0)) = 1.0
      _Invert		("Invert", Range (-20.0, 20.0)) = 1.0
      _Warp			("Warp", Range (-200.0, 200.0)) = 1.0
      _mRadius		("mRadius", Range (0.0, 50.0)) = 2.0
      _xPos			("xPos", Range (-0.5, 0.5)) = 0.0
      _yPos			("yPos", Range (-0.5, 0.5)) = 0.0
      } 


   SubShader { // Unity chooses the subshader that fits the GPU best
    
      Pass { // some shaders require multiple passes
      
        GLSLPROGRAM // here begins the part in Unity's GLSL
        #include "UnityCG.glslinc" 
         
        #define TWO_PI 6.28318530718    
   
		uniform float _Rotate;		
		uniform float _Multiply;		
		uniform float _SharpenLines;	
		uniform float _Invert;		
		uniform float _Warp;		
		uniform float _mRadius;		
		uniform float _xPos;	
		uniform float _yPos;	

        varying vec4 uv;

         #ifdef VERTEX // here begins the vertex shader
         void main() // all vertex shaders define a main() function
         {            
               uv = gl_MultiTexCoord0;
               gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            
         }
         #endif // here ends the definition of the vertex shader

         #ifdef FRAGMENT // here begins the fragment shader
         
         vec3 hsb2rgb( in vec3 c ){
    			vec3 rgb = clamp(abs(mod(c.x*_Multiply+vec3(0.0,4.0,2.0),
    			6.0)-3.0)-1.0, 
                0.0, 
                1.0 );
    		rgb = rgb*rgb*(3.0-2.0*rgb);
    		return c.z * mix( vec3(_Invert), rgb, c.y)*_Warp;
			}
     
         void main(){
			vec2 st = uv.xy;
		    vec3 color = vec3(0.0);
		    st.x -=_xPos;
		    st.y -=_yPos;

		    // Use polar coordinates instead of cartesian
		    vec2 toCenter = vec2(0.5)-st;
		    float angle = atan(toCenter.y,toCenter.x);
		    float radius = length(toCenter)*_mRadius;
		  
		    // Map the angle (-PI to PI) to the Hue (from 0 to 1)
		    // and the Saturation to the radius
		    color = hsb2rgb(vec3((angle/TWO_PI)+_Rotate,radius*_SharpenLines,1.0));

		    gl_FragColor = vec4(color,1.0);
	    }
         #endif // here ends the definition of the fragment shader

         ENDGLSL // here ends the part in GLSL 
      }
   }
  // Fallback "Unlit/Texture"
}