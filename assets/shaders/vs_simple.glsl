varying vec2 texture_coordinate;

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
  lightDir = normalize(vec3(gl_LightSource[0].position));
  NdotL = max(dot(normal, lightDir), 0.0);
  
  // Colors
  diffuse = gl_FrontMaterial.diffuse * gl_LightSource[0].diffuse;
  gl_FrontColor = NdotL * diffuse;
}
