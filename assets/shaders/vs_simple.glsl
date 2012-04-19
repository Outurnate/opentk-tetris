varying vec2 texture_coordinate;

uniform vec3 lightPosition = vec3(1.0,  0.0, -1.0);
uniform vec4 lightDiffuse  = vec4(1.0,  1.0,  1.0,  1.0);

void main()
{
  vec3 normal, lightDir;
  vec4 diffuse;
  float NdotL;
  
  // Positions
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  texture_coordinate = vec2(gl_MultiTexCoord0);
  
  // Normals
  normal = normalize(gl_NormalMatrix * gl_Normal);
  lightDir = normalize(vec3(lightPosition));
  NdotL = max(dot(normal, lightDir), 0.0);
  
  // Colors
  diffuse = gl_FrontMaterial.diffuse * lightDiffuse;
  gl_FrontColor = NdotL * diffuse;
}
