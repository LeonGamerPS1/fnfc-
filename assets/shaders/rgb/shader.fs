#version 330

in vec2 fragTexCoord;
in vec4 fragColor;

uniform sampler2D texture0;
uniform float redFactor;
uniform float greenFactor;
uniform float blueFactor;

out vec4 finalColor;

void main()
{
    vec4 texColor = texture(texture0, fragTexCoord);
    texColor.r *= redFactor;
    texColor.g *= greenFactor;
    texColor.b *= blueFactor;
    finalColor = texColor;
}
