vec3 hue2rgb(float hue) {
    return clamp( 
        abs(mod(hue * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 
        0.0, 1.0);
}

void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    vec2 relativePos = fragCoord.xy - (iResolution.xy / 2.0);
    vec2 polar;
    polar.y = sqrt(relativePos.x * relativePos.x + relativePos.y * relativePos.y);
    polar.y /= iResolution.x / 2.0;
    polar.y = 1.0 - polar.y;

    polar.x = atan(relativePos.y, relativePos.x);
    polar.x -= 1.57079632679;
    if(polar.x < 0.0){
		polar.x += 6.28318530718;
    }
    polar.x /= 6.28318530718;
    polar.x = 1.0 - polar.x;
    
    if (polar.y >= 0.5 && polar.y <= 0.7)
        fragColor = vec4(hue2rgb(polar.x), 1.0);
    else
        fragColor = vec4(0.0);
}
