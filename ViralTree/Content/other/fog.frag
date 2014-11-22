uniform sampler2D tex;
uniform sampler2D noise;

uniform vec2 offset;
uniform vec2 worldSize;
uniform vec2 playerPos;
uniform float scale;

void main(void){

	vec2 pos = vec2(gl_TexCoord[0]);

	vec4 color = texture2D(tex, pos);
	vec2 tmpOffset = vec2(offset.x, -offset.y);
	vec4 color2 = texture2D(noise, pos * 0.1 * scale + tmpOffset / (worldSize * 4));
	color2 += texture2D(noise, pos * 0.2 * scale + tmpOffset / (worldSize * 4)) * 0.5;
	color2 += texture2D(noise, pos * 0.4 * scale + tmpOffset / (worldSize * 4)) * 0.25;
	color2 /= 1.75;

	

	vec2 dir = (playerPos - offset) / vec2(500 * scale, 375 * scale);

	float d = distance(vec2(0.5, 0.5), pos - vec2(dir.x, -dir.y)) * scale / 0.75;

	//float p = d ;

	gl_FragColor = color + color2 * vec4(d, d, d, 1);
}
