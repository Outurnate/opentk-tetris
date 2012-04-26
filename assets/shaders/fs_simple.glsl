#version 120
 
varying vec2 texture_coordinate;
varying vec3 normal_out;
varying vec3 position_out;
 
uniform sampler2D color_texture;
 
uniform vec3 lightPosition;
uniform vec4 lightDiffuse;
 
void main()
{
  vec3 normal, lightDir;
  vec4 diffuse;
  float NdotL;
 
  // Normals
  normal = normalize(gl_NormalMatrix * normal_out);
  lightDir = normalize(vec3(position_out-lightPosition));
  NdotL = max(dot(normal, lightDir), 0.0f);
 
  // Colors
  diffuse = gl_FrontMaterial.diffuse * lightDiffuse;
  gl_FragColor = sqrt(texture2D(color_texture, texture_coordinate) * NdotL * diffuse);
}
