#version 330 core

in vec3 normal;
in vec2 texCoord;
in vec3 position;
out vec4 fragColor;

uniform vec3 viewPos;

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
uniform Light light;

struct Material {
    sampler2D diffuse;
    sampler2D specular;
    float shininess;
};
uniform Material material;

void main() {
    // 텍스처에서 색상 가져오기
    vec3 texColor = texture(material.diffuse, texCoord).xyz;

    // Ambient 성분
    vec3 ambient = texColor * light.ambient;

    // Diffuse 성분
    vec3 lightDir = normalize(light.position - position);
    vec3 pixelNorm = normalize(normal);
    float diff = max(dot(pixelNorm, lightDir), 0.0);
    vec3 diffuse = diff * texColor * light.diffuse;

    // Specular 성분
    vec3 specColor = texture(material.specular, texCoord).xyz;
    vec3 viewDir = normalize(viewPos - position);
    vec3 reflectDir = reflect(-lightDir, pixelNorm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = specColor * spec * light.specular;

    // 모든 성분 결합
    vec3 result = ambient + diffuse + specular;
    fragColor = vec4(result, 1.0);
}

