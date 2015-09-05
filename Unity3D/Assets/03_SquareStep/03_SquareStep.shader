//#SaturdayShader Week 03: Square Step
//2015-09-05
//by Joseph Fiola	http://www.josephfiola.com

//Based on the "Step" example by Patricio Gonzalez Vivo on http://patriciogonzalezvivo.com/2015/thebookofshaders/05/

Shader "SaturdayShaders/2015/03_SquareStep" {


 Properties {
 
 
      _Left		("Left",	Range (0.0, 1.0)) = 0.1
      _Right	("Right",	Range (0.0, 1.0)) = 0.9
      _Top		("Top",		Range (0.0, 1.0)) = 0.1
      _Bottom	("Bottom",	Range (0.0, 1.0)) = 0.9

      } 


   SubShader { // Unity chooses the subshader that fits the GPU best
    
      Pass { // some shaders require multiple passes
      
        GLSLPROGRAM // here begins the part in Unity's GLSL
        #include "UnityCG.glslinc" 
        
        
         
   
		uniform float _Left;
		uniform float _Right;
		uniform float _Top; 	
		uniform float _Bottom; 

        varying vec4 uv;

         #ifdef VERTEX // here begins the vertex shader
         void main() // all vertex shaders define a main() function
         {            
               uv = gl_MultiTexCoord0;
               gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
            
         }
         #endif // here ends the definition of the vertex shader

         #ifdef FRAGMENT // here begins the fragment shader
         
         #ifdef GL_ES
			precision mediump float;
		 #endif

		void main() {
		    vec2 st = uv.xy;
		    float y;
		    if(_Right > _Left){
				y = step(st.x,_Right) * step(_Left, st.x);
		    } else if(_Right < _Left){
		    	y = step(st.x,_Left) * step(_Right, st.x);	
		    }
		    float x;
		    if(_Top>_Bottom){
		    	x = step(st.y, _Top) * step(_Bottom, st.y);
		    } else if(_Top < _Bottom){
		    	x = step(st.y, _Bottom) * step(_Top, st.y);
		    	
		    }

		    vec3 color = vec3(y * x);

		    gl_FragColor = vec4(color,1.0);
		}

         #endif // here ends the definition of the fragment shader

         ENDGLSL // here ends the part in GLSL 
      }
   }
  // Fallback "Unlit/Texture"
}