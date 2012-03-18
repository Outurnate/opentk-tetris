varying vec2 texture_coordinate;
varying vec3 vertex_normal;

void main()
{
  gl_FrontColor = gl_Color;
  gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
  texture_coordinate = vec2(gl_MultiTexCoord0);
  vertex_normal = normalize(gl_NormalMatrix * gl_Normal);
}
