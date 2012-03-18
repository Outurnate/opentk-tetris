varying vec2 texture_coordinate;

uniform sampler2D color_texture;

void main()
{
    gl_FragColor = texture2D(color_texture, texture_coordinate) * gl_Color;
}
