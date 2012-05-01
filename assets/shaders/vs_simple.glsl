#version 120
 
varying vec2 texture_coordinate;
varying vec3 normal_out;
varying vec3 position_out;
 
uniform vec3 lightPosition;
uniform vec4 lightDiffuse;
 
void main() {
  // Positions
  vec4 interpPosition = (gl_ModelViewProjectionMatrix * gl_Vertex);
  gl_Position = interpPosition;
  position_out = interpPosition.xyz;
  texture_coordinate = vec2(gl_MultiTexCoord0);
  normal_out = gl_Normal;
  gl_FrontColor = gl_Color;
}
