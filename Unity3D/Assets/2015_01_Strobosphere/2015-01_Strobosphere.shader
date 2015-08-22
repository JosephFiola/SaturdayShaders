//#SaturdayShaders
//2015-01_Strobosphere
//by VJZEF (Joseph Fiola) - visit VJZEF.com & JoeFiola.com

//Shader code based on http://patriciogonzalezvivo.com/2015/thebookofshaders/07/
//GLSL to Unity 3D template found on https://en.wikibooks.org/wiki/GLSL_Programming/Unity/Minimal_Shader


Shader "SaturdayShaders/2015/01_Strobosphere" {


 Properties {
      _Roundness ("Roundness", Range (0.0, 1.0)) = 0.5
      _Scale1 ("Scale 1", Range (0.0, 2.0)) = 0.2
      _Scale2 ("Scale 2", Range (0.0, 1.0)) = 0.8
      _Speed ("Speed", Range (-30.0, 100.0)) = 2.0
      _Outline ("Outline", Range (0.00, 0.99)) = 0.0
      } 


   SubShader { // Unity chooses the subshader that fits the GPU best
   

   
      Pass { // some shaders require multiple passes
      
      
      
         GLSLPROGRAM // here begins the part in Unity's GLSL
         #include "UnityCG.glslinc"
         
         


         
   
		uniform float _Roundness;
		uniform float _Scale1;
		uniform float _Scale2;
		uniform float _Speed;
		uniform float _Outline;
		
		varying vec4 uv;
        

         #ifdef VERTEX // here begins the vertex shader
         void main() // all vertex shaders define a main() function
         {
         // uv =  gl_Vertex + vec4(0.5, 0.5, 0.5, 0.0);
               
               uv = gl_MultiTexCoord0;
               //uv = gl_ModelViewProjectionMatrix * gl_Vertex;
               gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
               // this line transforms the predefined attribute 
               // gl_Vertex of type vec4 with the predefined
               // uniform gl_ModelViewProjectionMatrix of type mat4
               // and stores the result in the predefined output 
               // variable gl_Position of type vec4.
         }
         #endif // here ends the definition of the vertex shader



         #ifdef FRAGMENT // here begins the fragment shader
         void main(){
			
		  vec2 st = uv.xy;
		 //st.y+= 0.0;
		 // st.x *= _ScreenParams.x/_ScreenParams.y;
		  //st -= mouse;
		  
		  //st.x += -0.5;
		  vec3 color = vec3(0.0);
		  float d = 0.0;

		  // Remap the space to -1. to 1.
		  st = st * 2.-1.;

		  // Make the distance field
		   d = length( max(abs(st)-_Roundness, 0.) );
		   
		   d = smoothstep(_Scale1,_Scale2,d);
		   
		   float math = float(fract(d*sin(d+_Time*_Speed)));
		   
		   color.x = math;
		   color.y = math;
		   color.z = math;
		   
		   
		  // color = vec3(fract(d*sin(d+_Time*_Speed)));
		   
		   
		   
		   
		   if (_Outline > 0.0) {
		   color = step(_Outline, color);
		   }
		   

		  // Visualize the distance field
		  gl_FragColor = vec4(color, 0.5);

		}
         #endif // here ends the definition of the fragment shader



         ENDGLSL // here ends the part in GLSL 
      }
   }
  // Fallback "Unlit/Texture"
}